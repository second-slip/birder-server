import { Component, ViewEncapsulation, OnInit } from '@angular/core';

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html',
  styleUrls: ['./counter.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CounterComponent implements OnInit {
  public currentCount = 0;

  constructor() { }

  ngOnInit() {
  }

  public incrementCounter() {
    this.currentCount++;
  }
}
