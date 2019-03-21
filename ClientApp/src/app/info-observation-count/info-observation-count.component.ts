import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ObservationService } from '../observation.service';

@Component({
  selector: 'app-info-observation-count',
  templateUrl: './info-observation-count.component.html',
  styleUrls: ['./info-observation-count.component.scss']
})
export class InfoObservationCountComponent implements OnInit {

  subscription: Subscription;

  constructor(private observationService: ObservationService) { }

  ngOnInit() {
    this.subscription = this.observationService.observationsChanged$
    .subscribe(data => {
      this.onObservationsChanged();
    });
  }

  onObservationsChanged(): void {
    alert('An observation was added or edited or deleted');
    // console.log('hello');
    // update observations count...
  }

}
