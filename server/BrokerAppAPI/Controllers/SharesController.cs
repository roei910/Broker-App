using BrokerAppAPI.Data;
using BrokerAppAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrokerAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SharesController : ControllerBase
    {
        private readonly ApiContext _context;

        public SharesController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public JsonResult GetShare(int ShareId)
        {
            var result = _context.Stocks.Find(ShareId);

            if (result == null)
                return new JsonResult(NotFound());

            return new JsonResult(Ok(result));
        }

        [HttpGet()]
        public JsonResult GetAllShares()
        {
            var result = _context.Stocks.ToList();

            return new JsonResult(Ok(result));
        }

        [HttpGet()]
        public JsonResult GetAllTransactions()
        {
            var result = _context.TransactionHistory.ToList();

            return new JsonResult(Ok(result));
        }

        [HttpGet()]
        public JsonResult Get10Transactions()
        {
            var result = _context.TransactionHistory.ToList();
            if(result.Count() > 10)
                return new JsonResult(Ok(result.GetRange(result.Count() - 10, 10)));
            return new JsonResult(Ok(result));
        }

        [HttpGet()]
        public JsonResult Share10Transactions(int stockId)
        {
            var result = _context.TransactionHistory.Where(
                transaction => 
                transaction.StockId == stockId).ToList();
            if (result.Count() > 10)
                return new JsonResult(Ok(result.GetRange(result.Count() - 10, 10)));
            return new JsonResult(Ok(result));
        }

        [HttpGet()]
        public JsonResult GetShareOpenRequests(int stockId)
        {
            var result = _context.OpenRequests.Where(
                transaction =>
                transaction.StockId == stockId).ToList();
            return new JsonResult(Ok(result));
        }

        [HttpGet()]
        public JsonResult GetAllOpenRequests()
        {
            var result = _context.OpenRequests.ToList();
            return new JsonResult(Ok(result));
        }

        [HttpPost]
        public JsonResult ShareRequest(ShareRequest shareRequest)
        {
            var stock = _context.Stocks.Find(shareRequest.StockId);
            var trader = _context.Traders.Find(shareRequest.TraderId);

            //couldnt find trader or stock
            if (stock == null || trader == null)
                return new JsonResult(NotFound());

            var sharePurchase = new SharePurchase
            {
                StockId = stock.Id,
                Amount = shareRequest.Amount,
                Price = shareRequest.Offer,
                Purchase = shareRequest.Purchase,
                TraderId = shareRequest.TraderId
            };
            var share = new Share
            {
                StockId = stock.Id,
                Amount = shareRequest.Amount,
                TraderId = shareRequest.TraderId
            };

            //search for open request with different type of purchase
            if (_context.OpenRequests.Where(
                request => request.StockId == stock.Id &&
                request.Purchase != shareRequest.Purchase &&
                request.TraderId == shareRequest.TraderId).ToList().Count() > 0)
            {
                var res = new
                {
                    message = "cannot create purchase, " +
                    "another open deal contradicting the purchase"
                };
                return new JsonResult(Ok(res));
            }

            //purchase request
            if (shareRequest.Purchase)
            {
                //check if trader has enough money
                if (trader.Money < stock.CurrentPrice * shareRequest.Amount)
                {
                    var res = new
                    {
                        message = "cannot create purchase, " +
                        "not enough available funds"
                    };
                    return new JsonResult(Ok(res));
                }

                //check price to purchase or add new request
                if (stock.CurrentPrice > shareRequest.Offer)
                {
                    _context.OpenRequests.Add(shareRequest);
                    trader.Money -= sharePurchase.Amount * sharePurchase.Price;
                }
                else
                {
                    sharePurchase.Price = stock.CurrentPrice;
                    shareRequest.Offer = stock.CurrentPrice;
                    //check if the trader can buy the entire amount
                    if (stock.Amount >= shareRequest.Amount)
                    {
                        //check if trader has the stock in his shares
                        var traderStock = _context.TradersShares.Where(
                            trdrStck =>
                            trdrStck.TraderId == trader.Id &&
                            trdrStck.StockId == stock.Id).ToList();
                        if (traderStock.Count > 0)
                            traderStock.ElementAt(0).Amount += shareRequest.Amount;
                        else
                            _context.TradersShares.Add(share);
                        //add the transaction
                        _context.TransactionHistory.Add(sharePurchase);
                        //update stock and trader funds
                        stock.Amount -= sharePurchase.Amount;
                        trader.Money -= sharePurchase.Amount * sharePurchase.Price;
                    }
                    //if not, buy available. send rest to open request
                    else
                    {
                        sharePurchase.Amount = stock.Amount;
                        _context.TransactionHistory.Add(sharePurchase);
                        //add shares to trader shares, check for existing share
                        share.Amount = stock.Amount;
                        var traderStock = _context.TradersShares.Where(
                            trdrStck =>
                            trdrStck.TraderId == trader.Id &&
                            trdrStck.StockId == stock.Id).ToList();
                        if (traderStock.Count > 0)
                            traderStock.ElementAt(0).Amount += share.Amount;
                        else
                            _context.TradersShares.Add(share);
                        shareRequest.Amount = shareRequest.Amount - stock.Amount;
                        _context.OpenRequests.Add(shareRequest);
                        //update stock and trader funds
                        stock.Amount = 0;
                        trader.Money -= sharePurchase.Amount * sharePurchase.Price;
                    }
                    //update purchase counter in stock
                    stock.CountPurchase += 1;
                    if(stock.CountPurchase >= 10)
                    {
                        stock.CurrentPrice = Convert.ToInt32(Math.Ceiling(stock.CurrentPrice * 1.01));
                        stock.CountPurchase -= 10;
                    }
                }
            }
            //sell request
            else
            {
                //check if available shares to sell
                var availableShares = _context.TradersShares.Where(
                    share =>
                    share.TraderId == trader.Id &&
                    share.StockId == stock.Id).ToList();
                if (availableShares.Count() > 0 &&
                    availableShares.ElementAt(0).Amount < shareRequest.Amount)
                {
                    var res = new
                    {
                        message = "not enough shares to sell"
                    };
                    return new JsonResult(Ok(res));
                }
                    

                //check price to sell or add new request
                if (stock.CurrentPrice < shareRequest.Offer)
                {
                    _context.OpenRequests.Add(shareRequest);
                }
                else
                {
                    //find trader stock by id if exists
                    var traderStock = _context.TradersShares.Where(
                        share =>
                        share.TraderId == shareRequest.TraderId &&
                        share.StockId == shareRequest.StockId).ToList();

                    if (traderStock.Count > 0)
                    {
                        if (traderStock.ElementAt(0).Amount == shareRequest.Amount)
                            _context.TradersShares.Remove(traderStock.ElementAt(0));
                        else
                            traderStock.ElementAt(0).Amount -= shareRequest.Amount;
                    }
                    else
                    {
                        var res = new
                        {
                            message = "couldnt find shares in assets"
                        };
                        return new JsonResult(Ok(res));
                    }

                    //add transaction
                    _context.TransactionHistory.Add(sharePurchase);

                    //change stock amount and trader funds
                    stock.Amount += shareRequest.Amount;
                    trader.Money += sharePurchase.Amount * sharePurchase.Price;
                }
                //update sell counter in stock
                stock.CountSale += 1;
                if (stock.CountSale >= 4)
                {
                    stock.CurrentPrice = Convert.ToInt32(Math.Ceiling(stock.CurrentPrice * 0.99));
                    stock.CountSale -= 4;
                }
            }
            
            _context.SaveChanges();
            return new JsonResult(Ok(shareRequest));
        }

        [HttpPost]
        public JsonResult CancelRequest(int requestId)
        {
            var result = _context.OpenRequests.Find(requestId);

            if (result == null)
                return new JsonResult(NotFound());

            //return money to trader
            var trader = _context.Traders.Find(result.TraderId);
            if(trader  == null)
                return new JsonResult(NotFound());
            trader.Money += result.Offer * result.Amount;

            //remove open request
            _context.OpenRequests.Remove(result);
            _context.SaveChanges();
            return new JsonResult(Ok(result));
        }


    }
}
