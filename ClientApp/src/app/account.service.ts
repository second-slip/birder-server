import { Injectable } from '@angular/core';
import { RegisterViewModel } from '../_models/RegisterViewModel';
import { HttpHeaders, HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { map, catchError, delay } from 'rxjs/operators';
import { Observable, throwError, of } from 'rxjs';
import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

const ALTER_EGOS = ['Tenko'];

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private http: HttpClient) { }

  register(viewModel: RegisterViewModel): Observable<void | ErrorReportViewModel> {
    return this.http.post<void>('api/Account/Register', viewModel, httpOptions)
    .pipe(
      catchError(err => this.handleHttpError(err))
    );
  }

  isAlterEgoTaken(alterEgo: string): Observable<boolean> {
    const isTaken = ALTER_EGOS.includes(alterEgo);
    console.log(isTaken);

    return of(isTaken).pipe(delay(400));
  }



  checkValidUsername(userName: string): Observable<boolean | ErrorReportViewModel> {
    userName = userName.trim();

    const options = userName ?
    { params: new HttpParams().set('userName', userName) } : {};

    return this.http.get<boolean>('api/Account/CheckUserName', options)
    .pipe(
      catchError(err => this.handleHttpError(err))
    );
  }

  private handleHttpError(error: HttpErrorResponse): Observable<ErrorReportViewModel> {
    const errorReport = new ErrorReportViewModel();

    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      errorReport.type = 'client-side or network error occurred';
      errorReport.errorNumber = error.status;
      errorReport.message = error.statusText;
      errorReport.friendlyMessage = 'An error occurred retrieving data.';
      // console.log(errorReport);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong
      if (error.status === 400) {
        // Bad request (modelstate not valid) with server modelstate errors
        const validationErrorDictionary = error.error;
        for (const fieldName in validationErrorDictionary) {
          if (validationErrorDictionary.hasOwnProperty(fieldName)) {
            errorReport.modelStateErrors.push(validationErrorDictionary[fieldName]);
          }
        }
      }
      errorReport.type = 'unsuccessful response code';
      errorReport.errorNumber = error.status;
      errorReport.message = error.statusText;
      errorReport.friendlyMessage = 'An error occurred retrieving data.';
      // console.log(errorReport);
    }

    // return an observable with a user-facing error object
    return throwError(errorReport);
  }


}
