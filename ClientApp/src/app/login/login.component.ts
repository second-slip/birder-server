import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AuthentificationService } from '../authentification.service';

@Component({
  selector: 'login',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {
  invalidLogin: boolean;

  constructor(private router: Router
    , private authentificationService: AuthentificationService) { }

  ngOnInit() {
    // reset login status
    this.authentificationService.logout();

    // get return url from route parameters or default to '/'
  }

  login(form: NgForm) {
    let credentials = JSON.stringify(form.value);

    this.authentificationService.login(credentials)
      .pipe(first())
      .subscribe(
        data => {
          // this.router.navigate([this.returnUrl]);
          console.log('successful login');
          this.router.navigate(['/']);
        },
        error => {
          // this.error = error;
          // this.loading = false;
          console.log('UN-successful login');
          this.invalidLogin = true;
        });
  }
}

