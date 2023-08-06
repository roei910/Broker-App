using System;
using System.Text;
using BrokerAppAPI.Data;
using BrokerAppAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BrokerAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TradersController : ControllerBase
    {
        private readonly ApiContext _context;

        public TradersController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public JsonResult GetTrader(int traderId)
        {
            var result = _context.Traders.Find(traderId);
            //shares
            var openRequests = _context.OpenRequests.Where(
                request => request.TraderId == traderId).ToList();
            var shares = _context.TradersShares.Where(
                share => share.TraderId == traderId).ToList();
            var res = new
            {
                trader = result,
                requests = openRequests,
                shares = shares
            };
            if (result == null)
                return new JsonResult(NotFound());

            return new JsonResult(Ok(res));
        }
        
        [HttpGet]
        public JsonResult GetAllTraders()
        {
            var result = _context.Traders.ToList();

            return new JsonResult(Ok(result));
        }

        [HttpGet]
        public JsonResult GetTraderTransactions(int traderId)
        {
            var traderTransactions = _context.TransactionHistory.Where(
                transaction => transaction.TraderId == traderId).ToList();

            if (traderTransactions == null)
                return new JsonResult(NotFound());

            List<SharePurchase> lst = new List<SharePurchase>();
            if(traderTransactions.Count > 8)
            {
                for(int i = traderTransactions.Count-8; i < traderTransactions.Count; i++)
                {
                    lst.Add(traderTransactions[i]);
                }
                return new JsonResult(Ok(lst));
            }
            return new JsonResult(Ok(traderTransactions));
        }

        [HttpGet()]
        public JsonResult Get8Transactions(int traderId)
        {
            var result = _context.TransactionHistory.Where(
                transaction =>
                transaction.TraderId == traderId
            ).ToList();

            if (result.Count() > 8)
                return new JsonResult(Ok(result.GetRange(result.Count() - 8, 8)));
            return new JsonResult(Ok(result));
        }

        [HttpGet()]
        public JsonResult GetTraderOpenRequests(int traderId)
        {
            var result = _context.OpenRequests.Where(
                transaction =>
                transaction.TraderId == traderId).ToList();

            return new JsonResult(Ok(result));
        }
    }
}
