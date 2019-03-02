import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {

  constructor() { }

  addToken(token: any) {
    localStorage.setItem('jwt', token);
  }

  removeToken() {
    localStorage.removeItem('jwt');
  }

}
