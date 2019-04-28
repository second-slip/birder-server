import { Component, ViewEncapsulation } from '@angular/core';
import { BirdsService } from '../../birds.service';
import { BirdDetailViewModel } from '../../../_models/BirdDetailViewModel';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { ErrorReportViewModel } from '../../../_models/ErrorReportViewModel';
import { ObservationViewModel } from '../../../_models/ObservationViewModel';

@Component({
  selector: 'app-birds-detail',
  templateUrl: './birds-detail.component.html',
  styleUrls: ['./birds-detail.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class BirdsDetailComponent {
  bird: BirdDetailViewModel;
  observations: ObservationViewModel[]; // lazy load

  constructor(private birdsService: BirdsService
    , private route: ActivatedRoute
    , private location: Location
    , private router: Router) {
      route.params.subscribe(val => {
        this.getBird();
      });
    }

    // _lazyContent: string;
  get lazyObservations() {
    if (!this.observations) {
      this.getObservations();
    }
    return this.observations;
  }

  getObservations() {
    alert('hello');
    this.birdsService.getObservations(this.bird.birdId)
    .subscribe(
      (data: ObservationViewModel[]) => {
        this.observations = data;
      },
      (error: ErrorReportViewModel) => {
        console.log('bad request');
        this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
      });
  }

  getBird(): void {
    const id = +this.route.snapshot.paramMap.get('id');

    this.birdsService.getBird(id)
      .subscribe(
        (data: BirdDetailViewModel) => {
          this.bird = data;
        },
        (error: ErrorReportViewModel) => {
          console.log('bad request');
          this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
        });
  }

  goBack(): void {
    this.location.back();
  }
}
