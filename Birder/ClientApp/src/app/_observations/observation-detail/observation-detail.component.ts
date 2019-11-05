import { Component, OnInit, ChangeDetectorRef, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { PhotographAlbum } from '@app/_models/PhotographAlbum';
import { ObservationService } from '@app/_services/observation.service';
import { GeocodeService } from '@app/_services/geocode.service';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { LocationViewModel } from '@app/_models/LocationViewModel';
import { UserViewModel } from '@app/_models/UserViewModel';
import { TokenService } from '@app/_services/token.service';


@Component({
  selector: 'app-observation-detail',
  templateUrl: './observation-detail.component.html',
  styleUrls: ['./observation-detail.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ObservationDetailComponent implements OnInit {
  user: UserViewModel;
  observation: ObservationViewModel;
  geolocation = 'location';
  private _album: Array<PhotographAlbum> = [];

  constructor(private observationService: ObservationService
            , private tokenService: TokenService
            , private route: ActivatedRoute
            , private location: Location
            , private router: Router
            , private geocodeService: GeocodeService
            , private ref: ChangeDetectorRef) { }

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
          this.getGeolocation();
        },
        (error: ErrorReportViewModel) => {
          this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
        });
  }

  getGeolocation(): void {
    this.geocodeService.reverseGeocode(this.observation.locationLatitude, this.observation.locationLongitude)
    .subscribe(
      (data: LocationViewModel) => {
      this.geolocation = data.formattedAddress;

      this.ref.detectChanges();
    },
    (error: any) => {
      //
    }
    );
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
        (error: any) => {
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
}
