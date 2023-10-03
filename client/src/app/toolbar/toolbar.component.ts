import { Component } from '@angular/core';
import { CartService } from '../services/cart.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.css']
})
export class ToolbarComponent {
  constructor(private router: Router, private service: CartService){
  }

  //function to remove data from local storage and go to welcome-page
  disconnect(){
    localStorage.removeItem('session');
    this.router.navigate(['/welcome-page']);
  }
}
