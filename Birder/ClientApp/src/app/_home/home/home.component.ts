import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '@app/_services/authentication.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class HomeComponent implements OnInit {

  constructor(private router: Router
    , private authenticationService: AuthenticationService) { }

  ngOnInit(): void {
    if (this.authenticationService.checkIsAuthenticated()) {
      this.router.navigate(['/observation-feed']);
    }
  }
}
