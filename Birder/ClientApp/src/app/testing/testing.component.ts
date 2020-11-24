import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { GeocodingService } from '@app/_services/geocoding.service';

@Component({
  selector: 'app-testing',
  templateUrl: './testing.component.html',
  styleUrls: ['./testing.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TestingComponent implements OnInit {
  formattedAddress: string;
  formattedAddress2: string;

  constructor(private geo: GeocodingService) { }

  ngOnInit() {
    this.findLocation();
    this.findLocation2();
  }

  findLocation(): void {
    this.geo.geocode(`83 christchurch road, reading`)
      .subscribe(
        (response: any) => {
          console.log(response.results[0]);
          this.formattedAddress = response.results[0].formatted_address;
          // coordinates
        }),
      ((error: any) => {
        console.log(error);
      });
  }

  findLocation2(): void {
    this.geo.reverseGeocode(40.714224, -73.961452)
      .subscribe(
        (response: any) => {
          console.log(response.results[0]);
          this.formattedAddress2 = response.results[0].formatted_address;
        }),
      ((error: any) => {
        console.log(error);
      });
  }
}
