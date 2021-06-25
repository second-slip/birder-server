import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { ContactFormModel } from './ContactFormModel';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class ContactFormService {

  constructor(private http: HttpClient) { }

  postMessage(model: ContactFormModel): Observable<ContactFormModel> {
    return this.http.post<ContactFormModel>('api/Message/SendContactMessage', model, httpOptions)
    .pipe(first());
  }
}
