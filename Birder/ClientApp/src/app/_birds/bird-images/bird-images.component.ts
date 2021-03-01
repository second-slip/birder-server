import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { FlickrUrlsViewModel } from '@app/_models/FlickrUrlsViewModel';
import { FlickrService } from '@app/_services/flickr.service';
import { Observable, throwError } from 'rxjs';
import { catchError, share } from 'rxjs/operators';

@Component({
  selector: 'app-bird-images',
  templateUrl: './bird-images.component.html',
  styleUrls: ['./bird-images.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class BirdImagesComponent implements OnInit {
  @Input() species: string;


  images$: Observable<FlickrUrlsViewModel[]>;
  public errorObject = null;

  constructor(private flickr: FlickrService) {
    
    //alert(this.species);
  }
  
  ngOnInit(): void {
    this.getImages();
  }


  getImages() {
    alert(this.species);
    this.images$ = this.flickr.getSearchResults(1, this.species)
      .pipe(share(),
        catchError(err => {
          this.errorObject = err;
          return throwError(err); // error thrown by interceptor...
        }));

    // .subscribe((results: any) => {
    //   this.images = this.flickr.getPhotoThumnail(results.photos.photo);
    // });
    // error...?
  }
}
