import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ObservationsAnalysisService } from '../observations-analysis.service';
import { LifeListViewModel } from '../../_models/LifeListViewModels';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';

@Component({
  selector: 'app-life-list',
  templateUrl: './life-list.component.html',
  styleUrls: ['./life-list.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LifeListComponent implements OnInit {
  model: LifeListViewModel;

  constructor(private observationsAnalysisService: ObservationsAnalysisService) { }

  ngOnInit() {
    this.getLifeList();
  }

  getLifeList(): void {
    this.observationsAnalysisService.getLifeList()
      .subscribe(
        (data: LifeListViewModel) => {
          this.model = data;
          console.log(this.model);
        },
        (error: ErrorReportViewModel) => {
          console.log(error);
          // ToDo: Something with the error (perhaps show a message)
        }
      );
  }
}
