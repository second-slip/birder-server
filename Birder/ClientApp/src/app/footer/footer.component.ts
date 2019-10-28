import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class FooterComponent implements OnInit {
  private currentYear: number;
  private title: string;

  constructor(private titleService: Title) { }

  ngOnInit() {
    this.title = this.titleService.getTitle();
    this.currentYear = new Date().getFullYear();
  }

}
