import { Component, ViewEncapsulation, OnInit, ChangeDetectorRef } from '@angular/core';
import { GeocodeService } from '../geocode.service';
import { LocationViewModel } from '../../_models/LocationViewModel';

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html',
  styleUrls: ['./counter.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CounterComponent implements OnInit {
  public currentCount = 0;
  searchAddress = '';
  location: LocationViewModel;
  loading: boolean;
  geoError: string;

  
  constructor(
    private geocodeService: GeocodeService,
    private ref: ChangeDetectorRef,
  ) { }

  ngOnInit() {
    this.addressToCoordinates('54.972237,-2.460856');
    // this.addressToCoordinates(this.location.lat.toString() + ',' + this.location.lng.toString());  // call from getObservation, etc...
  }

  closeAlert() {
    this.geoError = null;
  }

  // Move to its own service... return string with either search coords or error message
  getCurrentPosition() {
    if (window.navigator.geolocation) {
      window.navigator.geolocation.getCurrentPosition(
        (position) => {
          this.addressToCoordinates(position.coords.latitude.toString() + ',' + position.coords.longitude.toString());
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


  addressToCoordinates(searchValue: string) {
    // alert(searchValue);
    this.loading = true;
    this.geocodeService.geocodeAddress(searchValue)
      .subscribe((location: LocationViewModel) => {
        // alert('success');
        this.location = location;
        // console.log(location);
        this.loading = false;
        this.searchAddress = '';
        this.ref.detectChanges();
        // this.geocodeService.reverseGeocode(this.location);
      }
      );
  }

  public incrementCounter() {
    this.currentCount++;
  }

  placeMarker($event) {
    // console.log($event.coords.lat);
    // console.log($event.coords.lng);
    // alert($event.coords.lat + ',' + $event.coords.lng);
    // const model = <LocationViewModel>{
    //   latitude: $event.coords.lat,
    //   longitude: $event.coords.lng,
    //   formattedAddress: ''
    // };
    // this.loading = true;
    this.geocodeService.reverseGeocode($event.coords.lat, $event.coords.lng)
      .subscribe((location: LocationViewModel) => {
        // alert('success');
        this.location = location;
        // console.log(location);
        // this.loading = false;
        this.searchAddress = '';
        this.ref.detectChanges();
        // this.geocodeService.reverseGeocode(this.location);
      }
      );
  }
}
