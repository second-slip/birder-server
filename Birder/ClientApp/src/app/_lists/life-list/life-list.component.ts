import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { LifeListViewModel } from '@app/_models/LifeListViewModels';
import { ObservationAnalysisViewModel } from '@app/_models/ObservationAnalysisViewModel';
import { ObservationsAnalysisService } from '@app/_services/observations-analysis.service';
import { Observable, throwError } from 'rxjs';
import { catchError, share, toArray } from 'rxjs/operators';

@Component({
  selector: 'app-life-list',
  templateUrl: './life-list.component.html',
  styleUrls: ['./life-list.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LifeListComponent {
  //username: string;
  //lifeList: LifeListViewModel[];
  lifeList$: Observable<LifeListViewModel[]>;
  // analysis$: Observable<ObservationAnalysisViewModel>;
  requesting: boolean;
  page: number;
  pageSize = 10;

  constructor(private observationsAnalysisService: ObservationsAnalysisService
    , private route: ActivatedRoute) {
    route.params.subscribe(_ => {
      this.route.paramMap.subscribe(params => {
        this.getData(params.get('username'))
        //this.username = pmap.get('username');

        // this.getLifeList();
        // this.getObservationAnalysis();
      })
      // the next two statements reset the tabs.  This is required when the page is reloaded
      // with different data.  Otherwise the 'sightings' child component keeps its original data.
      // this.active = 1;
      // this.tabstatus = {};
    });
  }

  getData(username: string) {
    //this.getLifeList();
    // this.getObservationAnalysis();

    this.lifeList$ = this.observationsAnalysisService.getLifeList()
    .pipe(share()),
    catchError(err => {
      //this.errorObject = err;
      return throwError(err);
    });

    // this.analysis$ = this.observationsAnalysisService.getObservationAnalysis1(username)
    // .pipe(share()),
    // catchError(err => {
    //   //this.errorObject = err;
    //   return throwError(err);
    // });
  }

  // getLifeList(): void {
  //   this.requesting = true;
  //   this.observationsAnalysisService.getLifeList()
  //     .subscribe(
  //       (data: LifeListViewModel[]) => {
  //         this.lifeList = data;
  //         this.page = 1;
  //       },
  //       (error: ErrorReportViewModel) => {
  //         console.log(error);
  //         // ToDo: Something with the error (perhaps show a message)
  //       },
  //       () => {
  //         this.requesting = false;
  //       }
  //     );
  // }

  // getObservationAnalysis(): void {
  //   this.observationsAnalysisService.getObservationAnalysis(this.username)
  //     .subscribe(
  //       (data: ObservationAnalysisViewModel) => {
  //         this.analysis = data;
  //       },
  //       (error: ErrorReportViewModel) => {
  //         console.log(error);
  //         // ToDo: Something with the error (perhaps show a message)
  //       }
  //     );
  // }

  changePage(): void {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
}
