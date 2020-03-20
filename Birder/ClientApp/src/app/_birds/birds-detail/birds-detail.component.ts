import { Component, ViewEncapsulation, ViewChild } from '@angular/core';
import { BirdsService } from '../../_services/birds.service';
import { BirdDetailViewModel } from '../../_models/BirdDetailViewModel';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';
import { ObservationViewModel } from '../../_models/ObservationViewModel';


@Component({
  selector: 'app-birds-detail',
  templateUrl: './birds-detail.component.html',
  styleUrls: ['./birds-detail.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class BirdsDetailComponent {
  bird: BirdDetailViewModel;
  // observations: ObservationViewModel[];
  expanded: boolean;
  // executed: boolean;
  // displayedColumns: string[] = ['quantity', 'bird', 'user', 'date'];
  // dataSource: MatTableDataSource<ObservationViewModel>;
  // @ViewChild(MatPaginator) paginator: MatPaginator;
  // @ViewChild(MatSort) sort: MatSort;

  constructor(private birdsService: BirdsService
    , private route: ActivatedRoute
    , private location: Location
    , private router: Router) {
    route.params.subscribe(_ => {
      this.getBird();
      // if (this.dataSource != null) {
      //   this.expanded = false;
      //   // this.executed = false;
      //   this.dataSource = new MatTableDataSource();
      //   // this.dataSource = null;anded
      // }
    });
  }

  // _lazyContent: string;
  // lazyLoadObservations() {
  //   if (!this.expanded) {
  //     // console.warn();
  //     this.expanded = true;
  //     this.getObservations(this.bird.birdId);
  //   }
  //   return;
  // }

  // getObservations(birdId: number) {
  //   this.birdsService.getObservations(birdId)
  //     .subscribe(
  //       (data: ObservationViewModel[]) => {
  //         // this.observations = data;
  //         this.dataSource = new MatTableDataSource(data);
  //       },
  //       (error: ErrorReportViewModel) => {
  //         console.log('bad request');
  //         this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
  //       },
  //       () => {
  //         // operations when URL request is completed
  //         this.dataSource.paginator = this.paginator;
  //         this.dataSource.sort = this.sort;
  //       });
  //   // );
  // }

  getBird(): void {
    const id = +this.route.snapshot.paramMap.get('id');

    this.birdsService.getBird(id)
      .subscribe(
        (data: BirdDetailViewModel) => {
          this.bird = data;
          // this.executed = false;
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
