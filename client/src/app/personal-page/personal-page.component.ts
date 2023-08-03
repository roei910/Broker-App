import { Component } from '@angular/core';
import { CartService } from '../cart.service';
import { Trader } from '../trader';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-personal-page',
  templateUrl: './personal-page.component.html',
  styleUrls: ['./personal-page.component.css'],
})
export class PersonalPageComponent {
  name?: string;
  trader?: Trader;
  error?: string;
  lastDeals?: any;
  shares?: any;
  openRequests: any;

  constructor(private service: CartService, private http: HttpClient) {
    this.name = service.getUserName();
    this.updateTrader();
    this.getAllShares();
    this.getTraderOpenRequests();
  }

  updateTrader() {
    let url: string = `https://localhost:7072/api/Traders/GetTrader?traderId=${this.service.getUserId()}`;
    this.http.get<any>(url).subscribe((data) => {
      if (data.statusCode != 404) {
        this.trader = {
          Id: data.value.trader.id,
          Name: data.value.trader.name,
          Money: data.value.trader.money,
          Shares: data.value.shares == null ? null : data.value.shares,
        };
      }
    });

    url = `https://localhost:7072/api/Traders/Get8Transactions?traderId=${this.service.getUserId()}`;
    this.http.get<any>(url).subscribe((data) => {
      if (data.statusCode == 404) this.error = 'Problem connecting to server';
      else this.lastDeals = data.value;
    });
  }

  getAllShares() {
    const url: string = `https://localhost:7072/api/Shares/GetAllShares`;
    this.http.get<any>(url).subscribe((data) => {
      if (data.statusCode == 404) this.error = 'Problem connecting to server';
      else this.shares = data.value;
    });
  }

  getTraderOpenRequests() {
    var url = `https://localhost:7072/api/Traders/GetTraderOpenRequests?traderId=${this.service.getUserId()}`;
    this.http.get<any>(url).subscribe((data) => {
      if (data.statusCode == 404)
        this.openRequests = 'Problem fetching share from server';
      else this.openRequests = data.value;
    });
  }

  deleteRequest(requestId: number) {
    var url = `https://localhost:7072/api/Shares/CancelRequest?requestId=${requestId}`;
    this.http.post<any>(url, {}).subscribe((data) => {
      if (data.statusCode == 404)
        console.log('Error fetching shares from server');
      else 
        console.log('Successfully deleted request');
    });
  }
}
