import { ChangeDetectorRef, Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { ObservationFeedFilter } from '@app/_models/ObservationFeedFilter';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { ObservationService } from '@app/_sharedServices/observation.service';

@Component({
  selector: 'app-testing',
  templateUrl: './testing.component.html',
  styleUrls: ['./testing.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TestingComponent implements OnInit {
  observation: ObservationViewModel;


  constructor(private observationService: ObservationService) { }


  ngOnInit() { 
    //this.getObservation();

  }











  getObservation(): void {
    // const id = +this.route.snapshot.paramMap.get('id');

    this.observationService.getObservation(1)
      .subscribe(
        (observation: ObservationViewModel) => {
          this.observation = observation;

        });

  }

}
