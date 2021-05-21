import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AuthenticationService } from '@app/_services/authentication.service';


@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ConfirmEmailComponent implements OnInit {

  constructor(private authenticationService: AuthenticationService) { }

  ngOnInit() {
    this.authenticationService.logout(); // for users redirected from account manager
  }

}
