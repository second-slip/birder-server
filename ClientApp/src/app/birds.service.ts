import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Bird } from '../_models/Bird';
import { tap, catchError } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class BirdsService {

  private url = 'api/Birds';

  constructor(private http: HttpClient) { }

  getBirds(): Observable<Bird[]> {
    return this.http.get<Bird[]>(this.url)
      .pipe(
        tap(birds => this.log('fetched birds')),
        catchError(this.handleError('getBirds',  []))
      );
  }

  getBird(id: number): Observable<Bird> {
    const url = `${this.url}/GetBird?id=${id}`;
    // alert(url);
    return this.http.get<Bird>(url)
      .pipe(
        tap(bird => this.log('fetched bird')),
        catchError(this.handleError<Bird>('getBird'))
      );
  }

  /**
* Handle Http operation that failed.
* Let the app continue.
* @param operation - name of the operation that failed
* @param result - optional value to return as the observable result
*/
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

  /** Log a HeroService message with the MessageService */
  private log(message: string) {
    console.log(message);
    // this.messageService.add(`HeroService: ${message}`);
  }
}
