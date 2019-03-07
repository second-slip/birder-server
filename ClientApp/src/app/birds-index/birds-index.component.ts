import { Component, OnInit } from '@angular/core';
import { BirdsService } from '../birds.service';
import { Bird } from '../../_models/Bird';
import { Router } from '@angular/router';

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
      .subscribe(birds => { this.birds = birds; },
        error => {
          this.router.navigate(['/page-not-found']);
        });
  }
}

