using cAlgo.API;
using cAlgo.API.Indicators;
using System;
using System.Collections.Generic;

namespace cAlgo.Indicators
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ICTSwingPoints : Indicator
    {
        private const int LookbackPeriod = 5; // Adjust this value for the lookback period
        private List<int> stlIndexes = new List<int>();
        private List<int> sthIndexes = new List<int>();
        private List<int> itlIndexes = new List<int>();
        private List<int> ithIndexes = new List<int>();
        private List<int> ltlIndexes = new List<int>();
        private List<int> lthIndexes = new List<int>();

        protected override void Initialize()
        {
            // Initialize any required settings here
        }

        public override void Calculate(int index)
        {
            if (index < LookbackPeriod || index >= MarketSeries.Close.Count - LookbackPeriod)
                return;

            // Detect Short Term Low (STL)
            if (IsShortTermLow(index))
            {
                stlIndexes.Add(index);
                Chart.DrawText($"STL_{index}", "STL", new DateTime(index), MarketSeries.Low[index] - 10 * Symbol.PipSize, Color.Blue);
            }

            // Detect Short Term High (STH)
            if (IsShortTermHigh(index))
            {
                sthIndexes.Add(index);
                Chart.DrawText($"STH_{index}", "STH", new DateTime(index), MarketSeries.High[index] + 10 * Symbol.PipSize, Color.Red);
            }

            // Detect Intermediate Term Low (ITL)
            if (IsIntermediateTermLow(index))
            {
                itlIndexes.Add(index);
                Chart.DrawText($"ITL_{index}", "ITL", new DateTime(index), MarketSeries.Low[index] - 20 * Symbol.PipSize, Color.Green);
            }

            // Detect Intermediate Term High (ITH)
            if (IsIntermediateTermHigh(index))
            {
                ithIndexes.Add(index);
                Chart.DrawText($"ITH_{index}", "ITH", new DateTime(index), MarketSeries.High[index] + 20 * Symbol.PipSize, Color.Orange);
            }

            // Detect Long-Term Low (LTL)
            if (IsLongTermLow(index))
            {
                ltlIndexes.Add(index);
                Chart.DrawText($"LTL_{index}", "LTL", new DateTime(index), MarketSeries.Low[index] - 30 * Symbol.PipSize, Color.DarkGreen);
            }

            // Detect Long-Term High (LTH)
            if (IsLongTermHigh(index))
            {
                lthIndexes.Add(index);
                Chart.DrawText($"LTH_{index}", "LTH", new DateTime(index), MarketSeries.High[index] + 30 * Symbol.PipSize, Color.DarkRed);
            }
        }

        private bool IsShortTermLow(int index)
        {
            return MarketSeries.Low[index] < MarketSeries.Low[index - 1] && MarketSeries.Low[index] < MarketSeries.Low[index + 1];
        }

        private bool IsShortTermHigh(int index)
        {
            return MarketSeries.High[index] > MarketSeries.High[index - 1] && MarketSeries.High[index] > MarketSeries.High[index + 1];
        }

        private bool IsIntermediateTermLow(int index)
        {
            return IsShortTermLow(index) && MarketSeries.Low[index] < MarketSeries.Low[index - 2] && MarketSeries.Low[index] < MarketSeries.Low[index + 2];
        }

        private bool IsIntermediateTermHigh(int index)
        {
            return IsShortTermHigh(index) && MarketSeries.High[index] > MarketSeries.High[index - 2] && MarketSeries.High[index] > MarketSeries.High[index + 2];
        }

        private bool IsLongTermLow(int index)
        {
            return IsIntermediateTermLow(index) && MarketSeries.Low[index] < MarketSeries.Low[index - 3] && MarketSeries.Low[index] < MarketSeries.Low[index + 3];
        }

        private bool IsLongTermHigh(int index)
        {
            return IsIntermediateTermHigh(index) && MarketSeries.High[index] > MarketSeries.High[index - 3] && MarketSeries.High[index] > MarketSeries.High[index + 3];
        }
    }
}
