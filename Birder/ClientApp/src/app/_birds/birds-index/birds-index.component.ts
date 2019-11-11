import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { BirdsService } from '../../_services/birds.service';
import { Router } from '@angular/router';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';
import { BirdSummaryViewModel } from '../../_models/BirdSummaryViewModel';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { BirderStatus } from '../../_models/BirdIndexOptions';

@Component({
  selector: 'app-birds-index',
  templateUrl: './birds-index.component.html',
  styleUrls: ['./birds-index.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class BirdsIndexComponent implements OnInit {
  displayedColumns: string[] = ['englishName', 'btoStatusInBritain', 'conservationStatus'];
  dataSource: MatTableDataSource<any>;
  length: number;

  pageEvent: PageEvent;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(private birdsService: BirdsService
            , private router: Router) { }

  ngOnInit() {
    this.getBirds(1, 25);
  }

  // hello(e) {
  //   console.log(e);
  //   if (e.checked) {
  //     // alert('I have been checked');
  //     this.getBirds(BirderStatus.Uncommon);
  //   } else {
  //     // alert('I have been unchecked');
  //     this.getBirds(BirderStatus.Common);
  //   }
  // }

  onPaginateChange(event){
    // alert(JSON.stringify('Current page index: ' + event.pageIndex));

    this.getBirds(event.pageIndex + 1, event.pageSize);
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  getBirds(pageIndex: number, pageSize: number): void {
    this.birdsService.getBirdsIndex(pageIndex, pageSize)
      .subscribe(
        (data: any) => { // (data: BirdSummaryViewModel[]) => {
          this.dataSource = data.items;  // new MatTableDataSource(data.items);
          this.length = data.totalItems;
         },
        (error: ErrorReportViewModel) => {
          this.router.navigate(['/page-not-found']);
        },
        () => {
          // operations when URL request is completed
          
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
       });
  }
}

