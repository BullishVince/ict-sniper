using cAlgo.API;

namespace cAlgo.Indicators
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ICTSwingPoints : Indicator
    {
        [Parameter("Version", DefaultValue = "V0.1")]
        public string Version { get; set; }

        private DebugProps DebugProps { get; set; }

        private const int LookbackPeriod = 5; // Adjust this value for the lookback period
        private readonly List<int> stlIndexes = new();
        private readonly List<int> sthIndexes = new();
        private readonly List<int> itlIndexes = new();
        private readonly List<int> ithIndexes = new();
        private readonly List<int> ltlIndexes = new();
        private readonly List<int> lthIndexes = new();

        protected override void Initialize() => SetupDebug();

        public override void Calculate(int index)
        {
            if (index < LookbackPeriod || index >= Bars.ClosePrices.Count - LookbackPeriod)
                return;

            // Detect Short Term Low (STL)
            if (IsShortTermLow(index))
            {
                stlIndexes.Add(index);
                Chart.DrawText($"STL_{index}", "STL", Bars.OpenTimes[index], Bars.LowPrices[index] - 10 * Symbol.PipSize, Color.Blue);
            }

            // Detect Short Term High (STH)
            if (IsShortTermHigh(index))
            {
                sthIndexes.Add(index);
                Chart.DrawText($"STH_{index}", "STH", Bars.OpenTimes[index], Bars.HighPrices[index] + 10 * Symbol.PipSize, Color.Red);
            }

            // Detect Intermediate Term Low (ITL)
            if (IsIntermediateTermLow(index))
            {
                itlIndexes.Add(index);
                Chart.DrawText($"ITL_{index}", "ITL", Bars.OpenTimes[index], Bars.LowPrices[index] - 20 * Symbol.PipSize, Color.Green);
            }

            // Detect Intermediate Term High (ITH)
            if (IsIntermediateTermHigh(index))
            {
                ithIndexes.Add(index);
                Chart.DrawText($"ITH_{index}", "ITH", Bars.OpenTimes[index], Bars.HighPrices[index] + 20 * Symbol.PipSize, Color.Orange);
            }

            // Detect Long-Term Low (LTL)
            if (IsLongTermLow(index))
            {
                ltlIndexes.Add(index);
                Chart.DrawText($"LTL_{index}", "LTL", Bars.OpenTimes[index], Bars.LowPrices[index] - 30 * Symbol.PipSize, Color.DarkGreen);
            }

            // Detect Long-Term High (LTH)
            if (IsLongTermHigh(index))
            {
                lthIndexes.Add(index);
                Chart.DrawText($"LTH_{index}", "LTH", Bars.OpenTimes[index], Bars.HighPrices[index] + 30 * Symbol.PipSize, Color.DarkRed);
            }
        }

        private bool IsShortTermLow(int index)
        {
            return Bars.LowPrices[index] < Bars.LowPrices[index - 1] && Bars.LowPrices[index] < Bars.LowPrices[index + 1];
        }

        private bool IsShortTermHigh(int index)
        {
            return Bars.HighPrices[index] > Bars.HighPrices[index - 1] && Bars.HighPrices[index] > Bars.HighPrices[index + 1];
        }

        private bool IsIntermediateTermLow(int index)
        {
            return IsShortTermLow(index) && Bars.LowPrices[index] < Bars.LowPrices[index - 2] && Bars.LowPrices[index] < Bars.LowPrices[index + 2];
        }

        private bool IsIntermediateTermHigh(int index)
        {
            return IsShortTermHigh(index) && Bars.HighPrices[index] > Bars.HighPrices[index - 2] && Bars.HighPrices[index] > Bars.HighPrices[index + 2];
        }

        private bool IsLongTermLow(int index)
        {
            return IsIntermediateTermLow(index) && Bars.LowPrices[index] < Bars.LowPrices[index - 3] && Bars.LowPrices[index] < Bars.LowPrices[index + 3];
        }

        private bool IsLongTermHigh(int index)
        {
            return IsIntermediateTermHigh(index) && Bars.HighPrices[index] > Bars.HighPrices[index - 3] && Bars.HighPrices[index] > Bars.HighPrices[index + 3];
        }

        private void SetupDebug()
        {
            //EXPERIMENTAL STUFF 👌
            DebugProps = new DebugProps()
            {
                NumOfElements = Bars.ClosePrices.Count,
                CurrVersion = "V0.1",
                ChartWidth = Chart.Width,
                ChartHeight = Chart.Height,
            };

            var topRightX = Bars.OpenTimes.LastValue.AddDays(-200); // Get the last time point for the top-right corner
            var topRightY = Bars.LowPrices.LastValue - 0.01; // Place the text slightly above the highest value
            Chart.DrawText("debugInformation", DebugProps.ToString(), topRightX, topRightY, Color.Yellow);
        }
    }
}
