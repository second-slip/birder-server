// import { Component, ViewEncapsulation, OnDestroy } from '@angular/core';
// import { ObservationAnalysisViewModel } from '@app/_models/ObservationAnalysisViewModel';
// import { ObservationsAnalysisService } from '@app/_services/observations-analysis.service';
// import { TokenService } from '@app/_services/token.service';
// import { ObservationService } from '@app/_observations/observation.service';
// import { Observable, Subscription, throwError } from 'rxjs';
// import { catchError, share } from 'rxjs/operators';

// @Component({
//   selector: 'app-info-observation-count',
//   templateUrl: './info-observation-count.component.html',
//   styleUrls: ['./info-observation-count.component.scss'],
//   encapsulation: ViewEncapsulation.None
// })
// export class InfoObservationCountComponent implements OnDestroy {
//   observationsChangeSubscription: Subscription;
//   analysis$: Observable<ObservationAnalysisViewModel>;
//   public errorObject = null;

//   constructor(private observationService: ObservationService
//     , private tokenService: TokenService
//     , private observationsAnalysisService: ObservationsAnalysisService) {
//     this.getObservationAnalysis();
//     this.observationsChangeSubscription = this.observationService.observationsChanged$
//       .subscribe(_ => {
//         this.onObservationsChanged();
//       });
//   }

//   ngOnDestroy() {
//     this.observationsChangeSubscription.unsubscribe();
//   }

//   onObservationsChanged(): void {
//     this.getObservationAnalysis();
//   }

//   getObservationAnalysis(): void {
//     const username = this.tokenService.getUsername();

//     this.analysis$ = this.observationsAnalysisService.getObservationAnalysis(username)
//       .pipe(share(),
//         catchError(err => {
//           this.errorObject = err;
//           return throwError(err);
//         }));
//   }
// }
