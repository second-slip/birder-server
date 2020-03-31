import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { FlickrService, FlickrUrls } from '@app/flickr.service';

@Component({
  selector: 'app-testing',
  templateUrl: './testing.component.html',
  styleUrls: ['./testing.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TestingComponent implements OnInit {

  // currentResults: any[] = [];
  currentResults: FlickrUrls[];

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
