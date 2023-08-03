import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CartService } from '../cart.service';

@Component({
  selector: 'app-welcome-page',
  templateUrl: './welcome-page.component.html',
  styleUrls: ['./welcome-page.component.css']
})
export class WelcomePageComponent {
  response: any;

  constructor(private router: Router, private http: HttpClient, private service: CartService){
    if(this.service.getUserId() != 0)
      this.router.navigate(['/trading-page']);
  }
  
  routePage(value: any){
    const url: string = `https://localhost:7072/api/Traders/GetTrader?traderId=${value}`;
    this.http.get<any>(url).subscribe(data => {
      if(data.statusCode == 404)
        this.response = ", Error: invalid id";
      else{
          this.service.setUserData(data.value.trader.id, data.value.trader.name);
          this.router.navigate(['/trading-page']);
        }
    })
  }
}
