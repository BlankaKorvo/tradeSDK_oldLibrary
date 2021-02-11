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
    class AroonSignal : IndicatorSignalsHelper
    {
        int lookbackPeriod = 7;
        decimal aroonUpValueLong = 100;
        decimal aroonUpValueFromLong = 50;

        decimal aroonDownValue = 50;
        internal bool LongSignal(CandleList candleList, decimal deltaPrice)
        {
            List<AroonResult> aroon = Serialization.AroonData(candleList, deltaPrice, lookbackPeriod);

            if (
                aroon.Last().AroonUp > aroon.Last().AroonDown
                &&
                aroon.Last().AroonUp == aroonUpValueLong
                &&
                aroon.Last().AroonDown < aroonDownValue

                )
            {
                Log.Information("Aroon Up = " + aroon.Last().AroonUp + " " + aroon.Last().Date);
                Log.Information("Aroon Down = " + aroon.Last().AroonDown + " " + aroon.Last().Date);
                Log.Information("Aroon Oscillator = " + aroon.Last().Oscillator + " " + aroon.Last().Date);
                Log.Information("Should be: " + aroon.Last().AroonUp + " > " + aroon.Last().AroonDown);
                Log.Information("Should be: " + aroon.Last().AroonUp + " = " + aroonUpValueLong);
                Log.Information("Should be: " + aroon.Last().AroonDown + " < " + aroonDownValue);
                Log.Information("Aroon = Long - true");
                return true;
            }
            else
            {
                Log.Information("Aroon Up = " + aroon.Last().AroonUp + " " + aroon.Last().Date);
                Log.Information("Aroon Down = " + aroon.Last().AroonDown + " " + aroon.Last().Date);
                Log.Information("Aroon Oscillator = " + aroon.Last().Oscillator + " " + aroon.Last().Date);
                Log.Information("Should be: " + aroon.Last().AroonUp + " > " + aroon.Last().AroonDown);
                Log.Information("Should be: " + aroon.Last().AroonUp + " = " + aroonUpValueLong);
                Log.Information("Should be: " + aroon.Last().AroonDown + " < " + aroonDownValue);
                Log.Information("Aroon = Long - false");
                return false;
            }
        }

        internal bool FromLongSignal(CandleList candleList, decimal deltaPrice)
        {
            List<AroonResult> aroon = Serialization.AroonData(candleList, deltaPrice, lookbackPeriod);

            if (
                aroon.Last().AroonUp < aroon.Last().AroonDown
                ||
                aroon.Last().AroonUp < aroonUpValueFromLong
                ||
                aroon.Last().AroonDown > aroonUpValueFromLong

                )
            {
                Log.Information("Aroon Up = " + aroon.Last().AroonUp + " " + aroon.Last().Date);
                Log.Information("Aroon Down = " + aroon.Last().AroonDown + " " + aroon.Last().Date);
                Log.Information("Aroon Oscillator = " + aroon.Last().Oscillator + " " + aroon.Last().Date);
                Log.Information("Should be: " + aroon.Last().AroonUp + " < " + aroon.Last().AroonDown);
                Log.Information("Should be: " + aroon.Last().AroonUp + " < " + aroonUpValueFromLong);
                Log.Information("Should be: " + aroon.Last().AroonDown + " > " + aroonUpValueFromLong);
                Log.Information("Aroon = FromLong - true");
                return true;
            }
            else
            {
                Log.Information("Aroon Up = " + aroon.Last().AroonUp + " " + aroon.Last().Date);
                Log.Information("Aroon Down = " + aroon.Last().AroonDown + " " + aroon.Last().Date);
                Log.Information("Aroon Oscillator = " + aroon.Last().Oscillator + " " + aroon.Last().Date);
                Log.Information("Should be: " + aroon.Last().AroonUp + " < " + aroon.Last().AroonDown);
                Log.Information("Should be: " + aroon.Last().AroonUp + " < " + aroonUpValueFromLong);
                Log.Information("Should be: " + aroon.Last().AroonDown + " > " + aroonUpValueFromLong);
                Log.Information("Aroon = FromLong - false");
                return false;
            }
        }

        //internal bool FromLongSignal(CandleList candleList, decimal deltaPrice)
        //{
        //    List<MacdResult> macd = Serialization.MacdData(candleList, deltaPrice);

        //    if (macd.Last().Macd < macd.Last().Signal
        //        || macd.Last().Histogram < 0
        //        || MacdDegreeAverageAngle(macd, 2) < 0
        //        || MacdHistogramDegreeAverageAngle(macd, 1) < MacdHistogramDegreeAverageAngle(macd, 2))
        //    {
        //        Log.Information("Macd = " + macd.Last().Macd);
        //        Log.Information("Macd Histogram = " + macd.Last().Histogram);
        //        Log.Information("Macd Average Angle is not degree:  " + MacdDegreeAverageAngle(macd, 1) + " < 0");
        //        Log.Information("Macd Histogram Average Angle is not degree:  " + MacdHistogramDegreeAverageAngle(macd, 1) + " < " + MacdHistogramDegreeAverageAngle(macd, 2));
        //        Log.Information("Macd = FromLong - true");
        //        return true;
        //    }
        //    else
        //    {
        //        Log.Information("Macd = " + macd.Last().Macd);
        //        Log.Information("Macd Histogram = " + macd.Last().Histogram);
        //        Log.Information("Macd Average Angle is degree:  " + MacdDegreeAverageAngle(macd, 1) + " >= 0");
        //        Log.Information("Macd Histogram Average Angle is degree:  " + MacdHistogramDegreeAverageAngle(macd, 1) + " < " + MacdHistogramDegreeAverageAngle(macd, 2));
        //        Log.Information("Macd = FromLong - false");
        //        return false;
        //    }
        //}


    }
}