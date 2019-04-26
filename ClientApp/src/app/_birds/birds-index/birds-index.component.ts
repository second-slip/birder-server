import { Component, OnInit } from '@angular/core';
import { BirdsService } from '../../birds.service';
import { Router } from '@angular/router';
import { ErrorReportViewModel } from '../../../_models/ErrorReportViewModel';
import { BirdSummaryViewModel } from '../../../_models/BirdSummaryViewModel';
import { PagedResult } from '../../../_models/PagedResult';
import { PageEvent } from '@angular/material';

@Component({
  selector: 'app-birds-index',
  templateUrl: './birds-index.component.html',
  styleUrls: ['./birds-index.component.scss']
})
export class BirdsIndexComponent implements OnInit {
  birds: PagedResult<BirdSummaryViewModel>;
  pageEvent: PageEvent;
  gridView: boolean;

  constructor(private birdsService: BirdsService
    , private router: Router) { }

  ngOnInit() {
    this.getBirds();
  }

  public handlePage(e: any) {
    console.log(e);
    console.log('page index: ' + e.pageIndex);
    console.log('page index: ' + e.pageSize);
  }

  getBirds(): void {
    this.birdsService.getPagedBirds()
      .subscribe(
        (data: PagedResult<BirdSummaryViewModel>) => { this.birds = data; },
        (error: ErrorReportViewModel) => {
          this.router.navigate(['/page-not-found']);
        });
  }
}

