﻿using Serilog;
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tinkoff.Trading.OpenApi.Models;

namespace TinkoffData
{
    public static class Serialization
    {
        public static List<Quote> ConvertTinkoffCandlesToQuote(List<CandlePayload> candles)
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

        public static List<Quote> ConvertTinkoffCandlesToQuote(List<CandlePayload> candles, decimal realClose)
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

        public static List<MacdResult> MacdData(CandleList candleList)
        {
            List<Quote> candles = ConvertTinkoffCandlesToQuote(candleList.Candles);
            return Indicator.GetMacd(candles).ToList();
        }

        public static List<MacdResult> MacdData(CandleList candleList, decimal realPrise)
        {
            List<Quote> candles = ConvertTinkoffCandlesToQuote(candleList.Candles, realPrise);
            return Indicator.GetMacd(candles).ToList();
        }

        public static List<EmaResult> EmaData(CandleList candleList, int lookbackPeriod)
        {
            List<Quote> candles = ConvertTinkoffCandlesToQuote(candleList.Candles);
            return Indicator.GetEma(candles, lookbackPeriod).ToList();
        }

        public static List<EmaResult> EmaData(CandleList candleList, int lookbackPeriod, decimal realPrise)
        {
            List<Quote> candles = ConvertTinkoffCandlesToQuote(candleList.Candles, realPrise);
            return Indicator.GetEma(candles, lookbackPeriod).ToList();
        }

        public static List<ObvResult> ObvData(CandleList candleList, int lookbackPeriod)
        {
            List<Quote> candles = ConvertTinkoffCandlesToQuote(candleList.Candles);
            return Indicator.GetObv(candles, lookbackPeriod).ToList();
        }
        
        public static List<ObvResult> ObvData(CandleList candleList, int lookbackPeriod, decimal realPrise)
        {
            List<Quote> candles = ConvertTinkoffCandlesToQuote(candleList.Candles, realPrise);
            return Indicator.GetObv(candles, lookbackPeriod).ToList();
        }

        public static List<SmaResult> SmaData(CandleList candleList, int lookbackPeriod)
        {
            List<Quote> candles = ConvertTinkoffCandlesToQuote(candleList.Candles);
            return Indicator.GetSma(candles, lookbackPeriod).ToList();
        }
        public static List<SmaResult> SmaData(CandleList candleList, int lookbackPeriod, decimal realPrise)
        {
            List<Quote> candles = ConvertTinkoffCandlesToQuote(candleList.Candles, realPrise);
            return Indicator.GetSma(candles, lookbackPeriod).ToList();
        }

        public static List<DpoResult> DpoData(CandleList candleList, int lookbackPeriod)
        {
            List<Quote> candles = ConvertTinkoffCandlesToQuote(candleList.Candles);
            return Indicator.GetDpo(candles, lookbackPeriod).ToList();
        }

        public static List<DpoResult> DpoData(CandleList candleList, decimal realPrise, int lookbackPeriod)
        {
            Log.Information("DPO set price = " + realPrise);
            Log.Information("DPO set history = " + lookbackPeriod);
            List<Quote> candles = ConvertTinkoffCandlesToQuote(candleList.Candles, realPrise);
            List <DpoResult> dpoData = Indicator.GetDpo(candles, lookbackPeriod).ToList();

            Log.Information("DPO history = " + dpoData.Last().Dpo + " " + dpoData.Last().Date);

            return dpoData;


        }

        public static List<SuperTrendResult> SuperTrendData(CandleList candleList, int lookbackPeriod = 20, decimal multiplier = 2)
        {
            Log.Information("Super Trend History = " + lookbackPeriod);
            Log.Information("Super Trend Multiplier = " + multiplier);

            List<Quote> candles = ConvertTinkoffCandlesToQuote(candleList.Candles);
            List<SuperTrendResult> superTrandData = Indicator.GetSuperTrend(candles, lookbackPeriod, multiplier).ToList();

            Log.Information("Super Trend Value = " + superTrandData.Last().SuperTrend + " " + superTrandData.Last().Date);

            return superTrandData;
        }

        public static List<SuperTrendResult> SuperTrendData(CandleList candleList, decimal realPrise, int lookbackPeriod = 20, decimal multiplier = 2)
        {

            Log.Information("Average (Bid, Ask) Prise = " + realPrise);
            Log.Information("Super Trend History = " + lookbackPeriod);
            Log.Information("Super Trend Multiplier = " + multiplier);

            List<Quote> candles = ConvertTinkoffCandlesToQuote(candleList.Candles, realPrise);
            List<SuperTrendResult> superTrandData = Indicator.GetSuperTrend(candles, lookbackPeriod, multiplier).ToList();

            Log.Information("Super Trend Value = " + superTrandData.Last().SuperTrend + " " + superTrandData.Last().Date);

            return superTrandData;
        }

        public static List<IchimokuResult> IchimokudData(CandleList candleList, int signalPeriod = 9, int shortSpanPeriod = 26, int longSpanPeriod = 52)
        {

            Log.Information("signalPeriod = " + signalPeriod);
            Log.Information("shortSpanPeriod = " + shortSpanPeriod);
            Log.Information("longSpanPeriod = " + longSpanPeriod);

            List<Quote> candles = ConvertTinkoffCandlesToQuote(candleList.Candles);
            List<IchimokuResult> ichimokuData = Indicator.GetIchimoku(candles, signalPeriod, shortSpanPeriod, longSpanPeriod).ToList();

            Log.Information("TenkanSen = " + ichimokuData.Last().TenkanSen + " " + ichimokuData.Last().Date);
            Log.Information("KijunSen = " + ichimokuData.Last().KijunSen + " " + ichimokuData.Last().Date);
            Log.Information("SenkouSpanA = " + ichimokuData.Last().SenkouSpanA + " " + ichimokuData.Last().Date);
            Log.Information("SenkouSpanB = " + ichimokuData.Last().SenkouSpanB + " " + ichimokuData.Last().Date);
            Log.Information("ChikouSpan = " + ichimokuData.Last().ChikouSpan + " " + ichimokuData.Last().Date);

            return ichimokuData;
        }

        public static List<IchimokuResult> IchimokuData(CandleList candleList, decimal realPrise, int signalPeriod = 9, int shortSpanPeriod = 26, int longSpanPeriod = 52)
        {

            Log.Information("realPrise = " + realPrise);
            Log.Information("signalPeriod = " + signalPeriod);
            Log.Information("shortSpanPeriod = " + shortSpanPeriod);
            Log.Information("longSpanPeriod = " + longSpanPeriod);

            List<Quote> candles = ConvertTinkoffCandlesToQuote(candleList.Candles, realPrise);
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