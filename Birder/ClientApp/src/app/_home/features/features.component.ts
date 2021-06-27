import { Component, ViewEncapsulation } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { DatabaseService } from '../database.service';

@Component({
  selector: 'app-features',
  templateUrl: './features.component.html',
  styleUrls: ['./features.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class FeaturesComponent {
  private ngUnsubscribe = new Subject();

  constructor(private service: DatabaseService) {
    this.wakeUpDatabase();
  }

  private wakeUpDatabase() {
    this.service.wakeyWakey()
      .pipe(
        takeUntil(this.ngUnsubscribe)
      )
      .subscribe(_ => {
        // console.log('success');
      },
        (error => {
          // toast error notification
          console.log(error);
        }));
  }

  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
    console.log('unsubscribe');
  }
}
