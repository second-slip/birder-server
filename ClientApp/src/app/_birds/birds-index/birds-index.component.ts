import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { BirdsService } from '../../birds.service';
import { Router } from '@angular/router';
import { ErrorReportViewModel } from '../../../_models/ErrorReportViewModel';
import { BirdSummaryViewModel } from '../../../_models/BirdSummaryViewModel';
import { PageEvent } from '@angular/material';
import { BirderStatus } from '../../../_models/BirdIndexOptions';

@Component({
  selector: 'app-birds-index',
  templateUrl: './birds-index.component.html',
  styleUrls: ['./birds-index.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class BirdsIndexComponent implements OnInit {
  birds: BirdSummaryViewModel[];
  pageEvent: PageEvent;
  gridView: boolean;

  constructor(private birdsService: BirdsService
    , private router: Router) { }

  ngOnInit() {
    this.getBirds(BirderStatus.Common);
  }

  public handlePage(e: any) {
    console.log(e);
    // this.getBirds(e.pageIndex + 1, e.pageSize);
  }

  getBirds(filter: BirderStatus): void {
    this.birdsService.getBirds(filter)
      .subscribe(
        (data: BirdSummaryViewModel[]) => { this.birds = data; },
        (error: ErrorReportViewModel) => {
          this.router.navigate(['/page-not-found']);
        });
  }
}

