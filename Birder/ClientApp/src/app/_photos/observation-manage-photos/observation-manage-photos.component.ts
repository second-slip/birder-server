import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpEventType } from '@angular/common/http';
import { PhotosService } from '@app/_photos/photos.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ObservationService } from '@app/_observations/observation.service';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
// 
// import { Lightbox } from 'ngx-lightbox';
import { PhotographAlbum } from '@app/_models/PhotographAlbum';
import { TokenService } from '@app/_services/token.service';

@Component({
  selector: 'app-observation-manage-photos',
  templateUrl: './observation-manage-photos.component.html',
  styleUrls: ['./observation-manage-photos.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ObservationManagePhotosComponent implements OnInit {
  files: File[] = [];
  fileUploadProgress: string = null;
  observation: ObservationViewModel;
  errorReport: any;
  private _album: Array<PhotographAlbum> = [];
  // images = [1, 2, 3, 4, 5, 6, 7].map(() => `https://picsum.photos/900/500?random&t=${Math.random()}`);

  constructor(private router: Router
    , private route: ActivatedRoute
    // , private _lightbox: Lightbox
    , private tokenService: TokenService
    , private observationService: ObservationService
    , private photosService: PhotosService) {
  }

  ngOnInit() {
    this.getObservation();
  }

  // goBack()

  onSelect(event): void {
    console.log(event);
    this.files.push(...event.addedFiles);
  }

  onRemove(event): void {
    console.log(event);
    this.files.splice(this.files.indexOf(event), 1);
  }

  onSavePhotos(): void {
    const formData = new FormData();
    this.files.forEach((file) => { formData.append('files', file); });
    formData.append('observationId', this.observation.observationId.toString());

    this.photosService.postPhotos(formData)
      .subscribe(events => {
        if (events.type === HttpEventType.UploadProgress) {
          this.fileUploadProgress = Math.round(events.loaded / events.total * 100) + '%';
        } else if (events.type === HttpEventType.Response) {
          // this.toast.success('Success', 'New photographs were uploaded');
          this.fileUploadProgress = '';
          this.files = [];
          this._album = [];
          this.getPhotos(this.observation.observationId);
        }
      },
        (error: any) => {
          // this.toast.error(error.friendlyMessage, 'An error occurred');
        }
      );
  }

  getObservation(): void {
    const id = this.route.snapshot.paramMap.get('id');

    this.observationService.getObservation(id)
      .subscribe(
        (observation: ObservationViewModel) => {
          this.observation = observation;
          if (this.tokenService.checkIsRecordOwner(observation.user.userName) === false) {
            // this.toast.error(`Only the observation owner can edit their report`, `Not allowed`);
            this.router.navigate(['/observation-feed']);
            return;
          }
          this.getPhotos(observation.observationId);
        },
        (error: any) => {
          this.errorReport = error;
          this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
        });
  }

  onDeletePhoto(filename: string): void {
    const formData = new FormData();
    formData.append('observationId', this.observation.observationId.toString());
    formData.append('filename', filename);

    this.photosService.postDeletePhoto(formData)
      .subscribe(_ => {
        // this.toast.success('Success', 'Photo was deleted');
        this._album = [];
        this.getPhotos(this.observation.observationId);
      },
        (error: any) => {
          this.errorReport = error;
        });
  }

  // open(index: number): void {
  //   // open lightbox
  //   this._lightbox.open(this._album, index);
  // }

  // close(): void {
  //   // close lightbox programmatically
  //   this._lightbox.close();
  // }

  getPhotos(id: number): void {
    this.photosService.getPhotos(id)
      .subscribe(
        (result: any) => {
          this._album = result.map((photo): PhotographAlbum => ({
            src: photo.address,
            caption: this.observation.bird.englishName,
            thumb: photo.address,
            filename: photo.filename
          }));
        },
        (error: any) => {
          this.errorReport = error;
          // this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
        });
  }
}

