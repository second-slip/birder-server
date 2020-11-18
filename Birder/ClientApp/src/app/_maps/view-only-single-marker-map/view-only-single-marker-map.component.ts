import { ChangeDetectorRef, Component, Input, OnInit, ViewChild } from '@angular/core';
import { MapInfoWindow, MapMarker } from '@angular/google-maps';
import { LocationViewModel } from '@app/_models/LocationViewModel';
import { GeocodeService } from '@app/_services/geocode.service';

// export interface ChangePasswordViewModel {
//   latitude: number;
//   longitude: number;
//   geolocation: string;
// }

@Component({
  selector: 'app-view-only-single-marker-map',
  templateUrl: './view-only-single-marker-map.component.html',
  styleUrls: ['./view-only-single-marker-map.component.scss']
})
export class ViewOnlySingleMarkerMapComponent implements OnInit {
  @Input() latitude: number;
  @Input() longitude: number;

  geolocation = 'location';

  @ViewChild(MapInfoWindow, { static: false }) infoWindow: MapInfoWindow
  locationMarker;
  zoom = 8;
  options: google.maps.MapOptions = {
    mapTypeId: 'terrain'
  }

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
      // label: {
      //   color: 'red',
      //   text: 'Marker label',
      // },
      // title: 'Marker title',
      options: { animation: google.maps.Animation.BOUNCE },
    })

    this.getGeolocation(latitude, longitude);
  }

  openInfoWindow(marker: MapMarker) {
    this.infoWindow.open(marker);
  }

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
}
