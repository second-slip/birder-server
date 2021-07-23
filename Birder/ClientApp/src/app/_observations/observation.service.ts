import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { ObservationViewModel, ObservationAddDto, ObservationEditDto } from '@app/_models/ObservationViewModel';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class ObservationService {
  private observationsChanged = new Subject<any>();
  observationsChanged$ = this.observationsChanged.asObservable();

  constructor(private http: HttpClient) { }

  getObservation(id: string): Observable<ObservationViewModel> {
    const options = id ?
      { params: new HttpParams().append('id', id.toString()) } : {};

    return this.http.get<ObservationViewModel>('api/Observation/GetObservationDetail', options);
  }

  addObservation(viewModel: ObservationAddDto): Observable<ObservationViewModel> {
    return this.http.post<ObservationViewModel>('api/Observation/CreateObservation', viewModel, httpOptions)
      .pipe(tap(() => { this.announceObservationsChanged(); }));
  }

  updateObservation(id: number, viewModel: ObservationEditDto): Observable<ObservationViewModel> {
    const options = id ?
      { params: new HttpParams().set('id', id.toString()) } :
      { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };

    return this.http.put<ObservationViewModel>('api/Observation/UpdateObservation', viewModel, options)
      .pipe(tap(() => { this.announceObservationsChanged(); }));
  }

  deleteObservation(id: string): Observable<ObservationViewModel> {
    const options = id ?
      { params: new HttpParams().set('id', id.toString()) } : {};

    return this.http.delete<ObservationViewModel>('api/Observation/DeleteObservation', options)
      .pipe(tap(() => { this.announceObservationsChanged(); }));
  }

  private announceObservationsChanged(): void {
    this.observationsChanged.next();
  }
}
