import { Component, ViewEncapsulation } from '@angular/core';
import { BirdsService } from '../../_services/birds.service';
import { BirdDetailViewModel } from '../../_models/BirdDetailViewModel';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';
import { FlickrService } from '@app/_services/flickr.service';
import { FlickrUrlsViewModel } from '@app/_models/FlickrUrlsViewModel';

@Component({
  selector: 'app-birds-detail',
  templateUrl: './birds-detail.component.html',
  styleUrls: ['./birds-detail.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class BirdsDetailComponent {
  bird: BirdDetailViewModel;
  tabstatus = {};
  active;

  images: FlickrUrlsViewModel[];

  constructor(private birdsService: BirdsService
    , private flickr: FlickrService
    , private route: ActivatedRoute
    , private location: Location
    , private router: Router) {
    route.params.subscribe(_ => {
      this.getBird();
      // the next two statements reset the tabs.  This is required when the page is reloaded with
      // different data.  Otherwise the 'sightings' & 'voice' child components keep its original data.
      this.active = 1;
      this.tabstatus = {};
    });
  }

  getBird(): void {
    const id = +this.route.snapshot.paramMap.get('id');

    this.birdsService.getBird(id)
      .subscribe(
        (data: BirdDetailViewModel) => {
          this.bird = data;
          this.getImages(data.species);
        },
        (error: ErrorReportViewModel) => {
          console.log('bad request');
          this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
        });
  }

  getImages(species: string) {
    this.flickr.getSearchResults(1, species)
      .subscribe((results: any) => {
        this.images = this.flickr.getPhotoThumnail(results.photos.photo);
      });
      // error...?
  }

  goBack(): void {
    this.location.back();
  }
}
