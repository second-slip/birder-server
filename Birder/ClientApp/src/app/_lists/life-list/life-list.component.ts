import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { LifeListViewModel } from '@app/_models/LifeListViewModels';
import { ObservationsAnalysisService } from '@app/_services/observations-analysis.service';
import { Observable, throwError } from 'rxjs';
import { catchError, share } from 'rxjs/operators';

@Component({
  selector: 'app-life-list',
  templateUrl: './life-list.component.html',
  styleUrls: ['./life-list.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LifeListComponent {
  //username: string;
  // lifeList: LifeListViewModel[];
  lifeList$: Observable<LifeListViewModel[]>;
  requesting: boolean;
  page: number;
  pageSize = 10;

  constructor(private observationsAnalysisService: ObservationsAnalysisService
    , private route: ActivatedRoute) {
    route.params.subscribe(_ => {
      this.route.paramMap.subscribe(params => {
        this.getData(params.get('username'))
        //this.username = pmap.get('username');
      })
    });
  }

  getData(username: string) {

    this.lifeList$ = this.observationsAnalysisService.getLifeList()
      .pipe(share(),
        catchError(err => {
          //this.errorObject = err;
          return throwError(err);
        }));
  }

  changePage(): void {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
}
