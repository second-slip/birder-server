import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { tap, map, filter, debounceTime, distinct, flatMap, switchMap, catchError } from 'rxjs/operators';
import { BehaviorSubject, fromEvent, merge, Observable, throwError } from 'rxjs';
import { ObservationFeedDto, ObservationFeedPagedDto } from '@app/_models/ObservationFeedDto';
import { ObservationsFeedService } from '@app/_services/observations-feed.service';
import { ObservationFeedFilter } from '@app/_models/ObservationFeedFilter';
import * as _ from 'lodash';
import { ToastrService } from 'ngx-toastr';
import { UserViewModel } from '@app/_models/UserViewModel';
import { TokenService } from '@app/_services/token.service';

@Component({
  selector: 'app-observation-feed',
  templateUrl: './observation-feed.component.html',
  styleUrls: ['./observation-feed.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ObservationFeedComponent implements OnInit {
  user: UserViewModel;
  currentFilter: string;
  title: string;
  public errorObject = null;
  // loadingObs: boolean;
  allLoaded = false;
  private cache = [];
  private pageByManual$ = new BehaviorSubject(1);
  private itemHeight = 165;
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

  itemResults$: Observable<ObservationFeedDto[]> = this.pageToLoad$

    .pipe(
      tap(_ => this.loading = true),
      flatMap((page: number) => {
        return this.observationsFeedService.getObservationsFeed(page, this.currentFilter)
          .pipe(
            tap((resp: ObservationFeedPagedDto) => {
              if (page === Math.ceil(<number>resp.totalItems / <number>this.numberOfItems)) { this.allLoaded = true; }
              if (this.currentFilter != resp.returnFilter) {
                this.toast.info(this.getMessage(this.currentFilter, resp.returnFilter), `No items available`);
                this.currentFilter = resp.returnFilter.toString();
              }
              //this.currentFilter = resp.returnFilter.toString();
              this.setTitle();
            }),
            map((resp: any) => resp.items), // resp.results),
            tap(resp => {
              this.cache[page - 1] = resp;
              if ((this.itemHeight * this.numberOfItems * page) < window.innerHeight) {
                this.pageByManual$.next(page + 1);
              }
            }),
            catchError(err => {
              this.errorObject = err;
              return throwError(err);
            }))
      }),
      map(() => _.flatMap(this.cache))
    );


  constructor(private observationsFeedService: ObservationsFeedService
    , private toast: ToastrService
    , private tokenService: TokenService) { }

  ngOnInit() {
    this.user = this.tokenService.getAuthenticatedUserDetails();
    this.currentFilter = '0';
    this.setTitle();
  }

  setTitle(): void {
    if (this.currentFilter == '1') {
      this.title = 'Your observations';
      return;
    } if (this.currentFilter == '2') {
      this.title = 'All public observations';
      return;
    } else {
      this.title = 'Observations in your network';
      return;
    }
  }

  getMessage(requested: string, returned: string): string {
    let message = '';
    if (requested === '0') { message = message + `There are no observations in your ${ObservationFeedFilter[requested]}.  `; }
    if (requested === '1') { message = message + `You have not recorded any observations yet.  `; }
    message = message + `Your feed is showing the latest ${ObservationFeedFilter[returned]} observations instead...`;
    return message;
  }

  onFilterFeed(filter: string): void {
    this.currentFilter = filter;
    this.cache = [];
    this.allLoaded = false;

    this.itemResults$ = this.pageToLoad$
      .pipe(
        tap(_ => this.loading = true),
        switchMap((page: number) => {

          return this.observationsFeedService.getObservationsFeed(page, this.currentFilter)
            .pipe(
              tap((resp: ObservationFeedPagedDto) => {
                if (page === Math.ceil(<number>resp.totalItems / <number>this.numberOfItems)) { this.allLoaded = true; }
                if (this.currentFilter != resp.returnFilter) {
                  this.toast.info(this.getMessage(this.currentFilter, resp.returnFilter), `No items available`);
                  this.currentFilter = resp.returnFilter.toString();
                }
                //this.currentFilter = resp.returnFilter.toString();
                this.setTitle();
              }),
              map((resp: any) => resp.items),
              tap(resp => {
                this.cache[page - 1] = resp;
                if ((this.itemHeight * this.numberOfItems * page) < window.innerHeight) {
                  this.pageByManual$.next(page + 1);
                }
              }),
              catchError(err => {
                this.errorObject = err;
                return throwError(err);
              }))
        }),
        map(() => _.flatMap(this.cache))
      );
  }
}
