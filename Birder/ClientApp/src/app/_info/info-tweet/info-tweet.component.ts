import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { TweetDay } from '@app/_models/TweetDay';
import { TweetsService } from '@app/_services/tweets.service';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';

@Component({
  selector: 'app-info-tweet',
  templateUrl: './info-tweet.component.html',
  styleUrls: ['./info-tweet.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class InfoTweetComponent implements OnInit {
  tweet: TweetDay;
  requesting: boolean;

  constructor(private tweetsService: TweetsService) { }

  ngOnInit(): void {
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

