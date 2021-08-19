import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpEventType } from '@angular/common/http';
// 
import { Router } from '@angular/router';
import { AccountManagerService } from '../account-manager.service';

@Component({
  selector: 'app-account-manager-avatar',
  templateUrl: './account-manager-avatar.component.html',
  styleUrls: ['./account-manager-avatar.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AccountManagerAvatarComponent implements OnInit {
  fileData: File = null;
  previewUrl: any = null;
  fileUploadProgress: string = null;
  uploadedFilePath: string = null;

  constructor(private accountManager: AccountManagerService
    , private router: Router) { }

  ngOnInit() {
  }

  fileProgress(fileInput: any) {
    this.fileData = <File>fileInput.target.files[0];
    this.showPreview();
  }

  showPreview() {
    const mimeType = this.fileData.type;
    if (mimeType.match(/image\/*/) == null) {
      return;
    }

    const reader = new FileReader();
    reader.readAsDataURL(this.fileData);
    reader.onload = (_event) => {
      this.previewUrl = reader.result;
    };
  }

  onSubmit() {
    const formData = new FormData();
    formData.append('file', this.fileData);

    this.fileUploadProgress = '0%';

    this.accountManager.postAvatar(formData)
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
          // this.toast.success('Please login again', 'Avatar successfully changed');
          this.router.navigate(['/login'], { queryParams: { returnUrl: '/account-manager-avatar' } });
        }
      },
        (error => {
          // this.toast.success(error.friendlyMessage, 'An error occurred');
        })
      );
  }
}
