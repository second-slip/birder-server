import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { BirdsService } from '@app/_services/birds.service';

@Component({
  selector: 'app-testing',
  templateUrl: './testing.component.html',
  styleUrls: ['./testing.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TestingComponent implements OnInit {

  constructor(private birdsService: BirdsService) { }

  ngOnInit() { }
}
