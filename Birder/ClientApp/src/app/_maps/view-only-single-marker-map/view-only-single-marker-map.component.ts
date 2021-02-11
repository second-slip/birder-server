import { Component, Input, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MapInfoWindow, MapMarker } from '@angular/google-maps';

@Component({
  selector: 'app-view-only-single-marker-map',
  templateUrl: './view-only-single-marker-map.component.html',
  styleUrls: ['./view-only-single-marker-map.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ViewOnlySingleMarkerMapComponent implements OnInit {
  @Input() latitude: number;
  @Input() longitude: number;
  @Input() address: string;

  @ViewChild(MapInfoWindow, { static: false }) infoWindow: MapInfoWindow
  locationMarker;
  zoom = 8;
  options: google.maps.MapOptions = {
    mapTypeId: 'terrain'
  }

  constructor() { }

  ngOnInit(): void {
    this.addMarker(this.latitude, this.longitude, false);
  }

  addMarker(latitude: number, longitude: number, getAddress: boolean) {
    this.locationMarker = ({
      position: {
        lat: latitude,
        lng: longitude
      },
      options: { animation: google.maps.Animation.BOUNCE },
    })
  }

  openInfoWindow(marker: MapMarker) {
    this.infoWindow.open(marker);
  }
}
