using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelGenerator
{
    class DifficultyEvaluator
    {
        static public double EvaluateDifficulty(Level level, Rect screenBounds, int resolution, out double interestingness, out Dictionary<Vector2, bool> tryDictionary,
            Func<bool> updatePictureFunc)
        {
            Random generator = new Random();
            int winCount = 0;
            int lossCount = 0;

            interestingness = 1;

            int offscreenWinCount = 0;
            int onscreenWinCount = 0;
            float minControlledBodyTravelDistanceOnWin = -1.0f;
            float minTravelDistanceOnWin = -1.0f;
            tryDictionary = new Dictionary<Vector2, bool>();

            Rect innerScreen = new Rect(screenBounds.MinX + 30, screenBounds.MinY + 30, screenBounds.MaxX - 30, screenBounds.MaxY - 30);
            List<Tuple<int, int>> outerValues = new List<Tuple<int, int>>();
            for (int outerX = 40; outerX < screenBounds.Width - 40; outerX+= resolution)
            {
                for (int outerY = 40; outerY < screenBounds.Height - 40; outerY += resolution)
                {
                    outerValues.Add(new Tuple<int, int>(outerX, outerY));
                }
            }
            while (outerValues.Count > 0)
            {
                Dictionary<Vector2, bool> tempTryDictionary = new Dictionary<Vector2, bool>();
                Enumerable.Range(0, 1000)
                    .AsParallel()
                    .ForAll(throwAway =>
                        {
                            if (outerValues.Count == 0)
                            {
                                return;
                            }
                            int xOffset;
                            int yOffset;
                            lock (outerValues)
                            {
                                int outerIndex = generator.Next(outerValues.Count);
                                xOffset = outerValues[outerIndex].Item1;
                                yOffset = outerValues[outerIndex].Item2;
                                outerValues.RemoveAt(outerIndex);
                            }
                            EvaluateMousePosition(level, screenBounds, resolution, tempTryDictionary, ref winCount, ref lossCount, ref offscreenWinCount, ref onscreenWinCount, ref minControlledBodyTravelDistanceOnWin, ref minTravelDistanceOnWin, xOffset, yOffset);
                        });
                lock (tryDictionary)
                {
                    foreach (var kvp in tempTryDictionary)
                    {
                        tryDictionary.Add(kvp.Key, kvp.Value);
                    }
                }
                bool cancelRequested = updatePictureFunc();
                if (cancelRequested)
                {
                    return 0.0;
                }
            }
            interestingness *= minTravelDistanceOnWin;
            interestingness *= (float)onscreenWinCount / (onscreenWinCount + offscreenWinCount);
            interestingness *= minControlledBodyTravelDistanceOnWin;
            return (double)winCount / (winCount + lossCount);
        }

        private static void EvaluateMousePosition(Level level, Rect screenBounds, int resolution, Dictionary<Vector2, bool> tryDictionary, ref int winCount, ref int lossCount, ref int offscreenWinCount, ref int onscreenWinCount, ref float minControlledBodyTravelDistanceOnWin, ref float minTravelDistanceOnWin, int xOffset, int yOffset)
        {
            Vector2 mousePosition = new Vector2(screenBounds.MinX + xOffset, screenBounds.MinY + yOffset);
            SimulationState simulationState = new SimulationState(level, mousePosition);
            SimulationState noAvoidSimState = new SimulationState(level, mousePosition);


            bool hadOffscreen;
            float controlledBodyTravelDistance;
            Vector2 winPosition;

            bool win = simulationState.Simulate(true, screenBounds, SimulationState.SimulationFramesPerSecond * 15, out hadOffscreen, out controlledBodyTravelDistance, out winPosition);
            if (win)
            {
                lock (tryDictionary)
                {
                    tryDictionary.Add(mousePosition, true);
                }
                if (hadOffscreen)
                {
                    offscreenWinCount++;
                }
                else
                {
                    onscreenWinCount++;
                }
                float coveredDistance = (winPosition - level.ControlledBody.Position).Magnitude();
                controlledBodyTravelDistance = (controlledBodyTravelDistance - coveredDistance) / coveredDistance;
                if (controlledBodyTravelDistance < 0)
                {
                    controlledBodyTravelDistance = 0;
                }
                if (coveredDistance / screenBounds.Width > minTravelDistanceOnWin || minTravelDistanceOnWin == -1.0f)
                {
                    minTravelDistanceOnWin = coveredDistance / screenBounds.Width;
                }
                if (minControlledBodyTravelDistanceOnWin == -1.0f || controlledBodyTravelDistance < minControlledBodyTravelDistanceOnWin)
                {
                    minControlledBodyTravelDistanceOnWin = controlledBodyTravelDistance;
                }
                winCount++;
            }
            else
            {
                lock (tryDictionary)
                {
                    tryDictionary.Add(mousePosition, false);
                }
                lossCount++;
            }
        }
    }
}
