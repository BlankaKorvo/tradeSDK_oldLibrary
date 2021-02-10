﻿using Serilog;
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;
using TinkoffData;
using TradingAlgorithms.IndicatorSignals.Helpers;

namespace TradingAlgorithms.IndicatorSignals
{
    class SmaSignal : IndicatorSignalsHelper
    {
        int lookbackPeriod = 8;
        const decimal smaPriceDeltaCount = 0.12M;
        internal bool LongSignal(CandleList candleList, decimal deltaPrice)
        {
            List<SmaResult> sma = Serialization.SmaData(candleList, deltaPrice, lookbackPeriod);
            decimal? smaPriceDelta = 100 - (sma.Last().Sma * 100 / deltaPrice); //Насколько далеко убежала цена от Sma
            if (
                smaPriceDelta < smaPriceDeltaCount
               )
            {
                Log.Information("Проверка на отсутсвие гэпа через SMA");
                Log.Information("smaPriceDelta = " + smaPriceDelta);
                Log.Information("smaPriceDeltaCount = " + smaPriceDeltaCount);
                Log.Information("Должно быть: smaPriceDelta < smaPriceDeltaCount");
                Log.Information("Sma = Long - true");
                return true; 
            }
            else
            {
                Log.Information("Проверка на отсутсвие гэпа через SMA");
                Log.Information("smaPriceDelta = " + smaPriceDelta);
                Log.Information("smaPriceDeltaCount = " + smaPriceDeltaCount);
                Log.Information("Должно быть: smaPriceDelta < smaPriceDeltaCount");
                Log.Information("Sma = Long - falce");
                return false; 
            }
        }


    }
}
