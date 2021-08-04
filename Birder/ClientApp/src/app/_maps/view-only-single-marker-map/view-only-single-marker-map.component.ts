import { Component, Input, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MapInfoWindow, MapMarker } from '@angular/google-maps';
import { ObservationPosition } from '@app/_models/ObservationPosition';

@Component({
  selector: 'app-view-only-single-marker-map',
  templateUrl: './view-only-single-marker-map.component.html',
  styleUrls: ['./view-only-single-marker-map.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ViewOnlySingleMarkerMapComponent implements OnInit {
  @Input() position: ObservationPosition;
  @ViewChild(MapInfoWindow, { static: false }) infoWindow: MapInfoWindow;

  public errorObject = null;
  public locationMarker: any;
  public options: google.maps.MapOptions = {
    mapTypeId: 'terrain', zoom: 8,
  }

  constructor() { }

  ngOnInit(): void {
    this.addMarker(this.position.latitude, this.position.longitude);
  }

  addMarker(latitude: number, longitude: number): void {
    try {
      this.locationMarker = ({
        position: {
          lat: latitude,
          lng: longitude
        },
        options: { animation: google.maps.Animation.BOUNCE },
      })
    } catch (error) {
      this.errorObject = error;
    }
  }

  openInfoWindow(marker: MapMarker): void {
    this.infoWindow.open(marker);
  }
}
