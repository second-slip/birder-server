import { Component, OnInit } from '@angular/core';
import { BirdsService } from '../birds.service';

@Component({
  selector: 'app-birds-index',
  templateUrl: './birds-index.component.html',
  styleUrls: ['./birds-index.component.scss']
})
export class BirdsIndexComponent implements OnInit {

  constructor(private birdsService: BirdsService) { }

  ngOnInit() {
  }

}
