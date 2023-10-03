import { Injectable } from '@angular/core';
import { Trader } from './models/trader';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  constructor() { }

  //cart service function to retrieve the trader information from local storage
  getTraderFromStorage(){
    let data = localStorage.getItem('session');
    if (data){
      return {
        Id: JSON.parse(data).trader.id,
        Name: JSON.parse(data).trader.name,
        Money: JSON.parse(data).trader.money,
        Shares: JSON.parse(data).shares
      } as Trader;
    }
    else
      alert('Error has occurred');
    return {} as Trader;
  }
}
