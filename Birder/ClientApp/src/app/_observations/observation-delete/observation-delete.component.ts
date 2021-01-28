import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TokenService } from '@app/_services/token.service';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { ObservationService } from '@app/_sharedServices/observation.service';

@Component({
  selector: 'app-observation-delete',
  templateUrl: './observation-delete.component.html',
  styleUrls: ['./observation-delete.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ObservationDeleteComponent implements OnInit {
  observation: ObservationViewModel;
  errorReport: ErrorReportViewModel;

  constructor(private observationService: ObservationService
    , private toast: ToastrService
    , private tokenService: TokenService
    , private route: ActivatedRoute
    , private router: Router) { }

  ngOnInit(): void {
    this.getObservation();
  }

  getObservation(): void {
    const id = +this.route.snapshot.paramMap.get('id');

    this.observationService.getObservation(id)
      .subscribe(
        (observation: ObservationViewModel) => {
          this.observation = observation;
          if (this.tokenService.checkIsRecordOwner(observation.user.userName) === false) {
            this.toast.error(`Only the observation owner can delete their report`, `Not allowed`);
            this.router.navigate(['/observation-feed']);
            return;
          }
        },
        (error: ErrorReportViewModel) => {
          this.errorReport = error;
        });
  }

  deleteObservation(): void {
    this.observationService.deleteObservation(this.observation.observationId)
      .subscribe(_ => {
        this.toast.success(`You have successfully deleted your observation`, `Successfully deleted`);
        this.router.navigate(['/observation-feed']);
      },
        (error: ErrorReportViewModel) => {
          this.toast.error(`An error occurred deleing the observation report`, `Unsuccessful`);
          this.errorReport = error;
        });
  }
}
