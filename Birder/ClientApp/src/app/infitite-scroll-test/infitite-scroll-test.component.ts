import { Component, ViewEncapsulation } from '@angular/core';
import { tap, map, filter, debounceTime, distinct, flatMap } from 'rxjs/operators';
import { BehaviorSubject, fromEvent, merge, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import * as _ from 'lodash';
import { ObservationFeedDto } from '@app/_models/ObservationFeedDto';

@Component({
  selector: 'app-infitite-scroll-test',
  templateUrl: './infitite-scroll-test.component.html',
  styleUrls: ['./infitite-scroll-test.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class InfititeScrollTestComponent {
  private x = false;
  private cache = [];
  private pageByManual$ = new BehaviorSubject(1);
  private itemHeight = 145;
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

  itemResults$: Observable<ObservationFeedDto> = this.pageToLoad$ // itemResults$: ObservationFeedDto
    .pipe(
      tap(_ => this.loading = true),

      flatMap((page: number) => {
        // check max page reached?
        // if (!this.x) {
        // doesn't seem necessary as 'white space' check is adequate

        return this.httpClient.get(`api/ObservationFeed/Test?page=${page}`) //    `https://swapi.co/api/people?page=${page}`)
          .pipe(
            tap((resp: ObservationFeedDto) => {
              // this.n = resp.totalItems;
              if (page === Math.ceil(<number>resp.totalItems / <number>this.numberOfItems)) { this.x = true; }
            }),
            map((resp: ObservationFeedDto) => resp.items), // resp.results),
            tap(resp => {
              this.cache[page - 1] = resp;
              if ((this.itemHeight * this.numberOfItems * page) < window.innerHeight) {
                this.pageByManual$.next(page + 1);
              }
            })
          );
        // }
      }),
      map(() => _.flatMap(this.cache))
    );


  constructor(private httpClient: HttpClient) {
  }
}
