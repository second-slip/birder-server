import { Component, OnInit } from '@angular/core';
import { ObservationsAnalysisService } from '../observations-analysis.service';
import { AuthenticationService } from '../authentication.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-side-menu',
  templateUrl: './side-menu.component.html',
  styleUrls: ['./side-menu.component.scss']
})
export class SideMenuComponent implements OnInit {
  isLoggedIn: boolean;
  subscription: Subscription;

  constructor(private authenticationService: AuthenticationService) { }

  ngOnInit(): void {
    this.subscription = this.authenticationService.isAuthenticated$
      .subscribe(isLoggedIn => { this.isLoggedIn = isLoggedIn; });
    this.authenticationService.checkIsAuthenticated();
  }
}
