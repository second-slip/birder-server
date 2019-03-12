import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';
import { Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorHandlerService {

  constructor() { }

  handleHttpError(error: HttpErrorResponse): Observable<ErrorReportViewModel> {
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
