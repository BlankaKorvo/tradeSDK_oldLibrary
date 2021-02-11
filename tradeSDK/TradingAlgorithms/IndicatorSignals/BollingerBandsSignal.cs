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
    internal class BollingerBandsSignal : IndicatorSignalsHelper
    {
        int anglesCount = 3;
        internal bool LongSignal(CandleList candleList, decimal deltaPrice)
        {
            List<BollingerBandsResult> bollingerBands = Serialization.BollingerBandsData(candleList, deltaPrice);

            if (
                BollingerBandsWidthDegreeAverageAngle(bollingerBands, anglesCount) > 0
                &&
                BollingerBandsWidthDegreeAverageAngle(bollingerBands, 1) > 0
                )
            {
                Log.Information("BollingerBands UpperBand = " + bollingerBands.Last().UpperBand + " " + bollingerBands.Last().Date);
                Log.Information("BollingerBands LowerBand = " + bollingerBands.Last().LowerBand + " " + bollingerBands.Last().Date);
                Log.Information("BollingerBands PercentB = " + bollingerBands.Last().PercentB + " " + bollingerBands.Last().Date);
                Log.Information("BollingerBands Sma = " + bollingerBands.Last().Sma + " " + bollingerBands.Last().Date);
                Log.Information("BollingerBands Width = " + bollingerBands.Last().Width + " " + bollingerBands.Last().Date);
                Log.Information("BollingerBands ZScore = " + bollingerBands.Last().ZScore + " " + bollingerBands.Last().Date);
                Log.Information("BollingerBands BollingerBands Width Degree Average Angle Count " + anglesCount + " = " + BollingerBandsWidthDegreeAverageAngle(bollingerBands, anglesCount));
                Log.Information("BollingerBands BollingerBands Width Degree Average Angle Count 1 = " + BollingerBandsWidthDegreeAverageAngle(bollingerBands, 1));
                Log.Information("BollingerBands = Long - true");
                return true;
            }
            else
            {
                Log.Information("BollingerBands UpperBand = " + bollingerBands.Last().UpperBand + " " + bollingerBands.Last().Date);
                Log.Information("BollingerBands LowerBand = " + bollingerBands.Last().LowerBand + " " + bollingerBands.Last().Date);
                Log.Information("BollingerBands PercentB = " + bollingerBands.Last().PercentB + " " + bollingerBands.Last().Date);
                Log.Information("BollingerBands Sma = " + bollingerBands.Last().Sma + " " + bollingerBands.Last().Date);
                Log.Information("BollingerBands Width = " + bollingerBands.Last().Width + " " + bollingerBands.Last().Date);
                Log.Information("BollingerBands ZScore = " + bollingerBands.Last().ZScore + " " + bollingerBands.Last().Date);
                Log.Information("BollingerBands BollingerBands Width Degree Average Angle Count " + anglesCount + " = " + BollingerBandsWidthDegreeAverageAngle(bollingerBands, anglesCount));
                Log.Information("BollingerBands BollingerBands Width Degree Average Angle Count 1 = " + BollingerBandsWidthDegreeAverageAngle(bollingerBands, 1));
                Log.Information("BollingerBands = Long - false");
                return false;
            }

            double BollingerBandsWidthDegreeAverageAngle(List<BollingerBandsResult> bollingerBands, int anglesCount)
            {
                List<BollingerBandsResult> skipbollingerBands = bollingerBands.Skip(bollingerBands.Count - (anglesCount + 1)).ToList();
                List<decimal?> values = new List<decimal?>();
                foreach (var item in skipbollingerBands)
                {
                    values.Add(item.Width);
                    Log.Information("DPO for Degree Average Angle: " + item.Date + " " + item.Width);
                }
                return DeltaDegreeAngle(values);
            }
        }
    }
}