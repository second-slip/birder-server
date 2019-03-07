import { Injectable } from '@angular/core';
import { Observation } from '../_models/Observation';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ObservationService {

  constructor(private http: HttpClient) { }

  getObservations(): Observable<Observation[]> {
    return this.http.get<Observation[]>('api/Observation')
      .pipe(
        tap(observations => this.log('fetched observations')));
  }

  /** Log a HeroService message with the MessageService */
  private log(message: string) {
    console.log(message);
    // this.messageService.add(`HeroService: ${message}`);
  }
}
