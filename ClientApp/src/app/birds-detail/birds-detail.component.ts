import { Component, OnInit } from '@angular/core';
import { BirdsService } from '../birds.service';
import { Bird } from '../../_models/Bird';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-birds-detail',
  templateUrl: './birds-detail.component.html',
  styleUrls: ['./birds-detail.component.scss']
})
export class BirdsDetailComponent implements OnInit {
  bird: Bird;

  constructor(private birdsService: BirdsService
            , private route: ActivatedRoute
            , private location: Location) { }

  ngOnInit(): void {
    this.getBird();
  }

  getBird(): void {
    const id = +this.route.snapshot.paramMap.get('id');
    this.birdsService.getBird(id)
      .subscribe(bird => this.bird = bird);
      // TODO: error - what to do if someone types route with bad id.. eg. 404
  }

  // getBird2(): void {
  //   const id = +this.route.snapshot.paramMap.get('id');
  //   this.birdsService.getBird(id)
  //     .subscribe(bird => { this.bird = bird; },
  //       error => {
  //         alert('bad request');
  //       },
  //       () => {
  //         alert('');
  //         // 'onCompleted' callback.
  //         // No errors, route to new page here
  //       }
  //     );

  //     // TODO: error - what to do if someone types route with bad id.. eg. 404
  // }

  goBack(): void {
    this.location.back();
  }

}
