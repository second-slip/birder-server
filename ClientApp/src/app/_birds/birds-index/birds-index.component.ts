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
    this.getBirds(0, 0);
  }

  public handlePage(e: any) {
    console.log(e);
    this.getBirds(e.pageIndex, e.pageSize);
  }

  getBirds(pageIndex: number, pageSize: number): void {
    this.birdsService.getPagedBirds(pageIndex, pageSize)
      .subscribe(
        (data: PagedResult<BirdSummaryViewModel>) => { this.birds = data; },
        (error: ErrorReportViewModel) => {
          this.router.navigate(['/page-not-found']);
        });
  }
}

