import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';

@Injectable({
  providedIn: 'root'
})
export class GeocodingService {
  private readonly apiUrl = 'https://maps.google.com/maps/api/geocode/json?';

  constructor(private http: HttpClient) { }

  geocode(searchTerm: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}address=${encodeURIComponent(searchTerm)}&key=${environment.mapKey}`)
      .pipe(
      )
  }

    // reverseGeocode(): Observable<any> {
  // }

  // Shared

  // private formatSearchTerm(searchTerm: string): string {
  //   return searchTerm.split(' ').join('+');
  // }
}

  // Alternative which returns a Promise
  // geocode(term: string): Promise<any> {
  //   return this.http.get<any>(`https://maps.google.com/maps/api/geocode/json?address=${this.formatSearchTerm(term)}&key=AIzaSyD4IghqI4x7Sld9KP3sP6FtbN7wCPGySmY`)
  //     .toPromise()
  //     .then((response) => Promise.resolve(response))
  //     .catch((error) => Promise.resolve(error.json()));
  // }

  // Called like this...
  // findLocation(): void {
  //   this.geo.getLocation(`Whiteknights campus, uk`)
  //   .then((response) => { console.log(response.results[0]) }) //{ this.result = response.results[0] })
  //   .catch((error) => console.error(error));