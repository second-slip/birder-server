import { Component, OnInit } from '@angular/core';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';

@Component({
  selector: 'app-observations-list',
  templateUrl: './observations-list.component.html',
  styleUrls: ['./observations-list.component.scss']
})
export class ObservationsListComponent implements OnInit {
  isActived = false;
  observations: ObservationViewModel[];

  constructor() { }

  ngOnInit(): void {
    // alert(this.isActived);
    alert(this.observations);
    if (!this.observations) {
      alert('hello');
      this.observations = [];
      // this.isActived = true;
      let myObj: ObservationViewModel = { observationId: 1, locationLatitude: 1, locationLongitude: 1,
        quantity: 1,
        noteGeneral: 'string',
        noteHabitat: 'string',
        noteWeather: 'string',
        noteAppearance: 'string',
        noteBehaviour: 'string',
        noteVocalisation: 'string',
        hasPhotos: true,
        // SelectedPrivacyLevel: PrivacyLevel;
        observationDateTime: 'string',
        creationDate: 'string',
        lastUpdateDate: 'string',
        birdId: 1,
        bird: null,
        user: null  };
        console.log(myObj);
      this.observations.push(myObj);
      // this.observations = new Obser {observationId = 1}
      alert(this.observations.length);
    }
    alert(this.observations.length);
  }

}
