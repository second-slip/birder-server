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
  // loginViewModel: LoginViewModel;

  constructor(private router: Router
            , private authentificationService: AuthentificationService) { }

  ngOnInit() {
    this.authentificationService.logout();
    // this.loginViewModel = new LoginViewModel();
    // alert(this.loginViewModel.password);
    // TODO: get return url from route parameters or default to '/'
  }

  login(form: NgForm) {
    // let credentials = JSON.stringify(form.value);
    // this.loginViewModel = form.value;
    // console.log(this.eg);
    // var model: LoginViewModel;
    // model = form.value;
     console.log('form data: ' + form.value);

    this.authentificationService.login(form.value)
      .pipe(first())
      .subscribe(
        data => {
          // TODO: remove console log
          console.log('successful login');
          this.router.navigate(['/']);
        },
        error => {
          this.invalidLogin = true;
          // TODO: remove console log
          console.log('UN-successful login');
        });
  }
}

export interface LoginViewModel {
  userName: string;
  password: string;
}

