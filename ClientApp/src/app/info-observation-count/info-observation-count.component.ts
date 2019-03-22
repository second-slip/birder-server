import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ObservationService } from '../observation.service';
import { ObservationsAnalysisService } from '../observations-analysis.service';
import { ObservationAnalysisViewModel } from 'src/_models/ObservationAnalysisViewModel';
import { ErrorReportViewModel } from 'src/_models/ErrorReportViewModel';

@Component({
  selector: 'app-info-observation-count',
  templateUrl: './info-observation-count.component.html',
  styleUrls: ['./info-observation-count.component.scss']
})
export class InfoObservationCountComponent implements OnInit {
  analysis: ObservationAnalysisViewModel;
  subscription: Subscription;

  constructor(private observationService: ObservationService
    , private observationsAnalysisService: ObservationsAnalysisService) { }

  ngOnInit() {
    this.subscription = this.observationService.observationsChanged$
      .subscribe(data => {
        this.onObservationsChanged();
      });

    this.getObservationAnalysis();
  }

  onObservationsChanged(): void {
    alert('An observation was added or edited or deleted');
    this.getObservationAnalysis();
  }

  getObservationAnalysis(): void {
    alert('1');
    this.observationsAnalysisService.getObservationAnalysis()
      .subscribe(
        (data: ObservationAnalysisViewModel) => {
          this.analysis = data;
          alert('');
         },
        (error: ErrorReportViewModel) => { }
      );
  }

}
