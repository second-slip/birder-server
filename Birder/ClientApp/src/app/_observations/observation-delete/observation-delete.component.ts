import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
// 
import { TokenService } from '@app/_services/token.service';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { ObservationService } from '@app/_observations/observation.service';
import { Observable, throwError } from 'rxjs';
import { catchError, share, tap } from 'rxjs/operators';

@Component({
  selector: 'app-observation-delete',
  templateUrl: './observation-delete.component.html',
  styleUrls: ['./observation-delete.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ObservationDeleteComponent {
  public errorObject = null;
  observation$: Observable<ObservationViewModel>;

  constructor(private observationService: ObservationService
    , private tokenService: TokenService
    , private route: ActivatedRoute
    , private router: Router) {

    this.getObservation(this.route.snapshot.paramMap.get('id'));
  }

  getObservation(id: string): void {

    this.observation$ = this.observationService.getObservation(id)
      .pipe(share(),
        tap(resp => this.checkIsOwner(resp.user.userName)),
        catchError(err => {
          this.errorObject = err;
          return throwError(err);
        })
      );
  }

  checkIsOwner(username: string): void {
    if (this.tokenService.checkIsRecordOwner(username) === false) {
      // this.toast.error(`Only the owner can delete their observation report`, `Not allowed`);
      this.router.navigate(['/observation-feed']);
      return;
    }
  }

  deleteObservation(id: string): void {

    this.observationService.deleteObservation(id)
      .subscribe(_=> {
          // this.toast.success(`You have successfully deleted your observation`, `Successfully deleted`);
          this.router.navigate(['/observation-feed']);
        }),
        catchError(err => {
          this.errorObject = err;
          // this.toast.error(`An error occurred deleing the observation report`, `Unsuccessful`);
          return throwError(err);
        });
  }
}
