import { Component, OnInit, Input } from '@angular/core';
import { BirdsService } from '../birds.service';
import { Bird } from '../../_models/Bird';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-birds-detail',
  templateUrl: './birds-detail.component.html',
  styleUrls: ['./birds-detail.component.scss']
})
export class BirdsDetailComponent implements OnInit {

  // @Input() bird: Bird;
  bird: Bird;

  constructor(private birdsService: BirdsService
    , private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.getBird();
    console.log(this.bird);
  }

  getBird(): void {
    const id = +this.route.snapshot.paramMap.get('id');
    this.birdsService.getBird(id)
      .subscribe(bird => this.bird = bird);
      
  }

}
