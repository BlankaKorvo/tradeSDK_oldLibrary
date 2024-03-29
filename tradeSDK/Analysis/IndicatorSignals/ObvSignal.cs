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
        int obvSmaLookbackPeriodFirst = 10;
        int obvSmaLookbackPeriodSecond = 25;
        int obvAnglesCount = 3;

        internal bool ObvLongSignal(CandlesList candleList, decimal deltaPrice)
        {
            Log.Information("Start OBV LongSignal. Figi: " + candleList.Figi);
            List<ObvResult> obvFirst = Mapper.ObvData(candleList, deltaPrice, obvSmaLookbackPeriodFirst);
            List<ObvResult> obvSecond = Mapper.ObvData(candleList, deltaPrice, obvSmaLookbackPeriodSecond);

            double obvDegreeAverageAngleFirst = ObvDegreeAverageAngle(obvFirst, obvAnglesCount, obv.obv);
            double smaDegreeAverageAngleFirst = ObvDegreeAverageAngle(obvFirst, obvAnglesCount, obv.ObvSma);
            double denominatorDegreeAverageAngleFirst = ObvDegreeAverageAngle(obvFirst, obvAnglesCount, obv.ObvSmaDenominator);
            double obvDegreeAverageAngleSecond = ObvDegreeAverageAngle(obvSecond, obvAnglesCount, obv.obv);
            double smaDegreeAverageAngleSecond = ObvDegreeAverageAngle(obvSecond, obvAnglesCount, obv.ObvSma);
            double denominatorDegreeAverageAngleSecond = ObvDegreeAverageAngle(obvSecond, obvAnglesCount, obv.ObvSmaDenominator);
            if (
                obvFirst.Last().ObvSma > obvSecond.Last().ObvSma
                &&
                obvFirst.Last().Obv > obvFirst.Last().ObvSma
                &&
                //углы lookbackPeriodFirst
                obvDegreeAverageAngleFirst > 0
                &&
                smaDegreeAverageAngleFirst > 0
                &&
                //denominatorDegreeAverageAngleFirst > 0
                //&&
                //углы lookbackPeriodSecond
                obvDegreeAverageAngleSecond > 0
                &&
                smaDegreeAverageAngleSecond > 0
                //&&
                //denominatorDegreeAverageAngleSecond > 0
               )

            {
                Log.Information("Start ObvSignal");
                Log.Information("Obv = " + obvFirst.Last().Obv);
                Log.Information("ObvSmaDenominator = " + obvFirst.Last().ObvSmaDenominator);
                Log.Information("Sma(Obv, " + obvSmaLookbackPeriodFirst + " ) = " + obvFirst.Last().ObvSma);
                Log.Information("Sma(Obv, " + obvSmaLookbackPeriodSecond + " ) = " + obvSecond.Last().ObvSma);
                Log.Information("Sma(Obv, " + obvSmaLookbackPeriodFirst + " ) must be more then Sma(Obv, " + obvSmaLookbackPeriodSecond + " )");
                Log.Information("and");
                Log.Information("Obv must be more then Sma(Obv, " + obvSmaLookbackPeriodFirst + " )");
                Log.Information("Obv degree average angle obv( lookback period = " + obvSmaLookbackPeriodFirst + "; anglesCount = " + obvAnglesCount + " ) = " + obvDegreeAverageAngleFirst);
                Log.Information("Obv degree average angle obv sma( lookback period = " + obvSmaLookbackPeriodFirst + "; anglesCount = " + obvAnglesCount + " ) = " + smaDegreeAverageAngleFirst);
                Log.Information("Obv degree average angle Obv Sma denominator( lookback period = " + obvSmaLookbackPeriodFirst + "; anglesCount = " + obvAnglesCount + " ) = " + denominatorDegreeAverageAngleFirst);
                Log.Information("Obv degree average angle obv( lookback period = " + obvSmaLookbackPeriodSecond + "; anglesCount = " + obvAnglesCount + " ) = " + obvDegreeAverageAngleSecond);
                Log.Information("Obv degree average angle obv sma( lookback period = " + obvSmaLookbackPeriodSecond + "; anglesCount = " + obvAnglesCount + " ) = " + smaDegreeAverageAngleSecond);
                Log.Information("Obv degree average angle Obv Sma denominator( lookback period = " + obvSmaLookbackPeriodSecond + "; anglesCount = " + obvAnglesCount + " ) = " + denominatorDegreeAverageAngleSecond);
                Log.Information("ObvSignal = Long - true for: " + candleList.Figi);
                Log.Information("Stop ObvSignal");
                return true;
            }
            else
            {
                Log.Information("Start ObvSignal");
                Log.Information("Obv = " + obvFirst.Last().Obv);
                Log.Information("ObvSmaDenominator = " + obvFirst.Last().ObvSmaDenominator);
                Log.Information("Sma(Obv, " + obvSmaLookbackPeriodFirst + " ) = " + obvFirst.Last().ObvSma);
                Log.Information("Sma(Obv, " + obvSmaLookbackPeriodSecond + " ) = " + obvSecond.Last().ObvSma);
                Log.Information("Sma(Obv, " + obvSmaLookbackPeriodFirst + " ) must be more then Sma(Obv, " + obvSmaLookbackPeriodSecond + " )");
                Log.Information("and");
                Log.Information("Obv must be more then Sma(Obv, " + obvSmaLookbackPeriodFirst + " )");
                Log.Information("Obv degree average angle obv( lookback period = " + obvSmaLookbackPeriodFirst + "; anglesCount = " + obvAnglesCount + " ) = " + obvDegreeAverageAngleFirst);
                Log.Information("Obv degree average angle obv sma( lookback period = " + obvSmaLookbackPeriodFirst + "; anglesCount = " + obvAnglesCount + " ) = " + smaDegreeAverageAngleFirst);
                Log.Information("Obv degree average angle Obv Sma denominator( lookback period = " + obvSmaLookbackPeriodFirst + "; anglesCount = " + obvAnglesCount + " ) = " + denominatorDegreeAverageAngleFirst);
                Log.Information("Obv degree average angle obv( lookback period = " + obvSmaLookbackPeriodSecond + "; anglesCount = " + obvAnglesCount + " ) = " + obvDegreeAverageAngleSecond);
                Log.Information("Obv degree average angle obv sma( lookback period = " + obvSmaLookbackPeriodSecond + "; anglesCount = " + obvAnglesCount + " ) = " + smaDegreeAverageAngleSecond);
                Log.Information("Obv degree average angle Obv Sma denominator( lookback period = " + obvSmaLookbackPeriodSecond + "; anglesCount = " + obvAnglesCount + " ) = " + denominatorDegreeAverageAngleSecond);
                Log.Information("ObvSignal = Long - false for: " + candleList.Figi);
                Log.Information("Stop ObvSignal");
                return false;
            }
            double ObvDegreeAverageAngle(List<ObvResult> ObvValue, int anglesCount, obv obvLine)
            {
                Log.Information("Start OBV LongSignal. Figi: " + candleList.Figi);
                List<ObvResult> skipObv = ObvValue.Skip(ObvValue.Count - (anglesCount + 1)).ToList();
                List<decimal?> values = new List<decimal?>();
                foreach (var item in skipObv)
                {
                    switch (obvLine)
                    {
                        case obv.obv:
                            values.Add(item.Obv);
                            Log.Information("Obv degree average of " + anglesCount + " angles: " + item.Date + " " + item.Obv);
                            break;
                        case obv.ObvSma:
                            values.Add(item.ObvSma);
                            Log.Information("ObvSma degree average of " + anglesCount + " angles: " + item.Date + " " + item.ObvSma);
                            break;
                        case obv.ObvSmaDenominator:
                            values.Add(item.ObvSmaDenominator);
                            Log.Information("ObvSmaDenominator degree average of " + anglesCount + " angles: " + item.Date + " " + item.ObvSmaDenominator);
                            break;
                    }
                }
                return DeltaDegreeAngle(values);
            }

        }
        enum obv
        {
            obv,
            ObvSma,
            ObvSmaDenominator
        }
    }
}
