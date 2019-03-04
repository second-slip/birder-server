import { Component, OnInit } from '@angular/core';
import { BirdsService } from '../birds.service';
import { Bird } from '../../_models/Bird';

@Component({
  selector: 'app-birds-index',
  templateUrl: './birds-index.component.html',
  styleUrls: ['./birds-index.component.scss']
})
export class BirdsIndexComponent implements OnInit {

  birds: Bird[];

  constructor(private birdsService: BirdsService) { }

  ngOnInit() {
    this.getBirds();
  }

  getBirds(): void {
    this.birdsService.getBirds()
    .subscribe(birds => this.birds = birds);
  }

}
