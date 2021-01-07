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
  title: string;

  constructor(private observationService: ObservationService) { }
  currentFilter: string;

  ngOnInit() { 
    //this.getObservation();
    this.currentFilter = '0';
    this.setTitle();
    this.selectedVal = '0';
  }

  public selectedVal: string;

  
  
  public onValChange(val: string) {
    this.selectedVal = val;
  }




  onFilterFeed(): void {
    //alert(this.currentFilter);
    this.setTitle();
  }

  setTitle(): void {

    if (this.currentFilter == '1') {
      this.title = 'Your observations only';
      return;
    } if (this.currentFilter == '2') {
      this.title = 'All public observations';
      return;
    } else {
      this.title = 'Observations in your network';
      return;
    }
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
