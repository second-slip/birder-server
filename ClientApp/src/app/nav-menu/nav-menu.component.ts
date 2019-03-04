import { Component, OnInit } from '@angular/core';
import { AuthentificationService } from '../authentification.service';
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

  constructor(private authentificationService: AuthentificationService) {}

  ngOnInit(): void {
    this.subscription = this.authentificationService.isLoggedIn$
      .subscribe(isLoggedIn => this.isLoggedIn = isLoggedIn);
      // check login
      this.checkLoggedInStatus();
  }

  checkLoggedInStatus(): void {
    this.authentificationService.checkLoginStatus();
  }
}
