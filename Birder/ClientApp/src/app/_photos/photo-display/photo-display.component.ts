import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-photo-display',
  templateUrl: './photo-display.component.html',
  styleUrls: ['./photo-display.component.scss']
})
export class PhotoDisplayComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}

  // private _album: Array<PhotographAlbum> = [];

    // getPhotos(id: number): void {
  //   this.photosService.getPhotos(id)
  //     .subscribe(
  //       (result: any) => {
  //         this._album = result.map((photo): PhotographAlbum => ({
  //           src: photo.address,
  //           caption: this.observation.bird.englishName,
  //           thumb: photo.address,
  //           filename: photo.filename
  //         }));
  //       },
  //       () => {
  //         // this.errorReport = error;
  //         // this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
  //       });
  // }
