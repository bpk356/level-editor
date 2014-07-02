using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelGenerator
{
    class DifficultyEvaluator
    {
        static public double EvaluateDifficulty(Level level, Rect screenBounds, int resolution, out double interestingness, out Dictionary<Vector2, bool> tryDictionary,
            Action updatePictureFunc)
        {
            Random generator = new Random();
            int winCount = 0;
            int lossCount = 0;

            interestingness = 1;

            int offscreenWinCount = 0;
            int onscreenWinCount = 0;
            float minControlledBodyTravelDistanceOnWin = -1.0f;
            float minTravelDistanceOnWin = -1.0f;
            //Dictionary<Vector2, bool>  tryDictionary = new Dictionary<Vector2, bool>();
            int wouldHaveWons = 0;
            tryDictionary = new Dictionary<Vector2, bool>();

            Rect innerScreen = new Rect(screenBounds.MinX + 30, screenBounds.MinY + 30, screenBounds.MaxX - 30, screenBounds.MaxY - 30);

            //if (!level.ControlledBody.IsVisible(innerScreen))
            //{
            //    interestingness = 0;
            //    try2Dictionary = tryDictionary;
            //    return 0;
            //}

            //Enumerable.Range(0, (int)((screenBounds.Width - 80) / resolution)).Select(i => i * resolution + 40)
            //    .AsParallel()
            //    .ForAll(xOffset =>
            List<Tuple<int, int>> outerValues = new List<Tuple<int, int>>();
            for (int outerX = 0; outerX < resolution * 8; outerX++)
            {
                for (int outerY = 0; outerY < resolution * 8; outerY++)
                {
                    outerValues.Add(new Tuple<int,int>(outerX, outerY));
                }
            }
            while(outerValues.Count > 0)
            {
                int outerIndex = generator.Next(outerValues.Count);
                int outerX = outerValues[outerIndex].Item1;
                int outerY = outerValues[outerIndex].Item2;
                    for (int xOffset = 40 + outerX; xOffset < screenBounds.Width - 40; xOffset += resolution * 8)
                    {
                        //Console.WriteLine("Evaluting x = " + xOffset);
                        for (int yOffset = 40 + outerY; yOffset < screenBounds.Height - 40; yOffset += resolution * 8)
                        {
                            EvaluateMousePosition(level, screenBounds, resolution, tryDictionary, updatePictureFunc, ref winCount, ref lossCount, ref offscreenWinCount, ref onscreenWinCount, ref minControlledBodyTravelDistanceOnWin, ref minTravelDistanceOnWin, ref wouldHaveWons, xOffset, yOffset);
                        }
                    }//);
                outerValues.RemoveAt(outerIndex);
            }
            interestingness *= minTravelDistanceOnWin;
            interestingness *= (float)onscreenWinCount / (onscreenWinCount + offscreenWinCount);
            interestingness *= minControlledBodyTravelDistanceOnWin;
            if (winCount > 0)
            {
                interestingness *= (float)wouldHaveWons / (wouldHaveWons + winCount);
            }
            //try2Dictionary = tryDictionary;
            return (double)winCount / (winCount + lossCount);
        }

        private static void EvaluateMousePosition(Level level, Rect screenBounds, int resolution, Dictionary<Vector2, bool> tryDictionary, Action updatePictureFunc, ref int winCount, ref int lossCount, ref int offscreenWinCount, ref int onscreenWinCount, ref float minControlledBodyTravelDistanceOnWin, ref float minTravelDistanceOnWin, ref int wouldHaveWons, int xOffset, int yOffset)
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
                if (noAvoidSimState.Simulate(false, screenBounds, SimulationState.SimulationFramesPerSecond * 15, out hadOffscreen, out controlledBodyTravelDistance, out winPosition))
                {
                    wouldHaveWons += resolution * resolution;
                }
            }
            bool willUpdate = false;
            lock (tryDictionary)
            {
                if ((tryDictionary.Count % 1000) == 0)
                {
                    willUpdate = true;
                }
            }
            if (willUpdate)
            {
                updatePictureFunc();
            }
        }
    }
}
