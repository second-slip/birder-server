import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { TweetArchiveDto } from '@app/_models/TweetDay';
import { TweetsService } from '@app/_services/tweets.service';

@Component({
  selector: 'app-tweet-archive',
  templateUrl: './tweet-archive.component.html',
  styleUrls: ['./tweet-archive.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TweetArchiveComponent implements OnInit {
  tweets: TweetArchiveDto;

  constructor(private tweetsService: TweetsService) { }

  ngOnInit(): void {
    this.getTweetArchive();
  }

  getTweetArchive() {
    this.tweetsService.getTweetArchive(1, 25)
      .subscribe(
        (data: TweetArchiveDto) => {
          this.tweets = data;
        },
        (error: ErrorReportViewModel) => {
          // TOAST........................................................................................
        }
      );
  }
}


