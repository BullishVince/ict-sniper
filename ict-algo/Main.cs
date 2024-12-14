using System.Linq;
using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots;

[Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
public class ICTAlgo : Robot
{
    [Parameter(DefaultValue = 30)]
    public int BuyLevel { get; set; }

    [Parameter(DefaultValue = 70)]
    public int SellLevel { get; set; }

    private RelativeStrengthIndex _rsi;

    protected override void OnStart()
    {
        _rsi = Indicators.RelativeStrengthIndex(Bars.ClosePrices, 14);
    }

    protected override void OnBarClosed()
    {
        if (_rsi.Result.LastValue < BuyLevel)
        {
            if (Positions.Count == 0)
                ExecuteMarketOrder(TradeType.Buy, SymbolName, 1000);
            foreach (var position in Positions.Where(p => p.TradeType == TradeType.Sell))
            {
                position.Close();
            }

        }
        
        else if (_rsi.Result.LastValue > SellLevel)
        {
            if (Positions.Count == 0)
                ExecuteMarketOrder(TradeType.Sell, SymbolName, 1000);
            foreach (var position in Positions.Where(p => p.TradeType == TradeType.Buy))
            {
                position.Close();
            }
        }
    }

    protected override void OnStop()
    {

    }
}