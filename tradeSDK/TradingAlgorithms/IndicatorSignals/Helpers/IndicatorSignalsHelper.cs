﻿using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingAlgorithms.IndicatorSignals.Helpers
{
    internal class IndicatorSignalsHelper
    {
        internal double DeltaDegreeAngle(List<decimal?> values)
        {
            var countDelta = values.Count;
            double summ = 0;
            for (int i = 1; i < countDelta; i++)
            {
                double deltaLeg = Convert.ToDouble(values[i] - values[i - 1]);
                double legDifference = Math.Atan(deltaLeg);
                double angle = legDifference * (180 / Math.PI);
                Log.Information("Angle: " + angle.ToString());
                summ += angle;
            }
            double averageAngles = summ / (countDelta - 1);
            Log.Information("Average Angles: " + averageAngles.ToString());
            return averageAngles;
        }
    }
}