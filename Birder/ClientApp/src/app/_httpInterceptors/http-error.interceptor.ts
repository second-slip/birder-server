import {
  HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpResponse, HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';

export class HttpErrorInterceptor implements HttpInterceptor {
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request)
      .pipe(
        retry(3),
        catchError((error: HttpErrorResponse) => {
          let errorMessage = '';
          //console.log(error);
          if (error.error instanceof ErrorEvent) {
            // client-side error
            //console.log('client-side error');
            errorMessage = `Error: ${error.error.message}`;
          } else {
            // server-side error
            //console.log('server-side error');
            if (error.error.failureReason) { // type user authentication error...
              //alert(error.error.failureReason);
              return throwError(error.error.failureReason)
            }

            //console.log(error.error);
            if (error.error.includes('an sql server')) {
              errorMessage = error.error;
            } else {
              errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
            }
            //console.log(error.message);
          }
          // window.alert(errorMessage);
          return throwError(errorMessage);
        })
      )
  }
}



// import { Injectable } from '@angular/core';
// import {
//   HttpInterceptor,
//   HttpRequest,
//   HttpResponse,
//   HttpErrorResponse,
//   HttpHandler,
//   HttpEvent
// } from '@angular/common/http';

// import { Observable } from 'rxjs/Observable';
// import 'rxjs/add/operator/do';
// import 'rxjs/add/operator/catch';
// import 'rxjs/add/observable/throw';
// import 'rxjs/add/observable/of';

// @Injectable()
// export class HttpErrorInterceptor implements HttpInterceptor {
//   intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

//     // remove this if in your application, this is just for testing purposes of this plnkr
//     if (request.url === './error-and-bypass-interceptor.json') {
//       return next.handle(request);
//     }
//     console.log('Intercepting Request:', request);

//     return next.handle(request)
//       .catch((err: HttpErrorResponse) => {

//         if (err.error instanceof Error) {
//           // A client-side or network error occurred. Handle it accordingly.
//           console.error('An error occurred:', err.error.message);
//         } else {
//           // The backend returned an unsuccessful response code.
//           // The response body may contain clues as to what went wrong,
//           console.error(`Backend returned code ${err.status}, body was: ${err.error}`);
//         }

//         // ...optionally return a default fallback value so app can continue (pick one)
//         // which could be a default value
//         return Observable.of(new HttpResponse({body: [
//           {name: 'Default values returned by Interceptor', id: 88},
//           // {name: 'Default values returned by Interceptor(2)', id: 89}
//         ]}));
//         // or simply an empty observable
//         // return Observable.empty<HttpEvent<any>>();
//       });
//   }
// }
