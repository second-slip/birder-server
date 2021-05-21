/*
No point implmeneting this page until 2022.  All observations will be in 2021 until then


import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { LifeListViewModel } from '@app/_models/LifeListViewModels';
import { ObservationAnalysisViewModel } from '@app/_models/ObservationAnalysisViewModel';
import { ObservationsAnalysisService } from '@app/_services/observations-analysis.service';

@Component({
  selector: 'app-year-list',
  templateUrl: './year-list.component.html',
  styleUrls: ['./year-list.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class YearListComponent implements OnInit {
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
        (error: any) => {
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
        (error: any) => {
          console.log(error);
          // ToDo: Something with the error (perhaps show a message)
        }
      );
  }
}

*/
