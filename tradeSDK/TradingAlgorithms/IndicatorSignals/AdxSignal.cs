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
    class AdxSignal : IndicatorSignalsHelper
    {
        int lookbackPeriod = 14;
        int averageAngleCount = 2;
        internal bool LongSignal(CandleList candleList, decimal deltaPrice)
        {
            List<AdxResult> adx = Serialization.AdxData(candleList, deltaPrice, lookbackPeriod);
            if (
                            adx.Last().Pdi > adx.Last().Mdi
                            &&
                            adx.Last().Adx > adx.Last().Mdi
                            &&
                            AdxDegreeAverageAngle(adx, averageAngleCount, Adx.Adx) > 0
                            &&
                            AdxDegreeAverageAngle(adx, averageAngleCount, Adx.Pdi) > 0
                            &&
                            AdxDegreeAverageAngle(adx, averageAngleCount, Adx.Mdi) < 0
                            )
            {
                Log.Information("Adx Pdi = " + adx.Last().Pdi + " " + adx.Last().Date + " должен быть больше Adx Mdi");
                Log.Information("Adx Mdi = " + adx.Last().Mdi + " " + adx.Last().Date + " должен быть меньше Adx Pdi и Adx");
                Log.Information("Adx Adx = " + adx.Last().Adx + " " + adx.Last().Date + " должен быть больше Adx Mdi");
                Log.Information("Adx угол " + averageAngleCount + "прямых Adx = " + AdxDegreeAverageAngle(adx, averageAngleCount, Adx.Adx) + " Должна быть больше 0");
                Log.Information("Adx угол " + averageAngleCount + "прямых Pdi = " + AdxDegreeAverageAngle(adx, averageAngleCount, Adx.Pdi) + " Должна быть больше 0");
                Log.Information("Adx угол " + averageAngleCount + "прямых Mdi = " + AdxDegreeAverageAngle(adx, averageAngleCount, Adx.Mdi) + " Должна быть меньше 0");
                Log.Information("Adx = Long - true");
                return true;
            }
            else
            {
                Log.Information("Adx Pdi = " + adx.Last().Pdi + " " + adx.Last().Date + " должен быть больше Adx Mdi");
                Log.Information("Adx Mdi = " + adx.Last().Mdi + " " + adx.Last().Date + " должен быть меньше Adx Pdi и Adx");
                Log.Information("Adx Adx = " + adx.Last().Adx + " " + adx.Last().Date + " должен быть больше Adx Mdi");
                Log.Information("Adx угол " + averageAngleCount + "прямых Adx = " + AdxDegreeAverageAngle(adx, averageAngleCount, Adx.Adx) + " Должна быть больше 0");
                Log.Information("Adx угол " + averageAngleCount + "прямых Pdi = " + AdxDegreeAverageAngle(adx, averageAngleCount, Adx.Pdi) + " Должна быть больше 0");
                Log.Information("Adx угол " + averageAngleCount + "прямых Mdi = " + AdxDegreeAverageAngle(adx, averageAngleCount, Adx.Mdi) + " Должна быть меньше 0");
                Log.Information("Adx = Long - false");
                return false;
            }

        }
        private double AdxDegreeAverageAngle(List<AdxResult> AdxValue, int anglesCount, Adx adxLine)
        {
            List<AdxResult> skipAdx = AdxValue.Skip(AdxValue.Count - (anglesCount + 1)).ToList();
            List<decimal?> values = new List<decimal?>();
            foreach (var item in skipAdx)
            {
                switch (adxLine)
                { 
                    case Adx.Pdi:
                        values.Add(item.Pdi);
                        Log.Information("DPO for Degree Average Angle: " + item.Date + " " + item.Pdi);
                        break;
                    case Adx.Mdi:
                        values.Add(item.Mdi);
                        Log.Information("DPO for Degree Average Angle: " + item.Date + " " + item.Mdi);
                        break;
                    case Adx.Adx:
                        values.Add(item.Adx);
                        Log.Information("DPO for Degree Average Angle: " + item.Date + " " + item.Adx);
                        break;
                }
            }
            return DeltaDegreeAngle(values);
        }
        enum Adx
        {
            Pdi,
            Mdi,
            Adx
        }
    }
}
