import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Subscription } from 'rxjs';
import { UserViewModel } from '@app/_models/UserViewModel';
import { AuthenticationService } from '@app/_services/authentication.service';
import { TokenService } from '@app/_services/token.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss'],
  encapsulation: ViewEncapsulation.None
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
      this.authenticatedUser = this.tokenService.getAuthenticatedUserDetails();
    } else {
      this.authenticatedUser = null;
    }
  }
}

