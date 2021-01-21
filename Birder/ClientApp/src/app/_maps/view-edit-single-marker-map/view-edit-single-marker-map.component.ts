import { ChangeDetectorRef, Component, Input, OnInit, ViewChild } from '@angular/core';
import { MapInfoWindow, MapMarker } from '@angular/google-maps';
import { GeocodingService } from '@app/_services/geocoding.service';

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

  constructor(private geocoding: GeocodingService
    , private ref: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.addMarker(this.latitude, this.longitude, true);
  }

  addMarker(latitude: number, longitude: number, getAddress: boolean): void {
    this.locationMarker = ({
      position: {
        lat: latitude,
        lng: longitude
      },
      options: { draggable: true },
    })

    if (getAddress) {
      this.getFormattedAddress(latitude, longitude);
    }
  }

  markerChanged(event: google.maps.MouseEvent): void {
    this.addMarker(event.latLng.lat(), event.latLng.lng(), true);
  }

  openInfoWindow(marker: MapMarker): void {
    this.infoWindow.open(marker);
  }

  getFormattedAddress(latitude: number, longitude: number): void {
    this.geocoding.reverseGeocode(latitude, longitude)
      .subscribe(
        (response: any) => {
          this.geolocation = response.results[0].formatted_address;
          this.ref.detectChanges();
        },
        (error: any) => {
        }
      );
  }

  findAddress(searchValue: string): void {
    this.geocoding.geocode(searchValue)
      .subscribe(
        (response: any) => {
          this.changeZoomLevel(15);
          this.addMarker(response.results[0].geometry.location.lat, response.results[0].geometry.location.lng, false); // false to stop second hit on API to get address...
          this.geolocation = response.results[0].formatted_address;
          this.searchAddress = '';
          this.ref.detectChanges();
        }
      );
  }

  changeZoomLevel(level: number): void {
    this.zoom = level;
  }

  closeAlert(): void {
    this.geoError = null;
  }

  getCurrentPosition(): void {
    if (window.navigator.geolocation) {
      window.navigator.geolocation.getCurrentPosition(
        (position) => {
          this.changeZoomLevel(15);
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
}
