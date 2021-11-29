import { Component, ViewEncapsulation, OnDestroy, OnInit } from "@angular/core";
import { ObservationTopService } from "../observation-top.service";

@Component({
  selector: 'app-info-top-observations',
  templateUrl: './info-top-observations.component.html',
  styleUrls: ['./info-top-observations.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class InfoTopObservationsComponent implements OnInit {
  active: number = 1;

  constructor(readonly _service: ObservationTopService) { }

  ngOnInit(): void {
    this._getData();
  }

  private _getData(): void {
    this._service.getData();
    //this._setActiveTab();    -----> how to do this?
  }

  public reload(): void {
    this._getData();
  }

  // ??????????
  private _setActiveTab(qtyThisMonth: number): void {
    if (qtyThisMonth) {
      this.active = 1;
    } else {
      this.active = 2;
    }
  }
}

  // ngOnDestroy() {
  //   this.observationsChangeSubscription.unsubscribe();
  // }

  // onObservationsChanged(): void {
  //   this.getTopObservationsAnalysis();
  // }

  // getTopObservationsAnalysis(): void {
  //   this.analysis$ = this._observationsAnalysisService.getTopObservationsAnalysis()
  //     .pipe(share(),
  //       tap(res => this._setActiveTab(res.topMonthlyObservations.length)),
  //       catchError(err => {
  //         //this.errorObject = err;
  //         return throwError(err);
  //       })
  //     );
  // }
//}
