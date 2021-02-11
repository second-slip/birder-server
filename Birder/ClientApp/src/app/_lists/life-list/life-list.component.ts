import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ObservationsAnalysisService } from '../../_services/observations-analysis.service';
import { LifeListViewModel } from '../../_models/LifeListViewModels';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';
import { ObservationAnalysisViewModel } from '../../_models/ObservationAnalysisViewModel';

@Component({
  selector: 'app-life-list',
  templateUrl: './life-list.component.html',
  styleUrls: ['./life-list.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LifeListComponent implements OnInit {
  lifeList: LifeListViewModel[];
  analysis: ObservationAnalysisViewModel;

  constructor(private observationsAnalysisService: ObservationsAnalysisService) { }

  ngOnInit() {
    this.getLifeList();
    this.getObservationAnalysis();
  }

  getLifeList(): void {
    this.observationsAnalysisService.getLifeList()
      .subscribe(
        (data: LifeListViewModel[]) => {
          this.lifeList = data;
        },
        (error: ErrorReportViewModel) => {
          console.log(error);
          // ToDo: Something with the error (perhaps show a message)
        }
      );
  }

  getObservationAnalysis(): void {
    this.observationsAnalysisService.getObservationAnalysis()
      .subscribe(
        (data: ObservationAnalysisViewModel) => {
          this.analysis = data;
        },
        (error: ErrorReportViewModel) => {
          console.log(error);
          // ToDo: Something with the error (perhaps show a message)
        }
      );
  }
}
