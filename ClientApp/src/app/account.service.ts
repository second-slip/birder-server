import { Injectable } from '@angular/core';
import { RegisterViewModel } from '../_models/RegisterViewModel';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private http: HttpClient) { }

  register(viewModel: RegisterViewModel) {
    // TODO: remove console log
    console.log('loginservice - LOGIN');

    return this.http.post<any>('api/Account/Register', viewModel, httpOptions)
      .pipe(map(user => { 
        // Success, redirect to page to confirm by email

        // Fail, based on model state ? -> Report fault to user

        // Fail, some other reason -> Page Not Found

        return user;
      }));
  }
}
