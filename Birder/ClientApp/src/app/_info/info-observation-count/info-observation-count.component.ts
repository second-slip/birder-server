import { Component, OnInit, ViewEncapsulation, OnDestroy } from '@angular/core';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { ObservationAnalysisViewModel } from '@app/_models/ObservationAnalysisViewModel';
import { ObservationsAnalysisService } from '@app/_services/observations-analysis.service';
import { ObservationService } from '@app/_sharedServices/observation.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-info-observation-count',
  templateUrl: './info-observation-count.component.html',
  styleUrls: ['./info-observation-count.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class InfoObservationCountComponent implements OnInit, OnDestroy {
  observationsChangeSubscription: Subscription;
  analysis: ObservationAnalysisViewModel;
  requesting: boolean;

  constructor(private observationService: ObservationService
            , private observationsAnalysisService: ObservationsAnalysisService) { }

  ngOnInit() {
    this.getObservationAnalysis();
    this.observationsChangeSubscription = this.observationService.observationsChanged$
      .subscribe(_ => {
        this.onObservationsChanged();
      });
    // this.getObservationAnalysis();
  }

  ngOnDestroy() {
    this.observationsChangeSubscription.unsubscribe();
  }

  onObservationsChanged(): void {
    this.getObservationAnalysis();
  }

  getObservationAnalysis(): void {
    this.requesting = true;
    this.observationsAnalysisService.getObservationAnalysis()
      .subscribe(
        (data: ObservationAnalysisViewModel) => {
          this.analysis = data;
          this.requesting = false;
        },
        (error: ErrorReportViewModel) => {
          // console.log(error);
          this.requesting = false;
          // ToDo: Something with the error (perhaps show a message)
        }
      );
  }
}
