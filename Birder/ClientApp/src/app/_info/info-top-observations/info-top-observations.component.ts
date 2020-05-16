import { Component, OnInit, ViewEncapsulation, OnDestroy } from '@angular/core';
import { ObservationService } from '../../_services/observation.service';
import { ObservationsAnalysisService } from '../../_services/observations-analysis.service';
import { Subscription } from 'rxjs';
import { TopObservationsAnalysisViewModel } from '../../_models/ObservationAnalysisViewModel';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';

@Component({
  selector: 'app-info-top-observations',
  templateUrl: './info-top-observations.component.html',
  styleUrls: ['./info-top-observations.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class InfoTopObservationsComponent implements OnInit, OnDestroy {
  analysis: TopObservationsAnalysisViewModel;
  observationsChangeSubscription: Subscription;
  requesting: boolean;
  active;

  constructor(private observationService: ObservationService
    , private observationsAnalysisService: ObservationsAnalysisService) { }

  ngOnInit() {
    this.getTopObservationsAnalysis();
    this.observationsChangeSubscription = this.observationService.observationsChanged$
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
    this.observationsAnalysisService.getTopObservationsAnalysis()
      .subscribe(
        (data: TopObservationsAnalysisViewModel) => {
          this.analysis = data;
          if (this.analysis.topMonthlyObservations.length === 0) {
            this.active = 2;
          }
        },
        (error: ErrorReportViewModel) => {
          console.log(error);
          // ToDo: Something with the error (perhaps show a message)
        }
      );
  }
}
