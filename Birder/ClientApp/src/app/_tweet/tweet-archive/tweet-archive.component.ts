import { Component, ViewEncapsulation } from '@angular/core';
import { TweetArchiveDto } from '@app/_models/TweetDay';
import { TweetsService } from '@app/_tweet/tweets.service';
import { Observable, throwError } from 'rxjs';
import { catchError, share } from 'rxjs/operators';

@Component({
  selector: 'app-tweet-archive',
  templateUrl: './tweet-archive.component.html',
  styleUrls: ['./tweet-archive.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TweetArchiveComponent {
  tweets$: Observable<TweetArchiveDto>;
  public errorObject = null;
  page = 1;
  pageSize = 15;

  constructor(private tweetsService: TweetsService) {
    this.getTweetArchive();
  }

  getTweetArchive(): void {
    this.tweets$ = this.tweetsService.getTweetArchive(this.page, this.pageSize)
      .pipe(share(),
        catchError(err => {
          this.errorObject = err;
          return throwError(err);
        })
      );
  }

  changePage(): void {
    this.getTweetArchive();
  }
}


