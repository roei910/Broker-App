import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CartService } from '../cart.service';

@Component({
  selector: 'app-welcome-page',
  templateUrl: './welcome-page.component.html',
  styleUrls: ['./welcome-page.component.css']
})
export class WelcomePageComponent {
  @ViewChild('broker_id') brokerId: any;

  constructor(private router: Router, private http: HttpClient, private service: CartService) {
    let data = localStorage.getItem('session')
    if (data)
      this.router.navigate(['/trading-page']);

  }

  routePage() {
    let id = this.brokerId.nativeElement.value
    const url: string = `https://localhost:7072/api/Traders/GetTrader?traderId=${id}`;
    this.http.get<any>(url).subscribe(data => {
      if (data.statusCode == 404)
        alert("Error: invalid id");
      else {
        // let jsonData = {
        //   broker_id: id,
        //   broker_name: data.value.trader.name
        // }
        //save data in local storage
        localStorage.setItem('session', JSON.stringify(data.value));
        this.router.navigate(['/trading-page']);
      }
    })
  }
}
