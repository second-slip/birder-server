import { Injectable } from '@angular/core';
import { Observation } from '../_models/Observation';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { Bird } from '../_models/Bird';

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

  getObservation(id: number): Observable<Observation> {
    const url = `api/Observation/GetObservation?id=${id}`;
    return this.http.get<Observation>(url)
      .pipe(
        tap(observation => this.log(`fetched observation with id: ${id}`))
        // ,
        // catchError(this.handleError<Bird>('getBird'))
      );
  }

  // ************TEMPORARY **********
  getBirds(): Observable<Bird[]> {
    return this.http.get<Bird[]>('api/Birds')
      .pipe(
        tap(birds => this.log('fetched birds for ddl')));
      //   ,
      //   catchError(this.handleError('getBirds',  []))
      // );
  }
  // ************TEMPORARY **********

  /** Log a HeroService message with the MessageService */
  private log(message: string) {
    console.log(message);
    // this.messageService.add(`HeroService: ${message}`);
  }
}
