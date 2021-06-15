// import { Injectable } from '@angular/core';
// import { HttpErrorResponse } from '@angular/common/http';
// import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';
// import { Observable, throwError } from 'rxjs';

// @Injectable({
//   providedIn: 'root'
// })
// export class HttpErrorHandlerService {

//   constructor() { }

  // handleAuthenticationError(error: HttpErrorResponse): Observable<ErrorReportViewModel> {
    
  //   const errorReport = new AuthenticationErrorViewModel();
  //   console.log(error);
    
  //   // console.log(error);

  //   if (error.error instanceof ErrorEvent) {
  //     // A client-side or network error occurred. Handle it accordingly.
  //     errorReport.type = 'client-side or network error occurred';
  //     errorReport.errorNumber = error.status;
  //     errorReport.message = error.message;
  //     errorReport.friendlyMessage = 'Please try again later.';
  //   } else {
  //     console.log(error);
  //     errorReport.type = 'unsuccessful response code';
  //     errorReport.errorNumber = error.status;
  //     errorReport.message = error.statusText;
  //     errorReport.friendlyMessage = 'Please try again later.';
  //     //errorReport.failureReason = error.error.failureReason;
  //   }

  //   console.log(errorReport);

  //   return throwError(errorReport);
  // }

  // handleHttpError(error: HttpErrorResponse): Observable<ErrorReportViewModel> {
  //   const errorReport = new ErrorReportViewModel();
  //   console.log(error);

  //   if (error.error instanceof ErrorEvent) {
  //     // A client-side or network error occurred. Handle it accordingly.
  //     errorReport.type = 'client-side or network error occurred';
  //     errorReport.errorNumber = error.status;
  //     errorReport.message = error.message;
  //     errorReport.friendlyMessage = 'A network error occurred.  Please try again';
  //     // console.log(errorReport);
  //   } else {
  //     // The backend returned an unsuccessful response code.
  //     // The response body may contain clues as to what went wrong
  //     if (error.status === 400 && (typeof (error.error) !== 'string')) {
  //       // Bad request (modelstate not valid) with server modelstate errors
  //       errorReport.serverCustomMessage = 'See model state array';

  //       const validationErrorDictionary = error.error;
  //       for (const fieldName in validationErrorDictionary) {
  //         if (validationErrorDictionary.hasOwnProperty(fieldName)) {
  //           errorReport.modelStateErrors.push(validationErrorDictionary[fieldName]);
  //         }
  //       }
  //     } else {
  //       errorReport.serverCustomMessage = error.error;
  //     }
  //     errorReport.type = 'unsuccessful response code';
  //     errorReport.errorNumber = error.status;
  //     errorReport.message = error.statusText;
  //     errorReport.friendlyMessage = 'Please try again later.';
  //     // console.log(errorReport);
  //   }

  //   // return an observable with a user-facing error object
  //   return throwError(errorReport);
  // }
//}
