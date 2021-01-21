import { Component, ViewEncapsulation } from '@angular/core';
import { AuthenticationService } from '../_services/authentication.service';
import { Router } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LogoutComponent {

  constructor(private authenticationService: AuthenticationService
    , private router: Router, private location: Location) { }

  onLogout(): void {
    this.authenticationService.logout();
    this.router.navigate(['home']);
  }

  onNo(): void {
    this.location.back();
  }
}
