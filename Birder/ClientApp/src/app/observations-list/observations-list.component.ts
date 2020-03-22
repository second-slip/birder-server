import { Component, OnInit, Input } from '@angular/core';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { BirdsService } from '@app/_services/birds.service';

@Component({
  selector: 'app-observations-list',
  templateUrl: './observations-list.component.html',
  styleUrls: ['./observations-list.component.scss']
})
export class ObservationsListComponent implements OnInit {
  // birdId: number;
  isActived = false;
  observations: ObservationViewModel[];
  @Input() birdId: number;

  constructor(private birdsService: BirdsService) { }

  ngOnInit(): void {
    // alert(this.isActived);
    // alert(this.observations);
    if (!this.observations) {

      this.loadObservations(this.birdId);

      // this.observations = new Obser {observationId = 1}
      // alert(this.observations.length);
    }
    // alert(this.observations.length);
  }

  loadObservations(birdId: number): void {
        this.birdsService.getObservations(birdId)
      .subscribe(
        (data: ObservationViewModel[]) => {
          this.observations = data;
          // this.dataSource = new MatTableDataSource(data);
        },
        (error: ErrorReportViewModel) => {
          console.log('bad request');
          // this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
        },
        () => {
          // operations when URL request is completed
          // this.dataSource.paginator = this.paginator;
          // this.dataSource.sort = this.sort;
        });
    // );


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