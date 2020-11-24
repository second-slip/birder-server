import { Component, OnInit, ChangeDetectorRef, ViewEncapsulation, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { SetLocationViewModel } from '@app/_models/SetLocationViewModel';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { UserViewModel } from '@app/_models/UserViewModel';
import { TokenService } from '@app/_services/token.service';
import { AccountManagerService } from '@app/_services/account-manager.service';
import { GoogleMap, MapInfoWindow, MapMarker } from '@angular/google-maps';
import { GeocodingService } from '@app/_services/geocoding.service';

@Component({
  selector: 'app-account-manager-location',
  templateUrl: './account-manager-location.component.html',
  styleUrls: ['./account-manager-location.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AccountManagerLocationComponent implements OnInit {
  requesting: boolean;
  errorReport: ErrorReportViewModel;
  geolocation: string;
  searchAddress = '';
  geoError: string;
  marker; // make marker a property?

  @ViewChild(GoogleMap, { static: false }) map: GoogleMap
  @ViewChild(MapInfoWindow, { static: false }) infoWindow: MapInfoWindow
  zoom = 11;
  options: google.maps.MapOptions = {
    mapTypeId: 'terrain'
  }

  constructor(private router: Router
    , private tokenService: TokenService
    , private accountManager: AccountManagerService
    , private geocoding: GeocodingService
    , private ref: ChangeDetectorRef) { }

  ngOnInit() {
    this.getUser();
  }

  getUser(): void {
    this.tokenService.getAuthenticatedUserDetails()
      .subscribe(
        (data: UserViewModel) => {
          this.addMarker(data.defaultLocationLatitude, data.defaultLocationLongitude, true);
        },
        (error: any) => {
          console.log('could not get the user, using default coordinates');
        });
  }

  addMarker(latitude: number, longitude: number, getAddress: boolean) {
    this.marker = ({
      position: {
        lat: latitude,
        lng: longitude
      },
      label: {
        color: 'red',
        text: 'Marker label',
      },
      title: 'Marker title',
      options: { draggable: true },
    })

    if (getAddress) {
      this.getFormattedAddress(latitude, longitude);
    }
  }

  openInfoWindow(marker: MapMarker) {
    this.infoWindow.open(marker);
  }

  markerChanged(event: google.maps.MouseEvent): void {
    this.addMarker(event.latLng.lat(), event.latLng.lng(), true);
  }

  getFormattedAddress(latitude: number, longitude: number): void {
    this.geocoding.reverseGeocode(latitude, longitude)
      .subscribe(
        (response: any) => {
          this.geolocation = response.results[0].formatted_address;
          this.ref.detectChanges();
        },
        (error: any) => {
          //
        }
      );
  }

  findAddress(searchValue: string) {
    this.geocoding.geocode(searchValue)
      .subscribe(
        (response: any) => {
          this.addMarker(response.results[0].geometry.location.lat, response.results[0].geometry.location.lng, false); // false to stop second hit on API to get address...
          this.geolocation = response.results[0].formatted_address;
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
          this.addMarker(position.coords.latitude, position.coords.longitude, true);
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

  onSubmit(): void {
    this.requesting = true;
    const model = <SetLocationViewModel>{
      defaultLocationLatitude: this.marker.position.lat,
      defaultLocationLongitude: this.marker.position.lng,
    };

    // console.log(model);

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
