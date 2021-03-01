import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BirdDetailViewModel } from '@app/_models/BirdDetailViewModel';
import { BirdsService } from '@app/_services/birds.service';
import { Observable, throwError } from 'rxjs';
import { catchError, share } from 'rxjs/operators';

@Component({
  selector: 'app-bird-detail',
  templateUrl: './bird-detail.component.html',
  styleUrls: ['./bird-detail.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class BirdDetailComponent {
  bird$: Observable<BirdDetailViewModel>;
  public errorObject = null;

  constructor(private birdsService: BirdsService, private route: ActivatedRoute) {
    route.params.subscribe(_ => {
      this.route.paramMap.subscribe(params => this.getBird(params.get('id')));
    });
  }

  getBird(id: string): void {
    this.bird$ = this.birdsService.getBird(id)
      .pipe(share(),
        catchError(err => {
          this.errorObject = err;
          return throwError(err);
        }));
  }
}
