import { Component } from '@angular/core';
import { DatabaseService } from '@app/_home/database.service';
import { Observable, throwError } from 'rxjs';
import { catchError, share } from 'rxjs/operators';

@Component({
  selector: 'app-database-status',
  templateUrl: './database-status.component.html',
  styleUrls: ['./database-status.component.scss']
})
export class DatabaseStatusComponent {

  databaseStatus$: Observable<boolean>;
  public errorObject = null;

  constructor(private service: DatabaseService) { 
    this.databaseStatus$ = this.getDatabaseStatus();

  }

  private getDatabaseStatus(): Observable<boolean> {
    return this.service.wakeyWakey()
      .pipe(share(),
        catchError(err => {
          this.errorObject = err;
          return throwError(err);
        }));
  }

  

}
