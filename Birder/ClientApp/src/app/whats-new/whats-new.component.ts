import { HttpClient } from '@angular/common/http';
import { Component, ViewEncapsulation } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, share } from 'rxjs/operators';
import { FeaturesService } from './features.service';

@Component({
  selector: 'app-whats-new',
  templateUrl: './whats-new.component.html',
  styleUrls: ['./whats-new.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class WhatsNewComponent {
  features$: Observable<IFeatures[]>;
  public errorObject = null;

  constructor(private service: FeaturesService) {
    this.features$ = this.getFeatures();
  }

  private getFeatures(): Observable<IFeatures[]> {
    return this.service.getFeatures()
      .pipe(share(),
        catchError(err => {
          this.errorObject = err;
          return throwError(err);
        }));
  }
}


export interface IFeatures {
  id: number;
  feature: string;
  description: string;
  progress: string;
  priority: string;
  colourCode: string;
}
