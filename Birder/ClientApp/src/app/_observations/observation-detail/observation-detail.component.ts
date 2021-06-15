import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { ObservationService } from '@app/_observations/observation.service';
import { UserViewModel } from '@app/_models/UserViewModel';
import { TokenService } from '@app/_services/token.service';
import { Observable, throwError } from 'rxjs';
import { catchError, share } from 'rxjs/operators';

/*  ******** information ********
  child view is accessed via the #map local variable.  This is to access 'geolocation' property.
  Local varaible binding is only suitable for simple things like this...
*/

@Component({
  selector: 'app-observation-detail',
  templateUrl: './observation-detail.component.html',
  styleUrls: ['./observation-detail.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ObservationDetailComponent {
  user: UserViewModel;
  public errorObject = null;
  observation$: Observable<ObservationViewModel>;


  constructor(private observationService: ObservationService
    , private tokenService: TokenService
    , private route: ActivatedRoute) {
    this.user = this.tokenService.getAuthenticatedUserDetails();
    this.getObservation(this.route.snapshot.paramMap.get('id'));
  }

  getObservation(id: string): void {

    this.observation$ = this.observationService.getObservation(id)
      .pipe(share(),
        catchError(err => {
          this.errorObject = err;
          return throwError(err);
        })
      );
  }
}
