import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { TweetDay } from '../../_models/TweetDay';
import { TweetsService } from '../../_services/tweets.service';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';

@Component({
  selector: 'app-info-tweet-day',
  templateUrl: './info-tweet-day.component.html',
  styleUrls: ['./info-tweet-day.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class InfoTweetDayComponent implements OnInit {
  tweet: TweetDay;
  requesting: boolean;

  constructor(private tweetsService: TweetsService) { }

  ngOnInit() {
    this.getTweetOfTheDay();
  }

  getTweetOfTheDay(): void {
    this.requesting = true;
    this.tweetsService.getTweetDay()
      .subscribe(
        (data: TweetDay) => {
          this.tweet = data;
          this.requesting = false;
        },
        (error: ErrorReportViewModel) => {
          // console.log(error);
          this.requesting = false
        }
      );
  }
}
