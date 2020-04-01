import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { IXenoCantoResponse } from '@app/_models/IXenoCantoResponse';
import { XenoCantoService } from '@app/xeno-canto.service';

@Component({
  selector: 'app-testing',
  templateUrl: './testing.component.html',
  styleUrls: ['./testing.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TestingComponent implements OnInit {
  recordings: IXenoCantoResponse;

  constructor(private xeno: XenoCantoService) { }

  ngOnInit() {
    this.xeno.getRecordings('troglodytes troglodytes')
    .subscribe((results: IXenoCantoResponse) => {
      this.recordings = results;
      console.log(results);
    });
  }
}
