import { Component } from '@angular/core';
import { CartService } from '../cart.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.css']
})
export class ToolbarComponent {
  constructor(private router: Router, private service: CartService){
  }

  disconnect(){
    this.service.setUserData(0, "");
    this.router.navigate(['/welcome-page']);
  }
}
