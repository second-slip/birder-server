import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AboutComponent implements OnInit {

  // constructor(private titleService: Title) { }
  constructor() { }

  ngOnInit() {
    // this.setTitle();
  }

  public setTitle() {
    // this.titleService.setTitle('Birder');
  }

}
