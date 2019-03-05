import { Component, ViewEncapsulation } from '@angular/core';
import { AuthenticationService } from '../authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LogoutComponent {

  constructor(private authenticationService: AuthenticationService
            , private router: Router) { }

  onLogout(): void {
    this.authenticationService.logout();
    this.router.navigate(['/']);
  }

  onCancel(): void {
    // TODO: go back to last view
    alert('cancel');
  }
}
