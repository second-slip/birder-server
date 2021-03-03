import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { PhotographAlbum } from '@app/_models/PhotographAlbum';
import { ObservationService } from '@app/_sharedServices/observation.service';
import { UserViewModel } from '@app/_models/UserViewModel';
import { TokenService } from '@app/_services/token.service';
import { PhotosService } from '@app/_services/photos.service';
// import { Lightbox } from 'ngx-lightbox';

/*  ******** information ********
  child view is accessed via the #map local variable.  This is to access 'geolocation' property.
  Local varaible binding is only suitable for simple things like this...
*/

@Component({
  selector: 'app-observation-detail',
  templateUrl: './observation-detail.component.html',
  styleUrls: ['./observation-detail.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ObservationDetailComponent implements OnInit {
  user: UserViewModel;
  observation: ObservationViewModel;
  // private _album: Array<PhotographAlbum> = [];

  constructor(private observationService: ObservationService
    // , private _lightbox: Lightbox
    // , private photosService: PhotosService
    , private tokenService: TokenService
    , private route: ActivatedRoute
    , private router: Router) { }

  ngOnInit(): void {
    this.user = this.tokenService.getAuthenticatedUserDetails();
    this.getObservation();
  }

  getObservation(): void {
    const id = +this.route.snapshot.paramMap.get('id');

    this.observationService.getObservation(id)
      .subscribe(
        (observation: ObservationViewModel) => {
          this.observation = observation;
          //this.getPhotos(observation.observationId);
        },
        () => {
          this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
        });
  }

  // getUser(): void {
  //   this.tokenService.getAuthenticatedUserDetails()
  //     .subscribe(
  //       (data: UserViewModel) => {
  //         this.user = data;
  //       },
  //       () => { });
  // }

  // open(index: number): void { // open lightbox
  //   this._lightbox.open(this._album, index);
  // }

  // close(): void { // close lightbox programmatically
  //   this._lightbox.close();
  // }

  // getPhotos(id: number): void {
  //   this.photosService.getPhotos(id)
  //     .subscribe(
  //       (result: any) => {
  //         this._album = result.map((photo): PhotographAlbum => ({
  //           src: photo.address,
  //           caption: this.observation.bird.englishName,
  //           thumb: photo.address,
  //           filename: photo.filename
  //         }));
  //       },
  //       () => {
  //         // this.errorReport = error;
  //         // this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
  //       });
  // }
}
