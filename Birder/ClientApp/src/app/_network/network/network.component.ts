import { Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-network',
  templateUrl: './network.component.html',
  styleUrls: ['./network.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class NetworkComponent implements OnInit {
  tabstatus = {};
  active;

  constructor() { }

  ngOnInit() {
    this.active = 1;
    this.tabstatus = {};
  }
}
