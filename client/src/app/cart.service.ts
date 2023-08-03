import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  userId: number = 0;
  userName: string = "";
  stockId: number = 0;

  constructor(private http: HttpClient) { }

  setUserData(id: number, name: string){
    this.userId = id;
    this.userName = name
  }

  getUserId(){
    return this.userId;
  }

  getUserName(){
    return this.userName;
  }

  setStockId(id: number){
    this.stockId = id;
  }
  
  getStockId(){
    return this.stockId;
  }
}
