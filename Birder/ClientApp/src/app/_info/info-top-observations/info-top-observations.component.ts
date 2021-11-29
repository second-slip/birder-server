import { Component, ViewEncapsulation, OnDestroy } from "@angular/core";
import { TopObservationsAnalysisViewModel } from "@app/_models/ObservationAnalysisViewModel";
import { ObservationsAnalysisService } from "@app/_services/observations-analysis.service";
import { ObservationService } from "@app/_observations/observation.service";
import { Observable, Subscription, throwError } from "rxjs";
import { catchError, share, tap } from "rxjs/operators";
import { TokenService } from "@app/_services/token.service";

@Component({
  selector: 'app-info-top-observations',
  templateUrl: './info-top-observations.component.html',
  styleUrls: ['./info-top-observations.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class InfoTopObservationsComponent implements OnDestroy {
  analysis$: Observable<TopObservationsAnalysisViewModel>;
  observationsChangeSubscription: Subscription;
  public errorObject = null;
  active;

  constructor(private readonly _observationService: ObservationService
    , private readonly _observationsAnalysisService: ObservationsAnalysisService
    , private readonly _tokenService: TokenService) {

    this.getTopObservationsAnalysis();
    this.observationsChangeSubscription = this._observationService.observationsChanged$
      .subscribe(_ => {
        this.onObservationsChanged();
      });
  }

  ngOnDestroy() {
    this.observationsChangeSubscription.unsubscribe();
  }

  onObservationsChanged(): void {
    this.getTopObservationsAnalysis();
  }

  getTopObservationsAnalysis(): void {
    this.analysis$ = this._observationsAnalysisService.getTopObservationsAnalysis()
      .pipe(share(),
        tap(res => this.setActiveTab(res.topMonthlyObservations.length)),
        catchError(err => {
          this.errorObject = err;
          return throwError(err);
        })
      );
  }

  setActiveTab(qtyThisMonth: number): void {
    if (qtyThisMonth) {
      this.active = 1;
    } else {
      this.active = 2;
    }
  }
}
