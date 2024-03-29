﻿using MarketDataModules;
using Serilog;
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinkoffData;
using TradingAlgorithms.IndicatorSignals.Helpers;

namespace TradingAlgorithms.IndicatorSignals
{
    public partial class Signal : IndicatorSignalsHelper
    {
        int smaLookbackPeriod = 8;
        const decimal smaPriceDeltaCount = 4M;
        internal bool SmaLongSignal(CandlesList candleList, decimal deltaPrice)
        {
            Log.Information("Start Sma LongSignal. Figi: " + candleList.Figi);
            List<SmaResult> ema = Mapper.SmaData(candleList, deltaPrice, smaLookbackPeriod);
            decimal? smaPriceDelta = ((deltaPrice * 100) / ema.Last().Sma) - 100; //Насколько далеко убежала цена от Sma
            if (
                smaPriceDelta < smaPriceDeltaCount
               )
            {
                Log.Information("Checking for the absence of a gap via SMA");
                Log.Information("Sma = " + ema.Last().Sma + "LPrice = " + deltaPrice);
                Log.Information("smaPriceDelta = " + smaPriceDelta);
                Log.Information("smaPriceDeltaCount = " + smaPriceDeltaCount);
                Log.Information("Should be: smaPriceDelta < smaPriceDeltaCount");
                Log.Information("Sma = Long - true for: " + candleList.Figi);
                return true; 
            }
            else
            {
                Log.Information("Checking for the absence of a gap via SMA");
                Log.Information("Sma = " + ema.Last().Sma + "LPrice = " + deltaPrice);
                Log.Information("smaPriceDelta = " + smaPriceDelta);
                Log.Information("smaPriceDeltaCount = " + smaPriceDeltaCount);
                Log.Information("Should be: smaPriceDelta < smaPriceDeltaCount");
                Log.Information("Sma = Long - falce for: " + candleList.Figi);
                return false; 
            }
        }


    }
}
