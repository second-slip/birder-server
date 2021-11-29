import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ObservationCountService } from '@app/_services/observation-count.service';

@Component({
  selector: 'app-observation-count',
  templateUrl: './observation-count.component.html',
  styleUrls: ['./observation-count.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ObservationCountComponent implements OnInit {

  constructor(readonly _service: ObservationCountService) { }

  ngOnInit(): void {
    this._getData();
  }

  private _getData(): void {
    this._service.getData();
  }

  public reload(): void {
    this._getData();
  }
}
