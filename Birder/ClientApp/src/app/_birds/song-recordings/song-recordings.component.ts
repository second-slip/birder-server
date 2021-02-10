import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { RecordingViewModel } from '@app/_models/RecordingViewModel';
import { RecordingsService } from '../recordings.service';

@Component({
  selector: 'app-song-recordings',
  templateUrl: './song-recordings.component.html',
  styleUrls: ['./song-recordings.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SongRecordingsComponent implements OnInit {
  @Input() species: string;
  recordings: RecordingViewModel[];
  page: number;
  pageSize = 10;
  error = false;

  constructor(private recordingsService: RecordingsService) { }

  ngOnInit(): void {
    if (!this.recordings) {
      this.loadRecordings();
    }
  }

  loadRecordings(): void {
    this.recordingsService.getRecordings(this.species)
    .subscribe(
      ((results: RecordingViewModel[]) => {
        this.recordings = results;
        this.page = 1;
      }),
      (_ => {
        this.error = true;
      }));
  }

}
