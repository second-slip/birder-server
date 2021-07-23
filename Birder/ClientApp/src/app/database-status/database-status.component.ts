import { Component, ViewEncapsulation } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, share } from 'rxjs/operators';
import { DatabaseService } from './database.service';

@Component({
  selector: 'app-database-status',
  templateUrl: './database-status.component.html',
  styleUrls: ['./database-status.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class DatabaseStatusComponent {
  databaseStatus$: Observable<boolean>;
  public errorObject = null;

  constructor(private service: DatabaseService) {
    this.databaseStatus$ = this.getDatabaseStatus();
  }

  private getDatabaseStatus(): Observable<boolean> {
    return this.service.getWakeUpDatabase()
      .pipe(share(),
        catchError(err => {
          this.errorObject = err;
          return throwError(err);
        }));
  }
}
