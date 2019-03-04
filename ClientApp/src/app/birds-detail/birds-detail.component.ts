import { Component, OnInit } from '@angular/core';
import { BirdsService } from '../birds.service';
import { Bird } from '../../_models/Bird';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-birds-detail',
  templateUrl: './birds-detail.component.html',
  styleUrls: ['./birds-detail.component.scss']
})
export class BirdsDetailComponent implements OnInit {

  bird: Bird;

  constructor(private birdsService: BirdsService
    , private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.getHero();
  }

  getHero(): void {
    const id = +this.route.snapshot.paramMap.get('id');
    alert(id);
    this.birdsService.getBird(id)
      .subscribe(bird => this.bird = bird);
      alert(this.bird);
  }

}
