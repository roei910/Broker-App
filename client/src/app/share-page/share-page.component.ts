import { Component, Input, ViewChild } from '@angular/core';
import { CartService } from '../cart.service';
import { HttpClient, HttpContext, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-share-page',
  templateUrl: './share-page.component.html',
  styleUrls: ['./share-page.component.css'],
})
export class SharePageComponent {
  @ViewChild('purchase_type') purchaseType: any;
  @ViewChild('share_offer') shareOffer: any;
  @ViewChild('share_amount') shareAmount: any;
  // stock: Stock;
  stockResponse: any;
  transactions: any;
  openRequests: any;
  stockId: number;
  stockresponse: any;

  constructor(private service: CartService, 
    private http: HttpClient,
    private router: Router,
    private cRef: ChangeDetectorRef) {
    this.stockId = service.getStockId();
    this.initializePage();
  }

  initializePage(){
    this.getStockRespose();
    this.getStockTransactions();
    this.getOpenRequests();
  }

  getStockRespose() {
    var url = `https://localhost:7072/api/Shares/GetShare?ShareId=${
      this.stockId
    }`;
    this.http.get<any>(url).subscribe((data) => {
      if (data.statusCode == 404) 
        this.stockResponse = 'Problem fetching share from server';
      else 
        this.stockResponse = data.value;
    });
  }

  getStockTransactions() {
    var url = `https://localhost:7072/api/Shares/Share10Transactions?stockId=${
      // this.service.getStock().Id
      this.stockId
    }`;
    this.http.get<any>(url).subscribe((data) => {
      if (data.statusCode == 404) 
        this.transactions = 'Problem fetching share from server';
      else 
        this.transactions = data.value;
    });
  }

  getOpenRequests() {
    var url = `https://localhost:7072/api/Shares/GetShareOpenRequests?stockId=${
      this.stockId
    }`;
    this.http.get<any>(url).subscribe((data) => {
      if (data.statusCode == 404) 
        this.openRequests = 'Problem fetching share from server';
      else 
        this.openRequests = data.value;
    });
  }

  onSubmit(){
    var url = `https://localhost:7072/api/Shares/ShareRequest`;
    var body = {
      "id" : 0,
      "stockId" : this.stockId as number,
      "traderId" : this.service.getUserId() as number,
      "offer" : this.shareOffer.nativeElement.value as number,
      "amount" : this.shareAmount.nativeElement.value as number,
      "purchase" : (this.purchaseType.nativeElement.value == 'purchase')? true : false
    }; 
    this.http.post<any>(url, body).subscribe((data: any) => {
      if (data.statusCode == 404) 
        console.log('Problem with submitting data');
      else 
        console.log("purchase sent");
    });
  }
}
