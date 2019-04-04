import { Component, OnInit } from '@angular/core';
import { BirdsService } from '../birds.service';
import { Router } from '@angular/router';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';
import { BirdSummaryViewModel } from '../../_models/BirdSummaryViewModel';

@Component({
  selector: 'app-birds-index',
  templateUrl: './birds-index.component.html',
  styleUrls: ['./birds-index.component.scss']
})
export class BirdsIndexComponent implements OnInit {
  birds: BirdSummaryViewModel[];
  gridView: boolean;

  constructor(private birdsService: BirdsService
    , private router: Router) { }

  ngOnInit() {
    this.getBirds();
  }

  getBirds(): void {
    this.birdsService.getBirds()
      .subscribe(
        (data: BirdSummaryViewModel[]) => { this.birds = data; },
        (error: ErrorReportViewModel) => {
          this.router.navigate(['/page-not-found']);
        });
  }
}

