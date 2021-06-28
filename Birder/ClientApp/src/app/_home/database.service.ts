import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { first, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class DatabaseService {

  constructor(private http: HttpClient) { }

  wakeyWakey(): Observable<boolean> {
    return this.http.get<boolean>("api/Home").pipe(map(x => {return true}));
  }
}
