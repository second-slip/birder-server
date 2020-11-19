import { ChangeDetectorRef, Component, Input, OnInit, ViewChild } from '@angular/core';
import { MapInfoWindow, MapMarker } from '@angular/google-maps';
import { LocationViewModel } from '@app/_models/LocationViewModel';
import { GeocodeService } from '@app/_services/geocode.service';

@Component({
  selector: 'app-view-edit-single-marker-map',
  templateUrl: './view-edit-single-marker-map.component.html',
  styleUrls: ['./view-edit-single-marker-map.component.scss']
})
export class ViewEditSingleMarkerMapComponent implements OnInit {
  @Input() latitude: number;
  @Input() longitude: number;

  @ViewChild(MapInfoWindow, { static: false }) infoWindow: MapInfoWindow
  geolocation: string;
  locationMarker;
  zoom = 8;
  options: google.maps.MapOptions = {
    mapTypeId: 'terrain'
  }
  searchAddress = '';
  geoError: string;

  constructor(private geocodeService: GeocodeService
    , private ref: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.addMarker(this.latitude, this.longitude);
  }

  addMarker(latitude: number, longitude: number) {
    this.locationMarker = ({
      position: {
        lat: latitude,
        lng: longitude
      },
      options: { draggable: true },
    })

    this.getGeolocation(latitude, longitude);
  }



    markerChanged(event: google.maps.MouseEvent): void {
    this.addMarker(event.latLng.lat(), event.latLng.lng());
  }

  openInfoWindow(marker: MapMarker) {
    this.infoWindow.open(marker);
  }

  // If geolocation string is permanently held in the observation object then the geolocation step is redundant
  getGeolocation(latitude: number, longitude: number): void {
    this.geocodeService.reverseGeocode(latitude, longitude)
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

  //
    useGeolocation(searchValue: string) {
    this.geocodeService.geocodeAddress(searchValue)
      .subscribe((location: LocationViewModel) => {
        this.addMarker(location.latitude, location.longitude);
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
}
