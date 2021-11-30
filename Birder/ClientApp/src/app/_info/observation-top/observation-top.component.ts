import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ObservationTopService } from '../observation-top.service';

@Component({
  selector: 'app-observation-top',
  templateUrl: './observation-top.component.html',
  styleUrls: ['./observation-top.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ObservationTopComponent implements OnInit {
  active: number = 1;

  constructor(readonly _service: ObservationTopService) { }

  ngOnInit(): void {
    this._getData();
  }

  private _getData(): void {
    this._service.getData();
    //this._setActiveTab();    -----> how to do this?
  }

  public reload(): void {
    this._getData();
  }

  // ??????????
  private _setActiveTab(qtyThisMonth: number): void {
    if (qtyThisMonth) {
      this.active = 1;
    } else {
      this.active = 2;
    }
  }
}
