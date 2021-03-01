import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { RecordingViewModel } from '@app/_models/RecordingViewModel';
import { Observable, throwError } from 'rxjs';
import { catchError, share } from 'rxjs/operators';
import { RecordingsService } from '../recordings.service';

@Component({
  selector: 'app-song-recordings',
  templateUrl: './song-recordings.component.html',
  styleUrls: ['./song-recordings.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SongRecordingsComponent implements OnInit {
  @Input() species: string;
  recordings$: Observable<RecordingViewModel[]>;
  public errorObject = null;
  page: number;
  pageSize = 10;

  constructor(private recordingsService: RecordingsService) {
    // this.loadRecordings();
  }

  ngOnInit(): void {
    this.loadRecordings();
    this.page = 1;
  }

  loadRecordings(): void {
    this.recordings$ = this.recordingsService.getRecordings(this.species)
      .pipe(share(),
        catchError(err => {
          this.errorObject = err;
          return throwError(err); // error thrown by interceptor...
        }));
    // .subscribe(
    //   ((results: RecordingViewModel[]) => {
    //     this.recordings = results;
    //     this.page = 1;
    //   }),
    //   (_ => {
    //     this.error = true;
    //   }));
  }
}
