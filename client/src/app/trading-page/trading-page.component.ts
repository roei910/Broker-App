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

  constructor(private service: CartService, private router: Router, private http: HttpClient) {
    let data = localStorage.getItem('session');
    if (data) {
      this.name = JSON.parse(data).trader.name;
      const url: string = `https://localhost:7072/api/Shares/GetAllShares`;
      this.http.get<any>(url).subscribe(data => {
        if (data.statusCode == 404)
          alert("Problem connecting to server");
        else
          this.sharesJson = data.value;
      })
    }
    else
      alert('Error has occurred');
  }

  sharePage(stockId: number) {
    let data = {stockId: stockId}
    localStorage.setItem('stock_session', JSON.stringify(data));
    this.router.navigate(['/share-page']);
  }
}
