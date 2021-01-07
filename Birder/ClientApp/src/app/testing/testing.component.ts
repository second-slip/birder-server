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

  constructor(private observationService: ObservationService, private ref: ChangeDetectorRef) { }
  currentFilter: ObservationFeedFilter = 0;

  ngOnInit() { 
    //this.getObservation();
    this.setTitle();
  }

  onFilterFeed(): void {
    //alert(this.currentFilter);
    this.setTitle();
  }

  setTitle(): void {

    if (this.currentFilter == 1) {
      this.title = 'Showing only your observations';
      return;
    } if (this.currentFilter == 2) {
      this.title = 'Showing all public observations';
      return;
    } else {
      this.title = 'Showing observations in your network';
      return;
    }
  }

  //   let t = ObservationFeedFilter;
  // alert();
  //   switch(this.currentFilter) { 
      
  //     case ObservationFeedFilter.Own: { 
  //       alert();
  //        this.title = 'Your observations';
  //        break; 
  //     } 
  //     case t.Public: { 
  //       this.title = 'Public observations';
  //        break; 
  //     } 
  //     default: { 
  //       this.title = 'Observations in your network';
  //        break; 
  //     } 
  // }
   // this.ref.detectChanges();

 // }


  getObservation(): void {
    // const id = +this.route.snapshot.paramMap.get('id');

    this.observationService.getObservation(1)
      .subscribe(
        (observation: ObservationViewModel) => {
          this.observation = observation;

        });

  }

}
