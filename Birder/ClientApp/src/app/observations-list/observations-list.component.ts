import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { ObservationService } from '@app/_services/observation.service';
import { ObservationDto } from '@app/_models/ObservationFeedDto';

@Component({
  selector: 'app-observations-list',
  templateUrl: './observations-list.component.html',
  styleUrls: ['./observations-list.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ObservationsListComponent implements OnInit {
  observations: ObservationViewModel[];
  @Input() birdId: number;
  totalItems: number;
  page = 1;
  pageSize = 10;

  constructor(private observationsService: ObservationService) { }

  ngOnInit(): void {
    if (!this.observations) {
      this.getObservations(this.birdId, this.page, this.pageSize);
    }
  }

  changePage() { // event) { // page: number) {
    this.getObservations(this.birdId, this.page, this.pageSize);
  }

  getObservations(birdId: number, page: number, pageSize: number): void {
    this.observationsService.getObservationsByBirdSpecies(birdId, page, pageSize)
      .subscribe(
        (data: ObservationDto) => {
          this.totalItems = data.totalItems;
          this.observations = data.items;
        },
        (error: ErrorReportViewModel) => {
          console.log('bad request');
          // this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
        },
        () => {
        });
  }
}



// this.observations = [];
// // this.isActived = true;
// let myObj: ObservationViewModel = { observationId: 1, locationLatitude: 1, locationLongitude: 1,
//   quantity: 1,
//   noteGeneral: 'string',
//   noteHabitat: 'string',
//   noteWeather: 'string',
//   noteAppearance: 'string',
//   noteBehaviour: 'string',
//   noteVocalisation: 'string',
//   hasPhotos: true,
//   // SelectedPrivacyLevel: PrivacyLevel;
//   observationDateTime: 'string',
//   creationDate: 'string',
//   lastUpdateDate: 'string',
//   birdId: 1,
//   bird: null,
//   user: null  };
//   console.log(myObj);
// this.observations.push(myObj);
