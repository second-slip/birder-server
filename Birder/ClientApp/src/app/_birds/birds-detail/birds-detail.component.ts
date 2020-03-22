import { Component, ViewEncapsulation } from '@angular/core';
import { BirdsService } from '../../_services/birds.service';
import { BirdDetailViewModel } from '../../_models/BirdDetailViewModel';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';

@Component({
  selector: 'app-birds-detail',
  templateUrl: './birds-detail.component.html',
  styleUrls: ['./birds-detail.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class BirdsDetailComponent {
  bird: BirdDetailViewModel;
  tabstatus = {};

  constructor(private birdsService: BirdsService
    , private route: ActivatedRoute
    , private location: Location
    , private router: Router) {
    route.params.subscribe(_ => {
      this.getBird();
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
