import { Component, OnInit } from '@angular/core';
import { BirdsService } from '../birds.service';
import { Bird } from '../../_models/Bird';
import { ActivatedRoute, Router } from '@angular/router';
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
            , private location: Location
            , private router: Router) { }

  ngOnInit(): void {
    this.getBird();
  }

  getBird(): void {
    const id = +this.route.snapshot.paramMap.get('id');

    this.birdsService.getBird(id)
      .subscribe(bird => { this.bird = bird; },
        error => {
          console.log('bad request');
          this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
        });
    // ,() => {
    //   // alert('');
    //   // 'onCompleted' callback.
    //   // No errors, route to new page here
    // }
  }

  goBack(): void {
    this.location.back();
  }

}
