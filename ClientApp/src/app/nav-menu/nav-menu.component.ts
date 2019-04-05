import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../authentication.service';
import { Subscription } from 'rxjs';
import { UserViewModel } from '../../_models/UserViewModel';
import { JwtHelperService } from '@auth0/angular-jwt';

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
    , private jwtHelper: JwtHelperService) { }

  ngOnInit(): void {
    this.subscription = this.authenticationService.isAuthenticated$
      .subscribe(isLoggedIn => {
        this.isLoggedIn = isLoggedIn;
        this.updateAuthenticatedUser();
      });
    this.authenticationService.checkIsAuthenticated();
  }

  // checkLoggedInStatus(): void {
  //   this.authenticationService.checkIsAuthenticated();
  // }



  updateAuthenticatedUser(): void {
    // const token = localStorage.getItem('jwt');
    if (this.isLoggedIn === true) {
      const token = localStorage.getItem('jwt');
      if (token && !this.jwtHelper.isTokenExpired(token)) {
        const tokenDecoded = this.jwtHelper.decodeToken(token);
        const user = <UserViewModel>{
          userName: tokenDecoded.unique_name,
          profileImage: '',
          defaultLocationLatitude: 0,
          defaultLocationLongitude: 0
        };
        this.authenticatedUser = user;
      }
    } else {
      this.authenticatedUser = null;
    }
  }
}


