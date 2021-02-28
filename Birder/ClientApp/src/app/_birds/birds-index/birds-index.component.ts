import { Component, ViewEncapsulation } from '@angular/core';
import { BirdsDto } from '@app/_models/BirdSummaryViewModel';
import { BirdsService } from '@app/_services/birds.service';
import { catchError, finalize, share } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';

@Component({
  selector: 'app-birds-index',
  templateUrl: './birds-index.component.html',
  styleUrls: ['./birds-index.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class BirdsIndexComponent {
  birds$: Observable<BirdsDto>; // Observable<BirdSummaryViewModel[]>;
  public errorObject = null;
  page = 1;
  pageSize = 30;
  speciesFilter: string = '0';

  constructor(private birdsService: BirdsService) {
    this.getBirds();
  }

  changePage(): void {
    this.getBirds();
  }

  onChangeFilter(filter: string): void {
    this.page = 1;
    this.speciesFilter = filter;
    this.getBirds();
  }

  getBirds(): void {
    this.birds$ = this.birdsService.getBirds(this.page, this.pageSize, this.speciesFilter)
      .pipe(share(),
        finalize(() => window.scrollTo({ top: 0, behavior: 'smooth' })),
        catchError(err => {
          this.errorObject = err;
          return throwError(err);
        }));
  }
}
