﻿using Serilog;
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tinkoff.Trading.OpenApi.Models;
using MarketDataModules;

namespace TinkoffData
{
    public static class Mapper 
    {
        public static List<Quote> ConvertThisCandlesToQuote(List<CandleStructure> candles)
        {
            List<Quote> quotes = new List<Quote>();

            foreach (var candle in candles)
            {
                Quote quote = new Quote();
                quote.Close = candle.Close;
                quote.Date = candle.Time;
                quote.Open = candle.Open;
                quote.High = candle.High;
                quote.Low = candle.Low;
                quote.Volume = candle.Volume;
                quotes.Add(quote);
            }
            return quotes;
        }

        public static List<Quote> ConvertThisCandlesToQuote(List<CandleStructure> candles, decimal realClose)
        {
            List<Quote> quotes = new List<Quote>();

            foreach (var candle in candles)
            {
                Quote quote = new Quote();
                quote.Close = candle.Close;
                quote.Date = candle.Time;
                quote.Open = candle.Open;
                quote.High = candle.High;
                quote.Low = candle.Low;
                quote.Volume = candle.Volume;
                quotes.Add(quote);
            }
            quotes.Last().Close = realClose;
            return quotes;
        }

        internal static List<StochResult> StochData(CandlesList candleList, decimal deltaPrice, int stochLookbackPeriod, int stochSignalPeriod, int stochSmoothPeriod)
        {
            Log.Information("Start mapping Stoch");
            List<Quote> candles = ConvertThisCandlesToQuote(candleList.Candles, deltaPrice);
            List<StochResult> stoch = Indicator.GetStoch(candles, stochLookbackPeriod, stochSignalPeriod, stochSmoothPeriod).ToList();
            Log.Information("Stop mapping Stoch");
            return stoch;
        }


        public static List<AdxResult> TsiData(CandlesList candleList, int lookbackPeriod)
        {
            throw new NotImplementedException();
        }
        public static List<AdxResult> TsiData(CandlesList candleList, decimal deltaPrice, int lookbackPeriod)
        {
            throw new NotImplementedException();
        }


        public static List<AdxResult> AdxData(CandlesList candleList, int lookbackPeriod)
        {
            List<Quote> candles = ConvertThisCandlesToQuote(candleList.Candles);
            List<AdxResult> adx = Indicator.GetAdx(candles, lookbackPeriod).ToList();
            return adx;
        }

        public static List<AdxResult> AdxData(CandlesList candleList, decimal realPrise, int lookbackPeriod)
        {
            try
            {
                Log.Information("Start AdxData method. Figi: " + candleList.Figi);
                Log.Information("Candles count: " + candleList.Candles.Count);
                Log.Information("realPrise: " + realPrise);
                Log.Information("lookbackPeriod: " + lookbackPeriod);
                List<Quote> candles = ConvertThisCandlesToQuote(candleList.Candles, realPrise);
                List<AdxResult> adx = Indicator.GetAdx(candles, lookbackPeriod).ToList();
                Log.Information("Stop AdxData method. Figi: " + candleList.Figi);
                return adx;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                Log.Information("Stop AdxData method. Return: null");
                return null;
            }
        }

        public static List<AroonResult> AroonData(CandlesList candleList, int lookbackPeriod = 7)
        {
            List<Quote> candles = ConvertThisCandlesToQuote(candleList.Candles);
            List<AroonResult> aroon = Indicator.GetAroon(candles, lookbackPeriod).ToList();
            return aroon;
        }

        public static List<AroonResult> AroonData(CandlesList candleList, decimal realPrise, int lookbackPeriod = 7)
        {
            List<Quote> candles = ConvertThisCandlesToQuote(candleList.Candles, realPrise);
            List<AroonResult> aroon = Indicator.GetAroon(candles, lookbackPeriod).ToList();
            return aroon;
        }

        public static List<BollingerBandsResult> BollingerBandsData(CandlesList candleList, decimal realPrise, int lookbackPeriod = 20, decimal standardDeviations = 2m)
        {
            List<Quote> candles = ConvertThisCandlesToQuote(candleList.Candles, realPrise);
            List<BollingerBandsResult> bollingerBands = Indicator.GetBollingerBands(candles, lookbackPeriod, standardDeviations).ToList();
            return bollingerBands;
        }
        public static List<BollingerBandsResult> BollingerBandsData(CandlesList candleList, int lookbackPeriod = 20, decimal standardDeviations = 2m)
        {
            List<Quote> candles = ConvertThisCandlesToQuote(candleList.Candles);
            List<BollingerBandsResult> bollingerBands = Indicator.GetBollingerBands(candles, lookbackPeriod, standardDeviations).ToList();
            return bollingerBands;
        }

        public static List<MacdResult> MacdData(CandlesList candleList)
        {
            List<Quote> candles = ConvertThisCandlesToQuote(candleList.Candles);
            List<MacdResult> macdResult = Indicator.GetMacd(candles).ToList();
            return macdResult;
        }

        //public static List<MacdResult> MacdData(CandleList candleList, decimal realPrise, int fastPeriod = 12, int slowPeriod = 26, int signalPeriod = 9)
        public static List<MacdResult> MacdData(CandlesList candleList, decimal realPrise, int fastPeriod, int slowPeriod, int signalPeriod)
        {
            Log.Information("Start MacdData method:");
            Log.Information("Figi: " + candleList.Figi);
            Log.Information("Candles count: " + candleList.Candles.Count);
            Log.Information("fastPeriod: " + fastPeriod);
            Log.Information("slowPeriod: " + slowPeriod);
            Log.Information("signalPeriod: " + signalPeriod);
            List <Quote> candles = ConvertThisCandlesToQuote(candleList.Candles, realPrise);
            List <MacdResult> macdResult = Indicator.GetMacd(candles, fastPeriod, slowPeriod, signalPeriod).ToList();
            Log.Information("Stop MacdData method:");
            return macdResult;
        }

        public static List<EmaResult> EmaData(CandlesList candleList, int lookbackPeriod)
        {
            List<Quote> candles = ConvertThisCandlesToQuote(candleList.Candles);
            return Indicator.GetEma(candles, lookbackPeriod).ToList();
        }

        public static List<EmaResult> EmaData(CandlesList candleList, decimal realPrise, int lookbackPeriod)
        {
            List<Quote> candles = ConvertThisCandlesToQuote(candleList.Candles, realPrise);
            return Indicator.GetEma(candles, lookbackPeriod).ToList();
        }

        public static List<ObvResult> ObvData(CandlesList candleList, int lookbackPeriod)
        {
            List<Quote> candles = ConvertThisCandlesToQuote(candleList.Candles);
            return Indicator.GetObv(candles, lookbackPeriod).ToList();
        }
        
        public static List<ObvResult> ObvData(CandlesList candleList, decimal realPrise, int lookbackPeriod)
        {
            List<Quote> candles = ConvertThisCandlesToQuote(candleList.Candles, realPrise);
            return Indicator.GetObv(candles, lookbackPeriod).ToList();
        }

        public static List<AdlResult> AdlData(CandlesList candleList, int lookbackPeriod)
        {
            List<Quote> candles = ConvertThisCandlesToQuote(candleList.Candles);
            return Indicator.GetAdl(candles, lookbackPeriod).ToList();
        }

        public static List<AdlResult> AdlData(CandlesList candleList, decimal realPrise, int lookbackPeriod)
        {
            List<Quote> candles = ConvertThisCandlesToQuote(candleList.Candles, realPrise);
            return Indicator.GetAdl(candles, lookbackPeriod).ToList();
        }

        public static List<SmaResult> SmaData(CandlesList candleList, int lookbackPeriod)
        {
            List<Quote> candles = ConvertThisCandlesToQuote(candleList.Candles);
            return Indicator.GetSma(candles, lookbackPeriod).ToList();
        }
        public static List<SmaResult> SmaData(CandlesList candleList, decimal realPrise, int lookbackPeriod)
        {
            List<Quote> candles = ConvertThisCandlesToQuote(candleList.Candles, realPrise);
            return Indicator.GetSma(candles, lookbackPeriod).ToList();
        }

        public static List<DpoResult> DpoData(CandlesList candleList, int lookbackPeriod)
        {
            List<Quote> candles = ConvertThisCandlesToQuote(candleList.Candles);
            return Indicator.GetDpo(candles, lookbackPeriod).ToList();
        }

        public static List<DpoResult> DpoData(CandlesList candleList, decimal realPrise, int lookbackPeriod)
        {
            Log.Information("DPO set price = " + realPrise);
            Log.Information("DPO set lookbackPeriod = " + lookbackPeriod);
            List<Quote> candles = ConvertThisCandlesToQuote(candleList.Candles, realPrise);
            List <DpoResult> dpoData = Indicator.GetDpo(candles, lookbackPeriod).ToList();
            Log.Information("Last Dpo = " + dpoData.Last().Dpo + " " + dpoData.Last().Date);
            return dpoData;
        }

        public static List<SuperTrendResult> SuperTrendData(CandlesList candleList, int lookbackPeriod = 20, decimal multiplier = 2)
        {
            Log.Information("Super Trend History = " + lookbackPeriod);
            Log.Information("Super Trend Multiplier = " + multiplier);

            List<Quote> candles = ConvertThisCandlesToQuote(candleList.Candles);
            List<SuperTrendResult> superTrandData = Indicator.GetSuperTrend(candles, lookbackPeriod, multiplier).ToList();

            Log.Information("Super Trend Value = " + superTrandData.Last().SuperTrend + " " + superTrandData.Last().Date);

            return superTrandData;
        }

        public static List<SuperTrendResult> SuperTrendData(CandlesList candleList, decimal realPrise, int lookbackPeriod = 20, decimal multiplier = 2)
        {

            Log.Information("Average (Bid, Ask) Prise = " + realPrise);
            Log.Information("Super Trend History = " + lookbackPeriod);
            Log.Information("Super Trend Multiplier = " + multiplier);

            List<Quote> candles = ConvertThisCandlesToQuote(candleList.Candles, realPrise);
            List<SuperTrendResult> superTrandData = Indicator.GetSuperTrend(candles, lookbackPeriod, multiplier).ToList();

            Log.Information("Super Trend Value = " + superTrandData.Last().SuperTrend + " " + superTrandData.Last().Date);

            return superTrandData;
        }

        public static List<IchimokuResult> IchimokudData(CandlesList candleList, int signalPeriod = 9, int shortSpanPeriod = 26, int longSpanPeriod = 52)
        {

            Log.Information("signalPeriod = " + signalPeriod);
            Log.Information("shortSpanPeriod = " + shortSpanPeriod);
            Log.Information("longSpanPeriod = " + longSpanPeriod);

            List<Quote> candles = ConvertThisCandlesToQuote(candleList.Candles);
            List<IchimokuResult> ichimokuData = Indicator.GetIchimoku(candles, signalPeriod, shortSpanPeriod, longSpanPeriod).ToList();

            Log.Information("TenkanSen = " + ichimokuData.Last().TenkanSen + " " + ichimokuData.Last().Date);
            Log.Information("KijunSen = " + ichimokuData.Last().KijunSen + " " + ichimokuData.Last().Date);
            Log.Information("SenkouSpanA = " + ichimokuData.Last().SenkouSpanA + " " + ichimokuData.Last().Date);
            Log.Information("SenkouSpanB = " + ichimokuData.Last().SenkouSpanB + " " + ichimokuData.Last().Date);
            Log.Information("ChikouSpan = " + ichimokuData.Last().ChikouSpan + " " + ichimokuData.Last().Date);

            return ichimokuData;
        }

        public static List<IchimokuResult> IchimokuData(CandlesList candleList, decimal realPrise, int signalPeriod = 9, int shortSpanPeriod = 26, int longSpanPeriod = 52)
        {

            Log.Information("realPrise = " + realPrise);
            Log.Information("signalPeriod = " + signalPeriod);
            Log.Information("shortSpanPeriod = " + shortSpanPeriod);
            Log.Information("longSpanPeriod = " + longSpanPeriod);

            List<Quote> candles = ConvertThisCandlesToQuote(candleList.Candles, realPrise);
            List<IchimokuResult> ichimokuData = Indicator.GetIchimoku(candles, signalPeriod, shortSpanPeriod, longSpanPeriod).ToList();

            Log.Information("TenkanSen = " + ichimokuData.Last().TenkanSen + " " + ichimokuData.Last().Date);
            Log.Information("KijunSen = " + ichimokuData.Last().KijunSen + " " + ichimokuData.Last().Date);
            Log.Information("SenkouSpanA = " + ichimokuData.Last().SenkouSpanA + " " + ichimokuData.Last().Date);
            Log.Information("SenkouSpanB = " + ichimokuData.Last().SenkouSpanB + " " + ichimokuData.Last().Date);
            Log.Information("ChikouSpan = " + ichimokuData.Last().ChikouSpan + " " + ichimokuData.Last().Date);

            return ichimokuData;
        }
    }
}
