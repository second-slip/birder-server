import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ObservationService } from '../observation.service';
import { ObservationsAnalysisService } from '../observations-analysis.service';
import { Subscription } from 'rxjs';
import { TopObservationsAnalysisViewModel } from '../../_models/ObservationAnalysisViewModel';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';

@Component({
  selector: 'app-info-top-observations',
  templateUrl: './info-top-observations.component.html',
  styleUrls: ['./info-top-observations.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class InfoTopObservationsComponent implements OnInit {
  analysis: TopObservationsAnalysisViewModel;
  subscription: Subscription;

  constructor(private observationService: ObservationService
            , private observationsAnalysisService: ObservationsAnalysisService) { }

  ngOnInit() {
    this.getTopObservationsAnalysis();
    this.subscription = this.observationService.observationsChanged$
      .subscribe(_ => {
        this.onObservationsChanged();
      });
    // this.getObservationAnalysis();
  }

  onObservationsChanged(): void {
    this.getTopObservationsAnalysis();
  }

  getTopObservationsAnalysis(): void {
    this.observationsAnalysisService.getTopObservationsAnalysis()
      .subscribe(
        (data: TopObservationsAnalysisViewModel) => {
          this.analysis = data;
        },
        (error: ErrorReportViewModel) => {
          console.log(error);
          // ToDo: Something with the error (perhaps show a message)
        }
      );
  }

}
