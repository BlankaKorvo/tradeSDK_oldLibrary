﻿using DataCollector;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TinkoffAdapter.Authority;
using Tinkoff.Trading.OpenApi.Models;
using Tinkoff.Trading.OpenApi.Network;
using TinkoffAdapter.DataHelper;
using TradingAlgorithms.Algoritms;
using Operation = MarketDataModules.Operation;
using Orderbook = MarketDataModules.Orderbook;
using CandleInterval = MarketDataModules.CandleInterval;
using MarketDataModules;

namespace TinkoffAdapter.TinkoffTrade
{
    public class TinkoffTrading : TransactionModel
    {
        //public Context context { get; set; }
        //public string figi { get; set; }
        public CandleInterval candleInterval { get; set; }
        //public int countStoks { get; set; }
        public int CandlesCount { get; set; } = 120;
        //public decimal budget { get; set; }

        //время ожидания между следующим циклом
        int sleep { get; set; } = 0;

        //GetTinkoffData market = new GetTinkoffData();
        MarketDataCollector marketDataCollector = new MarketDataCollector();

        async public Task TransactionAsync(TransactionModel transactionModel)
        {
            Log.Information("Start Transaction method. Figi: " + transactionModel.Figi);
            if (
                transactionModel == null
                ||
                transactionModel.Figi == null
                ||
                transactionModel.Purchase == 0
                ||
                transactionModel.Price == 0
                ||
                transactionModel.Quantity == 0)
            {
                Log.Information("Figi: " + transactionModel.Figi);
                Log.Information("Margin: " + transactionModel.Purchase);
                Log.Information("Price: " + transactionModel.Price);
                Log.Information("Quantity: " + transactionModel.Quantity);
                Log.Information("Operation: " + transactionModel.Operation.ToString());
                Log.Warning("Transaction is not correct for implementation");
                Log.Information("Stop Transaction method. Figi: " + transactionModel.Figi);
                return;
            }

            switch (transactionModel.Operation)
            {
                case Operation.toLong:
                    Log.Information("Start Buy Stoks to Long. Figi: " + transactionModel.Figi);
                    await BuyStoksAsync(transactionModel);                    
                    Log.Information("Stop Transaction method. Figi: " + transactionModel.Figi);
                    return;

                case Operation.fromLong:
                    Log.Information("Start Sell Stoks from Long. Figi: " + transactionModel.Figi);
                    await SellStoksFromLongAsync(transactionModel);        
                    Log.Information("Stop Transaction method. Figi: " + transactionModel.Figi);
                    return;

                case Operation.toShort:
                    //not implemented
                    Log.Warning("Sell to short is not implemented");
                    return;

                case Operation.fromShort:
                    Log.Warning("By from short is not implemented");
                    return;
            }
        }

        public async Task<TransactionModel> PurchaseDecisionAsync()
        {
            Log.Information("Start PurchaseDecision method. Figi: " + this.Figi);
            TransactionModel transactionModel = new TransactionModel();
            transactionModel.Figi = this.Figi;
            transactionModel.Purchase = this.Purchase;
            //Получаем свечи
            CandlesList candleList = await marketDataCollector.GetCandlesAsync(transactionModel.Figi, candleInterval, CandlesCount);

            //Получаем стакан
            Orderbook orderbook = await marketDataCollector.GetOrderbookAsync(transactionModel.Figi);
            if (orderbook == null)
            {
                Log.Information("Orderbook " + transactionModel.Figi + " is null");
                transactionModel.Operation = Operation.notTrading;
                return transactionModel; 
            }
            decimal ask = orderbook.Asks.FirstOrDefault().Price;
            decimal bid = orderbook.Bids.FirstOrDefault().Price;
            int quantityAsksFirst = orderbook.Asks.FirstOrDefault().Quantity;
            int quantityBidsFirst = orderbook.Bids.FirstOrDefault().Quantity;
            decimal deltaPrice = (ask + bid) / 2;

            Mishmash mishmash = new Mishmash() { candleList = candleList, deltaPrice = deltaPrice, orderbook = orderbook };

            if (mishmash.Long()==true)
            {
                Log.Information("Go to Long: " + transactionModel.Figi);
                transactionModel.Quantity = quantityAsksFirst;
                transactionModel.Operation = Operation.toLong;
                transactionModel.Price = ask;
            }
            else if (mishmash.FromLong()==true)
            {                
                Log.Information("Go from Long: " + transactionModel.Figi);
                transactionModel.Quantity = quantityBidsFirst;
                transactionModel.Operation = Operation.fromLong;
                transactionModel.Price = bid;
            }
            Log.Information("Stop PurchaseDecision for: " + transactionModel.Figi);
            return transactionModel;
        }

        public async Task<TransactionModelBase> PurchaseDecisionAsync(string figi)
        {
            Log.Information("Start PurchaseDecision method. Figi: " + this.Figi);
            TransactionModelBase transactionModelBase = new TransactionModelBase();
            transactionModelBase.Figi = figi;
            //Получаем свечи
            CandlesList candleList = await marketDataCollector.GetCandlesAsync(figi, candleInterval, CandlesCount);

            //Получаем стакан
            Orderbook orderbook = await marketDataCollector.GetOrderbookAsync(figi);
            if (orderbook == null)
            {
                Log.Information("Orderbook " + figi + " is null");
                transactionModelBase.Operation = Operation.notTrading;
                return transactionModelBase;
            }
            decimal ask = orderbook.Asks.FirstOrDefault().Price;
            decimal bid = orderbook.Bids.FirstOrDefault().Price;
            int quantityAsksFirst = orderbook.Asks.FirstOrDefault().Quantity;
            int quantityBidsFirst = orderbook.Bids.FirstOrDefault().Quantity;
            decimal deltaPrice = (ask + bid) / 2;

            Mishmash mishmash = new Mishmash() { candleList = candleList, deltaPrice = deltaPrice, orderbook = orderbook };

            if (mishmash.Long() == true)
            {
                Log.Information("Go to Long: " + transactionModelBase.Figi);
                transactionModelBase.Operation = Operation.toLong;
                transactionModelBase.Price = ask;
            }
            else if (mishmash.FromLong() == true)
            {
                Log.Information("Go from Long: " + transactionModelBase.Figi);
                transactionModelBase.Operation = Operation.fromLong;
                transactionModelBase.Price = bid;
            }
            Log.Information("Stop PurchaseDecision for: " + figi);
            return transactionModelBase;
        }


        private async Task BuyStoksAsync(TransactionModel transactionModel)
        {
            Log.Information("Start BuyStoks: " + transactionModel.Figi);
            List<Order> orders = await RetryPolicy.Model.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.OrdersAsync());
            foreach (Order order in orders)
            {
                if (order.Figi == transactionModel.Figi)
                {
                    await RetryPolicy.Model.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.CancelOrderAsync(order.OrderId));
                    Log.Information("Delete order by figi: " + transactionModel.Figi + " RequestedLots " + order.RequestedLots + " ExecutedLots " + order.ExecutedLots + " Price " + order.Price + " Operation " + order.Operation + " Status " + order.Status + " Type " + order.Type);
                }
            }
            int lots = await CalculationLotsByMarginAsync(transactionModel);
            //transactionModel.Quantity = await CalculationLotsByMargin(transactionModel);
            if (lots == 0)
            {
                Log.Information("Not any lot in margin: " + transactionModel.Purchase);
                return; }

            await RetryPolicy.Model.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.PlaceLimitOrderAsync(new LimitOrder(transactionModel.Figi, lots, OperationType.Buy, transactionModel.Price)));
            Instrument instrument = await marketDataCollector.GetInstrumentByFigi(transactionModel.Figi);
            using (StreamWriter sw = new StreamWriter("_operation", true, System.Text.Encoding.Default))
            {
                sw.WriteLine(DateTime.Now + @" Buy " + transactionModel.Figi + " " + instrument.Ticker + " Quantity: " + lots +  " price: " + transactionModel.Price + " mzda: " + (transactionModel.Price * 0.02m) / 100m);
                sw.WriteLine();
            }
            Log.Information("Create order for Buy " + lots + " lots " + "figi: " + transactionModel.Figi + "price: " + transactionModel.Price);
            Log.Information("Stop BuyStoks: " + transactionModel.Figi);
        }

        private async Task SellStoksFromLongAsync(TransactionModel transactionModel)
        {
            Log.Information("Start SellStoksFromLong: " + transactionModel.Figi);
            int lots = await CalculationStocksFromLongAsync(transactionModel);
            if (lots == 0)
            { return; }
            await RetryPolicy.Model.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.PlaceLimitOrderAsync(new LimitOrder(transactionModel.Figi, lots, OperationType.Sell, transactionModel.Price)));
            using (StreamWriter sw = new StreamWriter("_operation", true, System.Text.Encoding.Default))
            {
                sw.WriteLine(DateTime.Now + @" Sell " + transactionModel.Figi + " Quantity: " + lots + " price: " + transactionModel.Price + " mzda: " + (transactionModel.Price * 0.02m) / 100m);
                sw.WriteLine();
            }
            Log.Information("Create order for Sell " + lots + " stocks " + "figi: " + transactionModel.Figi + "price: " + transactionModel.Price);
            Log.Information("Stop SellStoksFromLong: " + transactionModel.Figi);
        }
        
        private async Task<int> CalculationStocksBuyCount(string figi, int countLotsToBuy)
        {
            int lots = await CountLotsInPortfolioAsync(figi);
            Log.Information("Need to buy stocks: " + countLotsToBuy);

            int countLotsToBuyReal = countLotsToBuy - lots;
            Log.Information("Real need to buy: " + countLotsToBuyReal);

            return countLotsToBuyReal;
        }
        private async Task<int> CalculationLotsByMarginAsync(TransactionModel transactionModel)
        {
            Log.Information("Start CalculationLotsByMargin method. Figi: " + transactionModel.Figi);
            int lots = await CountLotsInPortfolioAsync(transactionModel.Figi);
            Log.Information("Lots " + transactionModel.Figi + " in portfolio: " + lots);
            decimal sumCostLotsInPorfolio = transactionModel.Price * Convert.ToDecimal(lots);
            decimal remainingCostLots = transactionModel.Purchase - sumCostLotsInPorfolio;
            if (remainingCostLots <= 0)
            {
                Log.Information("Stop CalculationLotsByMargin method. Figi: " + transactionModel.Figi + " Return: 0");
                return 0;
            }
            int countLotsToBuy = Convert.ToInt32(Math.Floor(remainingCostLots / transactionModel.Price));
            if (countLotsToBuy <= transactionModel.Quantity)
            {
                Log.Information("Need to buy: " + countLotsToBuy);
                Log.Information("Stop CalculationLotsByMargin method. Figi: " + transactionModel.Figi );
                return countLotsToBuy;
            }
            else
            {
                Log.Information("Need to buy: " + transactionModel.Quantity);
                Log.Information("Stop CalculationLotsByMargin method. Figi: " + transactionModel.Figi );
                return transactionModel.Quantity;
            }
        }

        private async Task<int> CalculationStocksFromLongAsync(TransactionModel transactionModel)
        {
            Log.Information("Start CalculationStocksFromLong method. Figi: " + transactionModel.Figi);
            int lots = await CountLotsInPortfolioAsync(transactionModel.Figi);
            Log.Information("Lots " + transactionModel.Figi + " in portfolio: " + lots);
            if (lots <= transactionModel.Quantity)
            {
                Log.Information("Need to sell: " + lots);
                Log.Information("Stop CalculationStocksFromLong method. Figi: " + transactionModel.Figi);
                return lots;
            }
            else
            {
                Log.Information("Need to buy: " + transactionModel.Quantity);
                Log.Information("Stop CalculationStocksFromLong method. Figi: " + transactionModel.Figi);
                return transactionModel.Quantity;
            }
        }
        private async Task<int> CountLotsInPortfolioAsync(string figi)
        {
            Log.Information("Start CountLotsInPortfolio method. Figi: " + Figi);
            var portfolio = await RetryPolicy.Model.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.PortfolioAsync());
            int lots = 0;
            foreach (var item in portfolio.Positions)
            {
                if (item.Figi == figi)
                {
                    lots = item.Lots;
                    Log.Information("Lots " + figi + " in portfolio: " + lots);
                    Log.Information("Stop CountLotsInPortfolio method. Figi: " + Figi);
                    break;
                }
            };
            Log.Information("Stop CountLotsInPortfolio method. Figi: " + Figi);
            return lots;
        }
    }
}
