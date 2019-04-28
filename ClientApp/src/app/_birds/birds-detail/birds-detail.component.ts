import { Component, ViewEncapsulation, ViewChild } from '@angular/core';
import { BirdsService } from '../../birds.service';
import { BirdDetailViewModel } from '../../../_models/BirdDetailViewModel';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { ErrorReportViewModel } from '../../../_models/ErrorReportViewModel';
import { ObservationViewModel } from '../../../_models/ObservationViewModel';
import { MatTableDataSource, MatPaginator, MatSort } from '@angular/material';

@Component({
  selector: 'app-birds-detail',
  templateUrl: './birds-detail.component.html',
  styleUrls: ['./birds-detail.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class BirdsDetailComponent {
  bird: BirdDetailViewModel;
  // observations: ObservationViewModel[];
  executed = false;
  displayedColumns: string[] = ['quantity', 'bird', 'user'];
  dataSource: MatTableDataSource<ObservationViewModel>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

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

    if (!this.dataSource && !this.executed) {
      this.executed = true;
      this.getObservations();
    }
    return;
  }

  getObservations() {
    this.birdsService.getObservations(this.bird.birdId)
      .subscribe(
        (data: ObservationViewModel[]) => {
          // this.observations = data;
          this.dataSource = new MatTableDataSource(data);
        },
        (error: ErrorReportViewModel) => {
          console.log('bad request');
          this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
        },
        () => {
          // operations when URL request is completed
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
       });
        // );
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
