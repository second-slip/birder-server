import { Component, ViewEncapsulation, OnInit, OnDestroy } from "@angular/core";
import { ErrorReportViewModel } from "@app/_models/ErrorReportViewModel";
import { TopObservationsAnalysisViewModel } from "@app/_models/ObservationAnalysisViewModel";
import { ObservationsAnalysisService } from "@app/_services/observations-analysis.service";
import { ObservationService } from "@app/_sharedServices/observation.service";
import { Subscription } from "rxjs";

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
    this.requesting = true;
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
        },
        () => this.requesting = false
      );
  }
}
