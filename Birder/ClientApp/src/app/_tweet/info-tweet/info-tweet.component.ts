import { Component, ViewEncapsulation } from '@angular/core';
import { TweetDay } from '@app/_models/TweetDay';
import { TweetsService } from '@app/_tweet/tweets.service';
import { Observable, throwError } from 'rxjs';
import { catchError, share } from 'rxjs/operators';

@Component({
  selector: 'app-info-tweet',
  templateUrl: './info-tweet.component.html',
  styleUrls: ['./info-tweet.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class InfoTweetComponent {
  tweet$: Observable<TweetDay>;
  public errorObject = null;

  constructor(private tweetsService: TweetsService) {
    this.getTweetOfTheDay();
  }

  getTweetOfTheDay(): void {
    this.tweet$ = this.tweetsService.getTweetDay()
      .pipe(share(),
        catchError(err => {
          this.errorObject = err;
          return throwError(err);
        })
      );
  }
}

