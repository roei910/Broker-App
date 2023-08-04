import { Component, Input, ViewChild } from '@angular/core';
import { CartService } from '../cart.service';
import { HttpClient, HttpContext, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { ChangeDetectorRef } from '@angular/core';
import { Trader } from '../trader';

@Component({
  selector: 'app-share-page',
  templateUrl: './share-page.component.html',
  styleUrls: ['./share-page.component.css'],
})
export class SharePageComponent {
  @ViewChild('purchase_type') purchaseType: any;
  @ViewChild('share_offer') shareOffer: any;
  @ViewChild('share_amount') shareAmount: any;
  trader?: Trader;
  stock: any;
  transactions: any;
  openRequests: any;
  stockId: any;

  //constructor for share-page
  constructor(private service: CartService, 
    private http: HttpClient,
    private router: Router,
    private cRef: ChangeDetectorRef) {
    //get trader from local storage
    let traderData = this.service.getTraderFromStorage();
    if(traderData)
      this.trader = traderData;
    //get stock information from local storage
    let stockData = localStorage.getItem('stock_session');
    if(stockData){
      this.stockId = JSON.parse(stockData).stockId;
      this.initializePage();
    }
    else
      alert('Error has occurred');
  }

  //initialize all variables
  initializePage(){
    this.getStock();
    this.getStockTransactions();
    this.getOpenRequests();
  }

  //get stock information from server
  getStock() {
    var url = `https://localhost:7072/api/Shares/GetShare?ShareId=${
      this.stockId
    }`;
    this.http.get<any>(url).subscribe((data) => {
      if (data.statusCode == 404) 
        alert('Problem fetching share from server');
      else 
        this.stock = data.value;
    });
  }

  //get all transaction for the current stock from server
  getStockTransactions() {
    var url = `https://localhost:7072/api/Shares/Share10Transactions?stockId=${
      this.stockId
    }`;
    this.http.get<any>(url).subscribe((data) => {
      if (data.statusCode == 404) 
        alert('Problem fetching share from server');
      else 
        this.transactions = data.value;
    });
  }

  //get all open requests for the current stock from server
  getOpenRequests() {
    var url = `https://localhost:7072/api/Shares/GetShareOpenRequests?stockId=${
      this.stockId
    }`;
    this.http.get<any>(url).subscribe((data) => {
      if (data.statusCode == 404) 
        alert('Problem fetching share from server');
      else 
        this.openRequests = data.value;
    });
  }

  //submit a new share request
  onSubmit(){
    var url = `https://localhost:7072/api/Shares/ShareRequest`;
    var body = {
      "id" : 0,
      "stockId" : this.stock.id as number,
      "traderId" : this.trader?.Id as number,
      "offer" : this.shareOffer.nativeElement.value as number,
      "amount" : this.shareAmount.nativeElement.value as number,
      "purchase" : (this.purchaseType.nativeElement.value == 'purchase')? true : false
    }; 
    this.http.post<any>(url, body).subscribe((data: any) => {
      if (data.statusCode == 404) 
        alert('Problem with submitting data');
      else if(data.value.message != undefined)
        alert(data.value.message);
      else{
        alert("purchase sent");
        window.location.reload();
      }
    });
  }
}
