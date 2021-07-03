import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ContactFormModel } from './ContactFormModel';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class ContactFormService {

  constructor(private readonly _http: HttpClient) { }

  postMessage(model: ContactFormModel): Observable<ContactFormModel> {
    return this._http.post<ContactFormModel>('api/Message/SendContactMessage', model, httpOptions);
  }
}
