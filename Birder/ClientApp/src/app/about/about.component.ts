import { Component, OnInit, ViewEncapsulation } from '@angular/core';
// import { ViewEncapsulation } from '@angular/compiler/src/core';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AboutComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
