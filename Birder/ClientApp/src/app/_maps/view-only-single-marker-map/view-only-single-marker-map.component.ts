import { ChangeDetectorRef, Component, Input, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MapInfoWindow, MapMarker } from '@angular/google-maps';
import { GeocodingService } from '@app/_services/geocoding.service';

@Component({
  selector: 'app-view-only-single-marker-map',
  templateUrl: './view-only-single-marker-map.component.html',
  styleUrls: ['./view-only-single-marker-map.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ViewOnlySingleMarkerMapComponent implements OnInit {
  @Input() latitude: number;
  @Input() longitude: number;

  @ViewChild(MapInfoWindow, { static: false }) infoWindow: MapInfoWindow
  geolocation: string;
  locationMarker;
  zoom = 8;
  options: google.maps.MapOptions = {
    mapTypeId: 'terrain'
  }

  constructor(private geocoding: GeocodingService
    , private ref: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.addMarker(this.latitude, this.longitude, true);
  }

  addMarker(latitude: number, longitude: number, getAddress: boolean) {
    this.locationMarker = ({
      position: {
        lat: latitude,
        lng: longitude
      },
      options: { animation: google.maps.Animation.BOUNCE },
    })

    if (getAddress) { // If geolocation string is permanently held in the observation object then the geolocation step is redundant
      this.getFormattedAddress(latitude, longitude);
    }
  }

  openInfoWindow(marker: MapMarker) {
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
          //
        }
      );
  }
}
