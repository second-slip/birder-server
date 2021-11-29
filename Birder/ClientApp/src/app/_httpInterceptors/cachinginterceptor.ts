import { Injectable } from '@angular/core';
import {
  HttpEvent, HttpRequest, HttpResponse,
  HttpInterceptor, HttpHandler
} from '@angular/common/http';

import { Observable, of } from 'rxjs';
import { startWith, tap } from 'rxjs/operators';
import { RequestCache } from './request-cache.service';


/**
 * If request is cachable (e.g., package search) and
 * response is in cache return the cached response as observable.
 * If has 'x-refresh' header that is true,
 * then also re-run the package search, using response from next(),
 * returning an observable that emits the cached response first.
 *
 * If not in cache or not cachable,
 * pass request through to next()
 */
@Injectable()
export class CachingInterceptor implements HttpInterceptor {
  constructor(private cache: RequestCache) {}

  intercept(request: HttpRequest<any>, next: HttpHandler) {
    // continue if not cachable.
    if (!isCachable(request)) { return next.handle(request); }

    const cachedResponse = this.cache.get(request);
    // cache-then-refresh
    if (request.headers.get('x-refresh')) {
      const results$ = sendRequest(request, next, this.cache);
      return cachedResponse ?
        results$.pipe( startWith(cachedResponse) ) :
        results$;
    }
    // cache-or-fetch
    return cachedResponse ?
      of(cachedResponse) : sendRequest(request, next, this.cache);
  }
}


/** Is this request cachable? */
function isCachable(request: HttpRequest<any>) {
  return request.method === 'GET'
    && request.url.indexOf('api/Test') !== 0
    && request.url.indexOf('api/Photograph') !== 0
    //&& request.url.indexOf('https://preview.ibb.co') !== 0
    && request.url.indexOf('api/ObservationFeed') !== 0
    && request.url.indexOf('api/Observation/GetObservationDetail') !== 0
    && request.url.indexOf('api/ObservationAnalysis') !== 0  // do not cache requests containing 'api/ObservationAnalysis'
    && request.url.indexOf('api/List/GetTopObservationsList') !== 0
    && request.url.indexOf('api/UserProfile') !== 0
    && request.url.indexOf('api/Network') !== 0
    && request.url.indexOf('api/Manage') !== 0
    && request.url.indexOf('api/Account/IsUsernameAvailable') !== 0;
  }

/**
 * Get server response observable by sending request to `next()`.
 * Will add the response to the cache on the way out.
 */
function sendRequest(
  request: HttpRequest<any>,
  next: HttpHandler,
  cache: RequestCache): Observable<HttpEvent<any>> {

  // No headers allowed in npm search request
  // const noHeaderReq = request.clone({ headers: new HttpHeaders() });

  return next.handle(request).pipe(
    tap(event => {
      // There may be other events besides the response.
      if (event instanceof HttpResponse) {
        cache.put(request, event); // Update the cache.
      }
    })
  );
}



/*
Copyright Google LLC. All Rights Reserved.
Use of this source code is governed by an MIT-style license that
can be found in the LICENSE file at http://angular.io/license
*/


// import { Injectable } from '@angular/core';
// import { HttpEvent, HttpRequest, HttpHandler, HttpInterceptor, HttpResponse } from '@angular/common/http';
// import { Observable, of } from 'rxjs';
// import { tap, shareReplay } from 'rxjs/operators';

// @Injectable()
// export class CacheInterceptor implements HttpInterceptor {
//   private cache = new Map<string, any>();

//   intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
//     // if cacheable
//     if (request.method !== 'GET') {
//       return next.handle(request);
//     }

//     console.warn('CacheInterceptor');

//     const cachedResponse = this.cache.get(request.url);
//     if (cachedResponse) {
//       return of(cachedResponse);
//     }

//     return next.handle(request).pipe(
//       tap(event => {
//         if (event instanceof HttpResponse) {
//           this.cache.set(request.url, event);
//           console.log(this.cache);
//         }
//       })
//     );
//   }
// }
