import { Injectable } from '@angular/core';
import { MapsAPILoader } from '@agm/core';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { of } from 'rxjs/observable/of';
import { filter, catchError, tap, map, switchMap } from 'rxjs/operators';
import { fromPromise } from 'rxjs/observable/fromPromise';
import { LocationViewModel } from '../_models/LocationViewModel';
import { ObserversModule } from '@angular/cdk/observers';
import { analyzeAndValidateNgModules } from '@angular/compiler';

declare var google: any;

@Injectable()
export class GeocodeService {
  private geocoder: any;

  constructor(private mapLoader: MapsAPILoader) {}

  private initGeocoder() {
    console.log('Init geocoder!');
    this.geocoder = new google.maps.Geocoder();
  }

  private waitForMapsToLoad(): Observable<boolean> {
    if (!this.geocoder) {
      return fromPromise(this.mapLoader.load())
      .pipe(
        tap(() => this.initGeocoder()),
        map(() => true)
      );
    }
    return of(true);
  }
  // reverseGeocode(model: LocationViewModel): Observable<LocationViewModel> {
  reverseGeocode(latitude: number, longitude: number): Observable<LocationViewModel> {
    const latlng = { lat: latitude, lng: longitude };

    console.log('Start geocoding!');
    return this.waitForMapsToLoad().pipe(
      switchMap(() => {
        return new Observable<LocationViewModel>(observer => {
          this.geocoder.geocode({ 'location': latlng }, function (results, status) {
            if (status === google.maps.GeocoderStatus.OK) {
              console.log('Geocoding complete!');
              observer.next({
                latitude: results[0].geometry.location.lat(),
                longitude: results[0].geometry.location.lng(),
                formattedAddress: results[0].formatted_address
              });
            } else {
              console.log('Error - ', results, ' & Status - ', status);
              observer.next({ latitude: 0, longitude: 0, formattedAddress: '' });
            }
            observer.complete();
          });
        });
      })
    );
  }

  geocodeAddress(location: string): Observable<LocationViewModel> {
    console.log('Start geocoding!');
    return this.waitForMapsToLoad().pipe(
      switchMap(() => {
        return new Observable<LocationViewModel>(observer => {
          this.geocoder.geocode({'address': location}, (results, status) => {
            if (status === google.maps.GeocoderStatus.OK) {
              console.log('Geocoding complete!');
              observer.next({
                latitude: results[0].geometry.location.lat(),
                longitude: results[0].geometry.location.lng(),
                formattedAddress: results[0].formatted_address
              });
            } else {
                console.log('Error - ', results, ' & Status - ', status);
                observer.next({ latitude: 0, longitude: 0, formattedAddress: '' });
            }
            observer.complete();
          });
        });
      })
    );
  }
}
