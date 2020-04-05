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
  birdId: number;
  bird: BirdDetailViewModel;
  images: FlickrUrlsViewModel[];
  tabstatus = {};
  active;

  constructor(private birdsService: BirdsService
    , private flickr: FlickrService
    , private route: ActivatedRoute
    , private location: Location
    , private router: Router) {
    route.params.subscribe(_ => {
      // this.birdId = +this.route.snapshot.paramMap.get('id');
      this.route.paramMap.subscribe(pmap => this.getBird(+pmap.get('id')));
      // this.getBird();
      // the next two statements reset the tabs.  This is required when the page is reloaded with
      // different data.  Otherwise the 'sightings' & 'voice' child components keep its original data.
      this.active = 1;
      this.tabstatus = {};
    });
  }

  getBird(id: number): void {
    this.birdsService.getBird(id)
      .subscribe(
        (data: BirdDetailViewModel) => {
          this.bird = data;
          this.getImages(data.species);
        },
        (error: ErrorReportViewModel) => {
          // TODO: show toast error
          this.gotoBirdsList();
        });
  }

  getImages(species: string) {
    this.flickr.getSearchResults(1, species)
      .subscribe((results: any) => {
        this.images = this.flickr.getPhotoThumnail(results.photos.photo);
      });
      // error...?
  }

  gotoBirdsList() {
    this.router.navigate(['/birds-index'], {relativeTo: this.route});
  }

  goBack(): void {
    this.location.back();
  }
}
