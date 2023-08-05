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
                        {
                            traderStock.ElementAt(0).Amount += shareRequest.Amount;
                        }
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
                        {
                            traderStock.ElementAt(0).Amount += share.Amount;
                        }
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
                if (availableShares.Count() == 0 ||
                    availableShares.ElementAt(0).Amount < shareRequest.Amount)
                {
                    var res = new
                    {
                        message = "Not Enough Shares To Sell"
                    };
                    return new JsonResult(Ok(res));
                }

                //check price to sell or add new request
                if (stock.CurrentPrice < shareRequest.Offer)
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
                        {
                            traderStock.ElementAt(0).Amount -= shareRequest.Amount;
                        }
                    }
                    else
                    {
                        var res = new
                        {
                            message = "couldnt find shares in assets"
                        };
                        return new JsonResult(Ok(res));
                    }
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
                        {
                            traderStock.ElementAt(0).Amount -= shareRequest.Amount;
                        }
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

            var trader = _context.Traders.Find(result.TraderId);
            if (trader == null)
                return new JsonResult(NotFound());

            if (result.Purchase)
            {
                //return money to trader
                trader.Money += result.Offer * result.Amount;
            }
            else
            {
                //return shares to trader
                //problem with remembering the original price for the total price.
                //need to save the data to return to the trader
                _context.TradersShares.Add(new Share
                {
                    StockId = result.StockId,
                    Amount = result.Amount,
                    TraderId = result.TraderId
                });
            }

            //remove open request
            _context.OpenRequests.Remove(result);
            _context.SaveChanges();
            return new JsonResult(Ok(result));
        }


        //traderId - the trader to allow the share request to happend,
        //if the share request trader wants to buy then the trader id is the seller.
        [HttpPost]
        public JsonResult TransferFunds(int shareRequestId, int traderId)
        {
            var request = _context.OpenRequests.Find(shareRequestId);
            if(request == null)
                return new JsonResult(NotFound());
            var stock = _context.Stocks.Find(request.StockId);
            var trader = _context.Traders.Find(request.TraderId);
            var trader2 = _context.Traders.Find(traderId);
            if (trader == null || trader2 == null || stock == null)
                return new JsonResult(NotFound());

            //check trader2 can initiate
            if (request.Purchase)
            {
                //check if trader2 available shares to sell
                var availableShares = _context.TradersShares.Where(
                    share =>
                    share.TraderId == trader2.Id &&
                    share.StockId == request.StockId).ToList();
                if (availableShares.Count() == 0 ||
                    availableShares.ElementAt(0).Amount < request.Amount)
                {
                    var res = new
                    {
                        message = "Not Enough Shares To Sell"
                    };
                    return new JsonResult(Ok(res));
                }

                //update trader1
                //update money after purchase
                trader.Money -= request.Offer * request.Amount;
                //add buy transaction
                _context.TransactionHistory.Add(new SharePurchase
                {
                    Amount = request.Amount,
                    StockId = request.StockId,
                    Price = request.Offer,
                    Purchase = request.Purchase,
                    TraderId = request.TraderId
                });
                //add shares to trader shares, check for existing share
                var traderStock = _context.TradersShares.Where(
                    trdrStck =>
                    trdrStck.TraderId == trader.Id &&
                    trdrStck.StockId == stock.Id).ToList();
                if (traderStock.Count > 0)
                {
                    traderStock.ElementAt(0).Amount += request.Amount;
                }
                else
                _context.TradersShares.Add(new Share
                {
                    Amount = request.Amount,
                    StockId = request.StockId,
                    TraderId = request.TraderId
                });

                //update trader2
                //decrease money after sale
                trader2.Money -= request.Offer * request.Amount;
                //add sell transaction
                _context.TransactionHistory.Add(new SharePurchase
                {
                    Amount = request.Amount,
                    StockId = request.StockId,
                    Price = request.Offer,
                    Purchase = !request.Purchase,
                    TraderId = trader2.Id
                });
                //remove shares
                var trader2Stock = _context.TradersShares.Where(
                    share =>
                    share.TraderId == trader2.Id &&
                    share.StockId == request.StockId).ToList();

                if (trader2Stock.ElementAt(0).Amount == request.Amount)
                    _context.TradersShares.Remove(trader2Stock.ElementAt(0));
                else
                {
                    trader2Stock.ElementAt(0).Amount -= request.Amount;
                }

            }
            else
            {
                //check if trader1 has enough money
                if (trader2.Money < stock.CurrentPrice * request.Amount)
                {
                    var res = new
                    {
                        message = "Cannot Create Purchase, " +
                        "Not Enough Available Funds"
                    };
                    return new JsonResult(Ok(res));
                }

                //update trader1
                //update money after sale
                trader.Money += request.Offer * request.Amount;
                //add sell transaction
                _context.TransactionHistory.Add(new SharePurchase
                {
                    Amount = request.Amount,
                    StockId = request.StockId,
                    Price = request.Offer,
                    Purchase = request.Purchase,
                    TraderId = request.TraderId
                });
                
                //update trader2
                //add money after purchase
                trader2.Money -= request.Offer * request.Amount;
                //add buy transaction
                _context.TransactionHistory.Add(new SharePurchase
                {
                    Amount = request.Amount,
                    StockId = request.StockId,
                    Price = request.Offer,
                    Purchase = !request.Purchase,
                    TraderId = trader2.Id
                });

                //add shares to trader shares, check for existing share
                var trader2Stock = _context.TradersShares.Where(
                    trdrStck =>
                    trdrStck.TraderId == trader.Id &&
                    trdrStck.StockId == stock.Id).ToList();
                if (trader2Stock.Count > 0)
                {
                    trader2Stock.ElementAt(0).Amount += request.Amount;
                }
                else
                {
                    _context.TradersShares.Add(new Share
                    {
                        StockId = request.StockId,
                        Amount = request.Amount,
                        TraderId = trader2.Id
                    });
                }
        }

            //remove open request
            _context.OpenRequests.Remove(request);

            _context.SaveChanges();
            return new JsonResult(Ok(request));
        }
    }
}
