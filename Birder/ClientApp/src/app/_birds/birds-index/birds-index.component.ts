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
  pageSize = 25;

  constructor(private birdsService: BirdsService
    , private router: Router) { }

  ngOnInit() {
    this.getBirds(1, this.pageSize);
  }

  changePage() { // event) { // page: number) {
    this.getBirds(this.page, this.pageSize);
  }

  getBirds(page: number, pageSize: number): void {
    this.birdsService.getBirds(page, pageSize)
      .subscribe(
        (data: BirdsDto) => { // (data: BirdSummaryViewModel[]) => {
          this.birds = data.items;
          this.totalItems = data.totalItems;
        },
        (error: ErrorReportViewModel) => {
          this.router.navigate(['/page-not-found']);
        },
        () => {
          // operations when URL request is completed
        });
  }
}

// import { Component, ViewEncapsulation, OnInit, ViewChild } from '@angular/core';
// import { PageEvent, MatPaginator } from '@angular/material/paginator';
// import { MatSort } from '@angular/material/sort';
// import { MatTableDataSource } from '@angular/material/table';
// import { BirdSummaryViewModel } from '@app/_models/BirdSummaryViewModel';
// import { BirdsService } from '@app/_services/birds.service';
// import { Router } from '@angular/router';
// import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';

// @Component({
//   selector: 'app-birds-index',
//   templateUrl: './birds-index.component.html',
//   styleUrls: ['./birds-index.component.scss'],
//   encapsulation: ViewEncapsulation.None
// })
// export class BirdsIndexComponent implements OnInit {
//   displayedColumns: string[] = ['englishName', 'btoStatusInBritain', 'conservationStatus'];
//   dataSource: MatTableDataSource<BirdSummaryViewModel[]>;
//   length: number;

//   pageEvent: PageEvent;

//   @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
//   @ViewChild(MatSort, { static: true }) sort: MatSort;

//   constructor(private birdsService: BirdsService
//     , private router: Router) { }

//   ngOnInit() {
//     this.getBirds(1, 25);
//   }

//   // hello(e) {
//   //   console.log(e);
//   //   if (e.checked) {
//   //     // alert('I have been checked');
//   //     this.getBirds(BirderStatus.Uncommon);
//   //   } else {
//   //     // alert('I have been unchecked');
//   //     this.getBirds(BirderStatus.Common);
//   //   }
//   // }

//   onPaginateChange(event) {
//     // alert(JSON.stringify('Current page index: ' + event.pageIndex));

//     this.getBirds(event.pageIndex + 1, event.pageSize);
//   }

//   applyFilter(filterValue: string) {
//     this.dataSource.filter = filterValue.trim().toLowerCase();

//     if (this.dataSource.paginator) {
//       this.dataSource.paginator.firstPage();
//     }
//   }

//   getBirds(pageIndex: number, pageSize: number): void {
//     this.birdsService.getBirds(pageIndex, pageSize)
//       .subscribe(
//         (data: any) => { // (data: BirdSummaryViewModel[]) => {
//           this.dataSource = data.items;  // new MatTableDataSource(data.items);
//           this.length = data.totalItems;
//         },
//         (error: ErrorReportViewModel) => {
//           this.router.navigate(['/page-not-found']);
//         },
//         () => {
//           // operations when URL request is completed
//           this.dataSource.paginator = this.paginator;
//           this.dataSource.sort = this.sort;
//         });
//   }
// }

