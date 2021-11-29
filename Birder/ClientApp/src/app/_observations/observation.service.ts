import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { ObservationViewModel, ObservationAddDto, ObservationEditDto } from '@app/_models/ObservationViewModel';
import { ObservationCountService } from '@app/_services/observation-count.service';
import { ObservationTopService } from '@app/_info/observation-top.service';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class ObservationService {

  constructor(private readonly _httpClient: HttpClient
    , private readonly _service: ObservationCountService
    , private readonly _topService: ObservationTopService) { }

  getObservation(id: string): Observable<ObservationViewModel> {
    const options = id ?
      { params: new HttpParams().append('id', id.toString()) } : {};

    return this._httpClient.get<ObservationViewModel>('api/Observation/GetObservationDetail', options);
  }

  addObservation(viewModel: ObservationAddDto): Observable<ObservationViewModel> {
    return this._httpClient.post<ObservationViewModel>('api/Observation/CreateObservation', viewModel, httpOptions)
      .pipe(tap(() => { this._onObservationsChanged(); }));
  }

  updateObservation(id: number, viewModel: ObservationEditDto): Observable<ObservationViewModel> {
    const options = id ?
      { params: new HttpParams().set('id', id.toString()) } :
      { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };

    return this._httpClient.put<ObservationViewModel>('api/Observation/UpdateObservation', viewModel, options)
      .pipe(tap(() => { this._onObservationsChanged(); }));
  }

  deleteObservation(id: string): Observable<ObservationViewModel> {
    const options = id ?
      { params: new HttpParams().set('id', id.toString()) } : {};

    return this._httpClient.delete<ObservationViewModel>('api/Observation/DeleteObservation', options)
      .pipe(tap(() => { this._onObservationsChanged(); }));
  }

  private _onObservationsChanged(): void {
    this._service.getData();
    this._topService.getData();
  }
}
