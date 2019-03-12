import { Component, OnInit } from '@angular/core';
import { BirdsService } from '../birds.service';
import { Bird } from '../../_models/Bird';
import { Router } from '@angular/router';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';

@Component({
  selector: 'app-birds-index',
  templateUrl: './birds-index.component.html',
  styleUrls: ['./birds-index.component.scss']
})
export class BirdsIndexComponent implements OnInit {

  birds: Bird[];

  constructor(private birdsService: BirdsService
    , private router: Router) { }

  ngOnInit() {
    this.getBirds();
  }

  getBirds(): void {
    this.birdsService.getBirds()
      .subscribe(
        (data: Bird[]) => { this.birds = data; },
        (error: ErrorReportViewModel) => {
          this.router.navigate(['/page-not-found']);
        });
  }
}

