import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { PhotographAlbum } from '@app/_models/PhotographAlbum';
import { ObservationService } from '@app/_sharedServices/observation.service';
import { UserViewModel } from '@app/_models/UserViewModel';
import { TokenService } from '@app/_services/token.service';
import { PhotosService } from '@app/_services/photos.service';
import { Lightbox } from 'ngx-lightbox';

@Component({
  selector: 'app-observation-detail',
  templateUrl: './observation-detail.component.html',
  styleUrls: ['./observation-detail.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ObservationDetailComponent implements OnInit {
  user: UserViewModel;
  observation: ObservationViewModel;
  private _album: Array<PhotographAlbum> = [];

  // @ViewChild

  constructor(private observationService: ObservationService
    , private _lightbox: Lightbox
    , private photosService: PhotosService
    , private tokenService: TokenService
    , private route: ActivatedRoute
    , private location: Location
    , private router: Router) { }

  ngOnInit(): void {
    this.getUser();
    this.getObservation();
  }

  getObservation(): void {
    const id = +this.route.snapshot.paramMap.get('id');

    this.observationService.getObservation(id)
      .subscribe(
        (observation: ObservationViewModel) => {
          this.observation = observation;
          this.getPhotos(observation.observationId);
        },
        () => {
          this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
        });
  }

  goBack(): void {
    this.location.back();
  }

  getUser(): void {
    this.tokenService.getAuthenticatedUserDetails()
      .subscribe(
        (data: UserViewModel) => {
          this.user = data;
        },
        () => {
          console.log('could not get the user, using default coordinates');
          const userTemp = <UserViewModel>{
            userName: '',
            avatar: '',
            defaultLocationLatitude: 54.972237,
            defaultLocationLongitude: -2.4608560000000352,
          };
          this.user = userTemp;
        });
  }

  open(index: number): void {
    // open lightbox
    this._lightbox.open(this._album, index);
  }

  close(): void {
    // close lightbox programmatically
    this._lightbox.close();
  }

  getPhotos(id: number): void {
    this.photosService.getPhotos(id)
      .subscribe(
        (result: any) => {
          this._album = result.map((photo): PhotographAlbum => ({
            src: photo.address,
            caption: this.observation.bird.englishName,
            thumb: photo.address,
            filename: photo.filename
          }));
        },
        () => {
          // this.errorReport = error;
          // this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
        });
  }
}
