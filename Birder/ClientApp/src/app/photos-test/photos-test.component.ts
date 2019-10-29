import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpHeaders, HttpClient, HttpParams, HttpEventType } from '@angular/common/http';
import { PhotosService } from '@app/_services/photos.service';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { Router, ActivatedRoute } from '@angular/router';
import { ObservationService } from '@app/_services/observation.service';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { ToastrService } from 'ngx-toastr';
import { UploadPhotosDto } from '@app/_models/UploadPhotosDto';

@Component({
  selector: 'app-photos-test',
  templateUrl: './photos-test.component.html',
  styleUrls: ['./photos-test.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PhotosTestComponent implements OnInit {
  files: File[] = [];
  fileUploadProgress: string = null;
  observation: ObservationViewModel;
  errorReport: ErrorReportViewModel;

  constructor(private router: Router
    , private route: ActivatedRoute
    , private observationService: ObservationService
    , private photosService: PhotosService
    , private toast: ToastrService) { }

  ngOnInit() {
    this.getObservation();
  }

  onSelect(event): void {
    console.log(event);
    this.files.push(...event.addedFiles);

  }

  onRemove(event): void {
    console.log(event);
    this.files.splice(this.files.indexOf(event), 1);
  }

  onSavePhotos(): void {

    const dto = <UploadPhotosDto>{
      observationId: this.observation.observationId,
      files: this.files
    };

    console.log(dto);

    this.photosService.postPhotos(dto)
    .subscribe(events => {
      if (events.type === HttpEventType.UploadProgress) {
        this.fileUploadProgress = Math.round(events.loaded / events.total * 100) + '%';
        // console.log(this.fileUploadProgress);
      } else if (events.type === HttpEventType.Response) {
        this.fileUploadProgress = '';
        //
        // console.log(events.body);
        // alert('SUCCESS !!');
        //
        this.toast.success('Please login again', 'Avatar successfully changed');
        // this.router.navigate(['/login'], { queryParams: { returnUrl: '/account-manager-avatar' } });
      }
    },
      (error: ErrorReportViewModel) => {
        this.toast.success(error.friendlyMessage, 'An error occurred');
      }
    );

  }

  getObservation(): void {
    // const id = +this.route.snapshot.paramMap.get('id');
    const id = 72;

    this.observationService.getObservation(id)
      .subscribe(
        (observation: ObservationViewModel) => {
          this.observation = observation;
        },
        (error: ErrorReportViewModel) => {
          this.errorReport = error;
          // this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
        });
  }

}
