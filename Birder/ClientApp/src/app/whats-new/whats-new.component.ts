import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FeaturesService } from './features.service';

@Component({
  selector: 'app-whats-new',
  templateUrl: './whats-new.component.html',
  styleUrls: ['./whats-new.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class WhatsNewComponent implements OnInit {
  // features$: Observable<IFeatures[]>;
  // private ngUnsubscribe = new Subject();
  // public features: Subject<Array<IFeatures>> = new Subject();
  public errorObject = null;

  constructor(readonly _service: FeaturesService) {
    // this.features$ = this._getFeatures();
    // this._getFeatures();
  }

  ngOnInit(): void {
    this._getData();
  }

  private _getData(): void {
    this._service.fetchList();
  }

  // private async _getFeatures(): Promise<void> {
  //   this._service.getFeatures()
  //     .pipe(take(1),takeUntil(this.ngUnsubscribe),
  //       catchError(err => {
  //         this.errorObject = err;
  //         return throwError(err);
  //       })
  //     ).subscribe(result => { this.features.next(result) });
  // }

  // ngOnDestroy() {
  //   this.ngUnsubscribe.next();
  //   this.ngUnsubscribe.complete();
  // }



  // private _getFeatures(): Observable<IFeatures[]> {
  //   return this._service.getFeatures()
  //     .pipe(share(),
  //       catchError(err => {
  //         this.errorObject = err;
  //         return throwError(err);
  //       }));
  // }
}
