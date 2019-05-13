import { Component } from '@angular/core';

@Component({
  selector: 'app-side-menu',
  templateUrl: './side-menu.component.html',
  styleUrls: ['./side-menu.component.scss']
})
export class SideMenuComponent {
  // isLoggedIn: boolean;
  // subscription: Subscription;

  constructor() { }

  // ngOnInit(): void {
  //   // this.subscription = this.authenticationService.isAuthenticated$
  //   //   .subscribe(isLoggedIn => { this.isLoggedIn = isLoggedIn; });
  //   // this.authenticationService.checkIsAuthenticated();
  // }
}
