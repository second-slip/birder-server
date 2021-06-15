import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { PhotographDto } from '@app/_models/PhotographDto';

@Injectable({
  providedIn: 'root'
})
export class PhotosService {

  constructor(private http: HttpClient) { }

  getPhotos(observationId: number): Observable<PhotographDto[]> {
    const options = observationId ? { params: new HttpParams().append('observationId', observationId.toString()) } : {};

    return this.http.get<any>('api/Photograph/GetPhotographs', options)
      .pipe();
  }

  postDeletePhoto(model: FormData): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Disposition': 'multipart/form-data' });

    return this.http.post('api/Photograph/DeletePhotograph', model, { headers: headers })
      .pipe();
  }

  postPhotos(model: FormData): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Disposition': 'multipart/form-data' });

    return this.http.post('api/Photograph', model, { headers: headers, observe: 'events', reportProgress: true })
      .pipe();
  }
}
