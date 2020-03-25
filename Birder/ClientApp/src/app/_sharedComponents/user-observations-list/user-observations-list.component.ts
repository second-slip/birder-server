import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { ObservationService } from '@app/_services/observation.service';
import { ObservationDto } from '@app/_models/ObservationFeedDto';

@Component({
  selector: 'app-user-observations-list',
  templateUrl: './user-observations-list.component.html',
  styleUrls: ['./user-observations-list.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class UserObservationsListComponent implements OnInit {
  observations: ObservationViewModel[];
  @Input() username: string;
  totalItems: number;
  page = 1;
  pageSize = 10;

  constructor(private observationsService: ObservationService) { }

  ngOnInit(): void {
    if (!this.observations) {
      this.getObservations(this.username, this.page, this.pageSize);
    }
  }

  changePage(): void { // event) { // page: number) {
    this.getObservations(this.username, this.page, this.pageSize);
  }

  getObservations(username: string, page: number, pageSize: number): void {
    this.observationsService.getObservationsByUser(username, page, pageSize)
      .subscribe(
        (data: ObservationDto) => {
          this.totalItems = data.totalItems;
          this.observations = data.items;
        },
        (error: ErrorReportViewModel) => {
          console.log('bad request');
          // this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
        });
        // () => {
        // });
  }
}
