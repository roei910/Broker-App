import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-traders-information-page',
  templateUrl: './traders-information-page.component.html',
  styleUrls: ['./traders-information-page.component.css']
})
export class TradersInformationPageComponent {
  traders?: any;
  shares?: any;
  transactions?: any;
  last10Transactions?: any;
  requests?: any;
  error?: any;

  constructor(private http: HttpClient) {
    this.getAllTraders();
    this.getAllShares();
    this.getAllOpenRequests();
    this.getAllTransactions();
   }

   getAllTraders(){
    const url: string = `https://localhost:7072/api/Traders/GetAllTraders`;
    this.http.get<any>(url).subscribe(data => {
      if(data.statusCode == 404)
        this.error = "Problem connecting to server";
      else
          this.traders = data.value;
    })
   }

   getAllTransactions(){
    const url: string = `https://localhost:7072/api/Shares/Get10Transactions`;
    this.http.get<any>(url).subscribe(data => {
      if(data.statusCode == 404)
        this.error = "Problem connecting to server";
      else
          this.transactions = data.value;
    })
   }

   getAllOpenRequests(){
    const url: string = `https://localhost:7072/api/Shares/GetAllOpenRequests`;
    this.http.get<any>(url).subscribe(data => {
      if(data.statusCode == 404)
        this.error = "Problem connecting to server";
      else
          this.requests = data.value;
    })
   }

   getAllShares(){
    const url: string = `https://localhost:7072/api/Shares/GetAllShares`;
    this.http.get<any>(url).subscribe(data => {
      if(data.statusCode == 404)
        this.error = "Problem connecting to server";
      else
          this.shares = data.value;
    })
   }
}
