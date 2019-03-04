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

  //   /** GET hero by id. Will 404 if id not found */
  //   getHero(id: number): Observable<Hero> {
  //     const url = `${this.heroesUrl}/${id}`;
  //     return this.http.get<Hero>(url).pipe(
  //       tap(_ => this.log(`fetched hero id=${id}`)),
  //       catchError(this.handleError<Hero>(`getHero id=${id}`))
  //     );
  // }
  getBird(id: number): Observable<Bird> {
    const url = `${this.url}/${id}`;
    alert(url);
    return this.http.get<Bird>(this.url)
      .pipe(
        tap(_ => this.log('fetched bird')),
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
