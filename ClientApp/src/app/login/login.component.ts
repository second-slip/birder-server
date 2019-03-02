import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { LocalStorageService } from '../local-storage.service';
import { HttpService, LoginViewModel } from '../http.service';
import { JwtHelper } from 'angular2-jwt';
import { LoginService } from '../login.service';

@Component({
  selector: 'login',
  templateUrl: './login.component.html'
})
export class LoginComponent {
  invalidLogin: boolean;
  // newToken: any;
  // thing: string;

  constructor(private router: Router
            , private http: HttpClient
            , private localStorageService: LocalStorageService
            , private httpService: HttpService
            , private loginService: LoginService) { }

  login(form: NgForm) {
    let credentials = JSON.stringify(form.value);
   // let token1 = token;

    // console.log('y: ' + this.newToken);

    this.httpService.getHeroes(credentials)
    .subscribe(response => {
      console.log('response: ' + response);
        let token = (<any>response).token;

        this.localStorageService.addToken(token);
  
        this.invalidLogin = false;

        console.log('successful login');

        this.router.navigate(['/']);

      });
      


    // .subscribe(heroes => this.newToken = heroes);


    // this.newToken = (<any>this.newToken).token;
    // console.log('x: ' + this.newToken);


    //let credentials = JSON.stringify(form.value);
    // this.http.post('http://localhost:55722/api/Account/Login', credentials, {
    //   headers: new HttpHeaders({
    //     'Content-Type': 'application/json'
    //   })
    // }).subscribe(response => {
    //   let token = (<any>response).token;
    //   this.localStorageService.addToken(token);
    //   // localStorage.setItem('jwt', token);

    //   this.invalidLogin = false;
    //   console.log('successful login');
    //   this.router.navigate(['/']);
    // }, err => {
    //   console.log('UN-successful login');
    //   this.invalidLogin = true;
    // });
  }


}


// import { Component, OnInit } from '@angular/core';

// @Component({
//   selector: 'app-login',
//   templateUrl: './login.component.html',
//   styleUrls: ['./login.component.css']
// })
// export class LoginComponent implements OnInit {

//   constructor() { }

//   ngOnInit() {
//   }

// }
