import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { BirdsService } from '../../birds.service';
import { Router } from '@angular/router';
import { ErrorReportViewModel } from '../../../_models/ErrorReportViewModel';
import { BirdSummaryViewModel } from '../../../_models/BirdSummaryViewModel';
import { PagedResult } from '../../../_models/PagedResult';
import { PageEvent } from '@angular/material';
import { BirdIndexOptions } from '../../../_models/BirdIndexOptions';

@Component({
  selector: 'app-birds-index',
  templateUrl: './birds-index.component.html',
  styleUrls: ['./birds-index.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class BirdsIndexComponent implements OnInit {
  birds: PagedResult<BirdSummaryViewModel>;
  pageEvent: PageEvent;
  gridView: boolean;

  constructor(private birdsService: BirdsService
    , private router: Router) { }

  ngOnInit() {
    const x = <BirdIndexOptions> {
      pageIndex: 1,
      pageSize: 5
    };
    this.getBirds(x);
  }

  public handlePage(e: any) {
    console.log(e);
    const x = <BirdIndexOptions> {
      pageIndex: e.pageIndex,
      pageSize: e.pageSize
    };
    this.getBirds(x);
  }

  getBirds(model: BirdIndexOptions): void {
    this.birdsService.getPagedBirds(model)
      .subscribe(
        (data: PagedResult<BirdSummaryViewModel>) => { this.birds = data; },
        (error: ErrorReportViewModel) => {
          this.router.navigate(['/page-not-found']);
        });
  }
}

