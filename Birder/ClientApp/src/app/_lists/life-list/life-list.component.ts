import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { LifeListViewModel } from '@app/_models/LifeListViewModels';
import { ObservationAnalysisViewModel } from '@app/_models/ObservationAnalysisViewModel';
import { ObservationsAnalysisService } from '@app/_services/observations-analysis.service';


@Component({
  selector: 'app-life-list',
  templateUrl: './life-list.component.html',
  styleUrls: ['./life-list.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LifeListComponent implements OnInit {
  lifeList: LifeListViewModel[];
  analysis: ObservationAnalysisViewModel;
  requesting: boolean;
  page: number;
  pageSize = 10;

  constructor(private observationsAnalysisService: ObservationsAnalysisService) { }

  ngOnInit() {
    this.getLifeList();
    this.getObservationAnalysis();
  }

  getLifeList(): void {
    this.requesting = true;
    this.observationsAnalysisService.getLifeList()
      .subscribe(
        (data: LifeListViewModel[]) => {
          this.lifeList = data;
          this.page = 1;
        },
        (error: ErrorReportViewModel) => {
          console.log(error);
          // ToDo: Something with the error (perhaps show a message)
        },
        () => {
          this.requesting = false;
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

  changePage(): void {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
}
