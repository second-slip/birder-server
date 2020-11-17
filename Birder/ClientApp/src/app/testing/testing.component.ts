import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-testing',
  templateUrl: './testing.component.html',
  styleUrls: ['./testing.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TestingComponent implements OnInit {
  

  constructor(private http: HttpClient) { }

  ngOnInit() {
    // this.findLocation();
   }

//   getLocation(term: string):Promise<any> {
//     return this.http.get('http://maps.google.com/maps/api/geocode/json?address=' + term + 'CA&sensor=false')
//          .toPromise()
//          .then((response) => Promise.resolve(response.json()))
//          .catch((error) => Promise.resolve(error.json()));
//  }

result;

// defaultLocationLatitude: 54.972237,
// defaultLocationLongitude: -2.4608560000000352,
findLocation(): void {
  this.getLocation(`Whiteknights campus`)
    .subscribe(
      (data: any) => (
        (this.result = data.results[0].geometry.location),
        console.log(data.results[0].geometry.location)
        // (this.latitude = data.results[0].geometry.location.lat),
        // (this.longitude = data.results[0].geometry.location.lng),
        // this.map.setCenter(
        //   new google.maps.LatLng(this.latitude, this.longitude)
        // ),
        // this.addMarker()
      ),
      (err: any) => console.log(err),
      () => console.log("All done getting location.")
    );
}

  getLocation(term: string): Observable<any> {
    const API_KEY = "AIzaSyD4IghqI4x7Sld9KP3sP6FtbN7wCPGySmY"; // replace with environment variable
    return this.http.get(
      `$https://maps.google.com/maps/api/geocode/json?address=1600+Amphitheatre+Parkway,
      &key=${API_KEY}`
    ).pipe(
      catchError(async (error) => console.log(error)));
      // catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }
}
