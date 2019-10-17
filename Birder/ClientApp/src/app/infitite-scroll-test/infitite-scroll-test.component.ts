import { Component, OnInit } from '@angular/core';

import { tap, map, filter, debounceTime, distinct, flatMap } from 'rxjs/operators';
import { BehaviorSubject, fromEvent, merge } from 'rxjs';
import { HttpClient } from '@angular/common/http';

import * as _ from 'lodash';

@Component({
  selector: 'app-infitite-scroll-test',
  templateUrl: './infitite-scroll-test.component.html',
  styleUrls: ['./infitite-scroll-test.component.scss']
})
export class InfititeScrollTestComponent {

  private cache = [];
  private pageByManual$ = new BehaviorSubject(1);
  private itemHeight = 40;
  private numberOfItems = 10;
  private pageByScroll$ = fromEvent(window, 'scroll')
    .pipe(
      map(() => window.scrollY),
      filter(current => current >= document.body.clientHeight - window.innerHeight),
      debounceTime(200),
      distinct(),
      map(y => Math.ceil((y + window.innerHeight) / (this.itemHeight * this.numberOfItems)))
    );

  private pageByResize$ = fromEvent(window, 'resize')
    .pipe(
      debounceTime(200),
      map(_ => Math.ceil(
        (window.innerHeight + document.body.scrollTop) / (this.itemHeight * this.numberOfItems)
      ))
    );

  private pageToLoad$ = merge(this.pageByManual$, this.pageByScroll$, this.pageByResize$)
    .pipe(
      distinct(),
      filter(page => this.cache[page - 1] === undefined)
    );

  loading = false;

  itemResults$ = this.pageToLoad$
    .pipe(
      tap(_ => this.loading = true),
      flatMap((page: number) => {
        return this.httpClient.get(`api/ObservationFeed?page=${page}`) //    `https://swapi.co/api/people?page=${page}`)
          .pipe(
            map((resp: any) => resp), // resp.results),
            tap(resp => {
              this.cache[page - 1] = resp;
              if ((this.itemHeight * this.numberOfItems * page) < window.innerHeight) {
                this.pageByManual$.next(page + 1);
              }
            })
          );
      }),
      map(() => _.flatMap(this.cache))
    );

  constructor(private httpClient: HttpClient) {
  }
}
