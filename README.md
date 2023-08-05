# BrokerApp
 
Front-end: angular
Back-end: .net core

backend:
allows traders to purchase or sell stocks to the market at the current price.
allow traders to trade between each other for prices different than current stock price.

shares http request:
1. GetShare, get share information by id.
2. GetAllShares, get all the shares information.
3. GetAllTransactions, gets all the transactions made in the broker app.
4. Get10Transactions, gets the last 10 transaction.
5. Share10Transactions, gets the last 10 transactions by stock id.
6. GetShareOpenRequests, gets all open share requests by stock id.
7. GetAllOpenRequests, gets all open share requests.
8. ShareRequest, posts a share request, buys or sell if the prices match the current stock price,
otherwise open a request to allow traders to interact.
9. CancelRequest, cancels a share request by request id.
10. TransferFunds, makes a trade between two traders by share request id and trader id (the latter does the opposite of the request purchase type)

traders http request:
1. GetTrader, gets the trader information by id.
2. GetAllTraders, gets all the traders information from db.
3. GetTraderTransactions, gets all the trader transaction by trader id.
4. Get8Transactions, gets the last 8 transaction made by the trader.
5. GetTraderOpenRequests, gets all the traders open requests by trader id.


frontend:
connection page:
allow connection by http request to the server.

trading page:
allows the user to see all the stocks prices and available for purchase from the market (not from other traders)

share page:
allows user to see the 10 last transactions on the stock, see open transactions and interact with them, make new share requests.

traders page:
allows the user to see all traders names and ids, allow to see the last 10 transactions made by all users, allows the user to see all the open transactions.

personal page:
allows the user to see all the funds and stocks he has. also he can see his last 8 transactions and open transaction. if the user has stocks he would be able to see the stocks in a pie chart to see how many stocks he has by amount!!, not by prices.
