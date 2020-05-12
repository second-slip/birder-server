import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Subscription } from 'rxjs';
import { ObservationService } from '../../_services/observation.service';
import { ObservationsAnalysisService } from '../../_services/observations-analysis.service';
import { ObservationAnalysisViewModel } from '../../_models/ObservationAnalysisViewModel';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';

@Component({
  selector: 'app-info-observation-count',
  templateUrl: './info-observation-count.component.html',
  styleUrls: ['./info-observation-count.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class InfoObservationCountComponent implements OnInit {
  analysis: ObservationAnalysisViewModel;
  subscription: Subscription;
  requesting: boolean;

  constructor(private observationService: ObservationService
            , private observationsAnalysisService: ObservationsAnalysisService) { }

  ngOnInit() {
    this.getObservationAnalysis();
    this.subscription = this.observationService.observationsChanged$
      .subscribe(_ => {
        this.onObservationsChanged();
      });
    // this.getObservationAnalysis();
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
