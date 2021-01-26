import { Component, ViewEncapsulation, OnInit } from '@angular/core';

@Component({
  selector: 'app-testing',
  templateUrl: './testing.component.html',
  styleUrls: ['./testing.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TestingComponent implements OnInit {
  

  constructor() { }
  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }
  
}