import { Component, ViewEncapsulation } from '@angular/core';
import { AuthentificationService } from '../authentification.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LogoutComponent {

  constructor(private authentificationService: AuthentificationService
            , private router: Router) { }

  onLogout(): void {
    this.authentificationService.logout();
    this.router.navigate(['/']);
  }

  onCancel(): void {
    // TODO: go back to last view
    alert('cancel');
  }
}
