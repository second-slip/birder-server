import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import { XenoCantoService } from '@app/xeno-canto.service';
import { IXenoCantoResponse } from '@app/_models/IXenoCantoResponse';

@Component({
  selector: 'app-birds-voice',
  templateUrl: './birds-voice.component.html',
  styleUrls: ['./birds-voice.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class BirdsVoiceComponent implements OnInit {
  @Input() species: string;
  recordings: IXenoCantoResponse;
  page: number;
  pageSize = 3;

  constructor(private xeno: XenoCantoService) { }

  ngOnInit(): void {
    if (!this.recordings) {
    this.loadRecordings();
    }
  }

  loadRecordings(): void {
    this.xeno.getRecordings(this.species)
      .subscribe((results: IXenoCantoResponse) => {
        results.recordings.length = 10;
        this.recordings = results;
        this.page = 1;
      });
  }

}
