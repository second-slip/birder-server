import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { FlickrService } from '@app/flickr.service';
import { FlickrUrlsViewModel } from '@app/_models/FlickrUrlsViewModel';

@Component({
  selector: 'app-testing',
  templateUrl: './testing.component.html',
  styleUrls: ['./testing.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TestingComponent implements OnInit {
  currentResults: FlickrUrlsViewModel[];

  constructor(private flickr: FlickrService) { }

  ngOnInit() {

    // this.x();
    this.y();
  }

  y() {
        // call this after successful getBird() request
    this.flickr.getSearchResults(1, 'Alca arctica')
    .subscribe((results: any) => {
      this.currentResults = this.flickr.getPhotoThumnail(results.photos.photo);
      // console.log(this.currentResults);
    });
  }
}
