import { Component } from '@angular/core';
import { CartService } from '../../cart.service';
import { Trader } from '../../models/trader';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-personal-page',
  templateUrl: './personal-page.component.html',
  styleUrls: ['./personal-page.component.css'],
})
export class PersonalPageComponent {
  trader: Trader;
  lastDeals?: any;
  shares?: any;
  openRequests: any;
  chart?: any = null;


  constructor(private service: CartService, private http: HttpClient) {
    this.trader = this.service.getTraderFromStorage();
    if (this.trader != {} as Trader) {
      this.updateTrader();
      this.getAllShares();
      this.getTraderOpenRequests();
    }
  }

  ngOnInit() {

  }

  ngAfterViewChecked(){
    this.createChartOnInit();
  }

  updateTrader() {
    let url: string = `https://localhost:7072/api/Traders/GetTrader?traderId=${this.trader.Id}`;
    this.http.get<any>(url).subscribe((data) => {
      if (data.statusCode != 404) {
        this.trader = {
          Id: data.value.trader.id,
          Name: data.value.trader.name,
          Money: data.value.trader.money,
          Shares: data.value.shares,
        };
        // localStorage.setItem('shares', JSON.stringify(data.value.shares));
      }
    });

    url = `https://localhost:7072/api/Traders/Get8Transactions?traderId=${this.trader?.Id}`;
    this.http.get<any>(url).subscribe((data) => {
      if (data.statusCode == 404)
        alert('Problem connecting to server');
      else this.lastDeals = data.value;
    });
  }

  getAllShares() {
    const url: string = `https://localhost:7072/api/Shares/GetAllShares`;
    this.http.get<any>(url).subscribe((data) => {
      if (data.statusCode == 404)
        alert('Problem connecting to server');
      else {
        this.shares = data.value;
        // localStorage.setItem('stocks', JSON.stringify(data.value));
      }
    });
  }

  getTraderOpenRequests() {
    var url = `https://localhost:7072/api/Traders/GetTraderOpenRequests?traderId=${this.trader?.Id}`;
    this.http.get<any>(url).subscribe((data) => {
      if (data.statusCode == 404)
        alert('Problem fetching share from server');
      else this.openRequests = data.value;
    });
  }

  deleteRequest(requestId: number) {
    var url = `https://localhost:7072/api/Shares/CancelRequest?requestId=${requestId}`;
    this.http.post<any>(url, {}).subscribe((data) => {
      if (data.statusCode == 404)
        alert('Error fetching shares from server');
      else
        alert('Successfully deleted request');
    });
    window.location.reload();
  }

  createChartOnInit() {
    if(this.trader.Shares.length == 0)
      return;
    var data = [];
    var chartData = [];
    var sum = 0;
    for (var i = 0; i < this.trader.Shares.length; i++) {
      data.push({ shares: this.trader.Shares[i].amount, id: this.trader.Shares[i].stockId });
      sum += this.trader.Shares[i].amount;
    }
    for (var i = 0; i < data.length; i++) {
      chartData.push(({ y: (100.0 * data[i].shares) / sum, name: this.shares[data[i].id - 1].name }));
    }
    var chartOptions = {
      animationEnabled: true,
      title: {
        text: "Shares by Stock Name"
      },
      data: [{
        type: "pie",
        startAngle: -90,
        indexLabel: "{name}: {y}",
        yValueFormatString: "#,###.##'%'",
        dataPoints: chartData
      }]
    };
    this.chart = chartOptions;
  }

}
