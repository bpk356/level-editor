using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelGenerator
{
    class DifficultyEvaluator
    {

        //static public double EvaluateDifficultyRandomly(Level level, Rect screenBounds, int simulationCount, Random generator)
        //{
        //    int winCount = 0;
        //    int lossCount = 0;

        //    for (int simulationI = 0; simulationI < simulationCount; simulationI++)
        //    {
        //        int xOffset = generator.Next((int)screenBounds.Width);
        //        int yOffset = generator.Next((int)screenBounds.Height);
        //        Vector2 mousePosition = new Vector2(screenBounds.MinX + xOffset, screenBounds.MinY + yOffset);
        //        SimulationState simulationState = new SimulationState(level, mousePosition);


        //        bool win = simulationState.Simulate(screenBounds, SimulationState.SimulationFramesPerSecond * 120);
        //        if (win)
        //        {
        //            winCount++;
        //        }
        //        else
        //        {
        //            lossCount++;
        //        }
        //    }
        //    return (double)winCount / (winCount + lossCount);
        //}

        static public double EvaluateDifficulty(Level level, Rect screenBounds, int resolution, out double interestingness, out Dictionary<Vector2, bool> try2Dictionary)
        {
            int winCount = 0;
            int lossCount = 0;

            interestingness = 1;

            int offscreenWinCount = 0;
            int onscreenWinCount = 0;
            float minControlledBodyTravelDistanceOnWin = -1.0f;
            float minTravelDistanceOnWin = -1.0f;
            Dictionary<Vector2, bool>  tryDictionary = new Dictionary<Vector2, bool>();
            int wouldHaveWons = 0;

            Rect innerScreen = new Rect(screenBounds.MinX + 30, screenBounds.MinY + 30, screenBounds.MaxX - 30, screenBounds.MaxY - 30);

            //if (!level.ControlledBody.IsVisible(innerScreen))
            //{
            //    interestingness = 0;
            //    try2Dictionary = tryDictionary;
            //    return 0;
            //}

            Enumerable.Range(0, (int)((screenBounds.Width - 80) / resolution)).Select(i => i * resolution + 40)
                .AsParallel()
                .ForAll(xOffset =>
            {
                //Console.WriteLine("Evaluting x = " + xOffset);
                for (int yOffset = 40; yOffset < screenBounds.Height - 40; yOffset += resolution)
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
                }
            });
            interestingness *= minTravelDistanceOnWin;
            interestingness *= (float)onscreenWinCount / (onscreenWinCount + offscreenWinCount);
            interestingness *= minControlledBodyTravelDistanceOnWin;
            if (winCount > 0)
            {
                interestingness *= (float)wouldHaveWons / (wouldHaveWons + winCount);
            }
            try2Dictionary = tryDictionary;
            return (double)winCount / (winCount + lossCount);
        }
    }
}
