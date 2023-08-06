import { Component } from '@angular/core';
import { CartService } from '../cart.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-tradering-page',
  templateUrl: './trading-page.component.html',
  styleUrls: ['./trading-page.component.css']
})
export class TradingPageComponent {
  name?: string;
  sharesJson?: any;
  error?: string;

  //constructor for trading-page
  constructor(private service: CartService, private router: Router, private http: HttpClient) {
    //get user data from local storage if exists
    let data = localStorage.getItem('session');
    if (data) {
      this.name = JSON.parse(data).trader.name;
      this.getAllShares()
    }
    else
      alert('Error fetching data from local storage, please reconnect');
  }

  //route to share-page and save the stock chosen
  sharePage(stockId: number) {
    let data = {stockId: stockId}
    localStorage.setItem('stock_session', JSON.stringify(data));
    this.router.navigate(['/share-page']);
  }

  //get all shares data from server
  getAllShares(){
    const url: string = `https://localhost:7072/api/Shares/GetAllShares`;
      this.http.get<any>(url).subscribe(data => {
        if (data.statusCode == 404)
          alert("Problem connecting to server");
        else
          this.sharesJson = data.value;
      })
  }
}
