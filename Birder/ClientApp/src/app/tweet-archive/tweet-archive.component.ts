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
  totalItems: number;
  page = 1;
  pageSize = 25;

  constructor(private tweetsService: TweetsService) { }

  ngOnInit(): void {
    this.getTweetArchive(this.page, this.pageSize);
  }

  getTweetArchive(page: number, pageSize: number) {
    this.tweetsService.getTweetArchive(this.page, this.pageSize)
      .subscribe(
        (data: TweetArchiveDto) => {
          this.tweets = data;
          this.totalItems = data.totalItems;
        },
        (error: ErrorReportViewModel) => {
          // TOAST........................................................................................
        }
      );
  }

  changePage() { // event) { // page: number) {
    this.getTweetArchive(this.page, this.pageSize);
  }
}


