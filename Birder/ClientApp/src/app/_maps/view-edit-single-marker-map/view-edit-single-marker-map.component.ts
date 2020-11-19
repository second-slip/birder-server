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
}
