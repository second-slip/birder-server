import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { BirdSummaryViewModel, BirdsDto } from '@app/_models/BirdSummaryViewModel';
import { BirdsService } from '@app/_services/birds.service';
import { Router } from '@angular/router';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';

@Component({
  selector: 'app-birds-index',
  templateUrl: './birds-index.component.html',
  styleUrls: ['./birds-index.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class BirdsIndexComponent implements OnInit {
  birds: BirdSummaryViewModel[];
  totalItems: number;
  page = 1;
  pageSize = 30;
  speciesFilter: string;
  requesting: boolean;

  constructor(private birdsService: BirdsService
    , private router: Router) { }

  ngOnInit() {
    this.speciesFilter = '0';
    this.getBirds();
  }

  changePage(): void {
    this.getBirds();
  }

  onChangeFilter(filter: string): void {
    this.page = 1;
    this.speciesFilter = filter;
    this.getBirds();
  }

  getBirds(): void {
    this.requesting = true;
    this.birdsService.getBirds(this.page, this.pageSize, this.speciesFilter)
      .subscribe(
        (data: BirdsDto) => { // (data: BirdSummaryViewModel[]) => {
          this.birds = data.items;
          this.totalItems = data.totalItems;
        },
        (error: ErrorReportViewModel) => {
          this.router.navigate(['/page-not-found']);
        },
        () => {
          this.requesting = false;
          window.scrollTo({ top: 0, behavior: 'smooth' });
        });
  }
}
