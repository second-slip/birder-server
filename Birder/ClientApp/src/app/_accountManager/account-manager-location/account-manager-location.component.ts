import { Component, OnInit, ChangeDetectorRef, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { SetLocationViewModel } from '@app/_models/SetLocationViewModel';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { UserViewModel } from '@app/_models/UserViewModel';
import { TokenService } from '@app/_services/token.service';
import { AccountManagerService } from '@app/_services/account-manager.service';
import { LocationViewModel } from '@app/_models/LocationViewModel';
import { GeocodeService } from '@app/_services/geocode.service';


@Component({
  selector: 'app-account-manager-location',
  templateUrl: './account-manager-location.component.html',
  styleUrls: ['./account-manager-location.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AccountManagerLocationComponent implements OnInit {
  requesting: boolean;
  model: SetLocationViewModel;
  setLocationForm: FormGroup;
  errorReport: ErrorReportViewModel;
  geolocation: string;
  searchAddress = '';
  geoError: string;
  user: UserViewModel;

  constructor(private router: Router
    , private tokenService: TokenService
    , private accountManager: AccountManagerService
    , private formBuilder: FormBuilder
    , private geocodeService: GeocodeService
    , private ref: ChangeDetectorRef) { }

  ngOnInit() {
    this.getUser();
  }

  getUser(): void {
    this.tokenService.getAuthenticatedUserDetails()
      .subscribe(
        (data: UserViewModel) => {
          // this.model.defaultLocationLatitude = data.defaultLocationLatitude;
          // this.model.defaultLocationLongitude = data.defaultLocationLongitude;
          this.user = data;
          this.createForms();
          this.getGeolocation();
        },
        (error: any) => {
          console.log('could not get the user, using default coordinates');
        });
  }

  createForms(): void {
    this.setLocationForm = this.formBuilder.group({
      locationLatitude: new FormControl(this.user.defaultLocationLatitude),
      locationLongitude: new FormControl(this.user.defaultLocationLongitude),
    });
  }

  // new google maps methods...

  getGeolocation(): void {
    this.geocodeService.reverseGeocode(this.user.defaultLocationLatitude, this.user.defaultLocationLongitude)
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

  useGeolocation(searchValue: string) {
    this.geocodeService.geocodeAddress(searchValue)
      .subscribe((location: LocationViewModel) => {
        this.setLocationForm.get('locationLatitude').setValue(location.latitude);
        this.setLocationForm.get('locationLongitude').setValue(location.longitude);
        this.geolocation = location.formattedAddress;
        this.searchAddress = '';
        this.ref.detectChanges();
      }
      );
  }

  closeAlert() {
    this.geoError = null;
  }

  getCurrentPosition() {
    if (window.navigator.geolocation) {
      window.navigator.geolocation.getCurrentPosition(
        (position) => {
          this.useGeolocation(position.coords.latitude.toString() + ',' + position.coords.longitude.toString());
        }, (error) => {
          switch (error.code) {
            case 3: // ...deal with timeout
              this.geoError = 'The request to get user location timed out...';
              break;
            case 2: // ...device can't get data
              this.geoError = 'Location information is unavailable...';
              break;
            case 1: // ...user said no ☹️
              this.geoError = 'User denied the request for Geolocation...';
              break;
            default:
              this.geoError = 'An error occurred with Geolocation...';
          }
        });
    } else {
      this.geoError = 'Geolocation not supported in this browser';
    }
  }

  placeMarker($event) {
    this.geocodeService.reverseGeocode($event.coords.lat, $event.coords.lng)
      .subscribe(
        (location: LocationViewModel) => {
          this.setLocationForm.get('locationLatitude').setValue(location.latitude);
          this.setLocationForm.get('locationLongitude').setValue(location.longitude);
          this.geolocation = location.formattedAddress;
          this.ref.detectChanges();
        },
        (error: any) => { }
      );
  }

  onSubmit(value): void {
    this.requesting = true;
    const model = <SetLocationViewModel> {
      defaultLocationLatitude: value.locationLatitude,
      defaultLocationLongitude: value.locationLongitude,
    };

    this.accountManager.postSetLocation(model)
    .subscribe(
       (data: SetLocationViewModel) => {
        //  console.log('successful registration');
         this.router.navigate(['login']);
       },
      (error: ErrorReportViewModel) => {
        // if (error.status === 400) { }
        // this.errorReport = error;
        // this.unsuccessful = true;
        this.requesting = false;
        console.log(error.friendlyMessage);
        console.log('unsuccessful registration');
      });
  }
}
