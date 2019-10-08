import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../_services/authentication.service';
import { Subscription } from 'rxjs';
import { UserViewModel } from '../_models/UserViewModel';
import { TokenService } from '../_services/token.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isNavbarCollapsed = true;
  isLoggedIn: boolean;
  subscription: Subscription;
  authenticatedUser: UserViewModel;

  constructor(private authenticationService: AuthenticationService
    , private tokenService: TokenService) { }

  ngOnInit(): void {
    this.subscription = this.authenticationService.isAuthenticated$
      .subscribe(isLoggedIn => {
        this.isLoggedIn = isLoggedIn;
        this.updateAuthenticatedUser();
      });
    this.authenticationService.checkIsAuthenticated();
  }

  updateAuthenticatedUser(): void {
    if (this.isLoggedIn === true) {
      this.tokenService.getAuthenticatedUserDetails()
      .subscribe(
        (data: UserViewModel) => {
          this.authenticatedUser = data;
        },
        (error: any) => {
          this.authenticatedUser = null;
          // ToDo: redirect to login?
        }
      );
    } else {
      this.authenticatedUser = null;
    }
  }
}
