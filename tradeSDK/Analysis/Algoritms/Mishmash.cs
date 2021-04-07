﻿using MarketDataModules;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Tinkoff.Trading.OpenApi.Models;
using TradingAlgorithms.IndicatorSignals;

namespace TradingAlgorithms.Algoritms
{
    public class Mishmash
    {
        Signal Signal = new Signal();


        //Передаваемые при создании объекта параметры
        public CandlesList candleList { get; set; }
        public decimal deltaPrice { get; set; }

        //Тюнинг индикаторов


        public bool Long()
        {
            if (

                Signal.DpoLongSignal(candleList, deltaPrice)
                &&
                Signal.MacdLongSignal(candleList, deltaPrice)
                &&
                Signal.AroonLongSignal(candleList, deltaPrice)
                &&
                Signal.AdxLongSignal(candleList, deltaPrice)               
                &&
                Signal.ObvLongSignal(candleList, deltaPrice)
                //Проверка на отсутствие боковика
                &&
                Signal.BollingerBandsLongSignal(candleList, deltaPrice)
                
                &&
                Signal.CandleLongSignal(candleList, deltaPrice)

                //Проверка на отсутсвие гэпа
                &&
                Signal.SmaLongSignal(candleList, deltaPrice)

                )
            {
                Log.Information("Mishmash Algoritms: Long - true " + candleList.Figi);
                return true; 
            }
            else 
            {
                Log.Information("Mishmash Algoritms: Long - false " + candleList.Figi);
                return false;
            }
        }
        public bool FromLong()
        {
            if (
                Signal.MacdFromLongSignal(candleList, deltaPrice)
                ||
                Signal.AdxFromLongSignal(candleList, deltaPrice)
                ||
                Signal.AroonFromLongSignal(candleList, deltaPrice)
                )
            {
                Log.Information("Mishmash Algoritms: FromLong - true " + candleList.Figi);
                return true; 
            }
            else
            {
                Log.Information("Mishmash Algoritms: FromLong - false " + candleList.Figi);
                return false; 
            }
        }

        public bool Short()
        {
            throw new NotImplementedException();
        }

        public bool FromShort()
        {
            throw new NotImplementedException();
        }
    }
}
