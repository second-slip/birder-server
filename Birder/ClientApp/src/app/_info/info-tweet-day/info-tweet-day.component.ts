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
  // error = false;
  isLoading: boolean;

  constructor(private tweetsService: TweetsService) { }

  ngOnInit() {
    this.getTweetOfTheDay();
  }

  getTweetOfTheDay(): void {
    this.isLoading = true;
    this.tweetsService.getTweetDay()
      .subscribe(
        (data: TweetDay) => {
          this.tweet = data;
          this.isLoading = false;
        },
        (error: ErrorReportViewModel) => {
          console.log(error);
          // this.error = true;
          this.isLoading = false
          // alert('error...');
          // ToDo: Something with the error (perhaps show a message)
        }
      );
  }
}
