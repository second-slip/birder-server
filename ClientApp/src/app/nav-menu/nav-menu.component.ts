import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../authentication.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isNavbarCollapsed = true;
  isLoggedIn: boolean;

  subscription: Subscription;

  constructor(private authenticationService: AuthenticationService) {}

  ngOnInit(): void {
    this.subscription = this.authenticationService.isAuthenticated$
      .subscribe(isLoggedIn => this.isLoggedIn = isLoggedIn);
      // check login
      this.checkLoggedInStatus();
  }

  checkLoggedInStatus(): void {
    this.authenticationService.checkIsAuthenticated();
  }
}
