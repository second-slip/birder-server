import { Component, OnInit } from '@angular/core';
import { HttpEventType } from '@angular/common/http';
import { AccountManagerService } from '@app/_services/account-manager.service';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-account-manager-avatar',
  templateUrl: './account-manager-avatar.component.html',
  styleUrls: ['./account-manager-avatar.component.scss']
})
export class AccountManagerAvatarComponent implements OnInit {
  fileData: File = null;
  previewUrl: any = null;
  fileUploadProgress: string = null;
  uploadedFilePath: string = null;

  constructor(private accountManager: AccountManagerService
    , private toast: ToastrService
    , private router: Router) { }

  ngOnInit() {
  }

  fileProgress(fileInput: any) {
    this.fileData = <File>fileInput.target.files[0];
    this.showPreview();
  }

  showPreview() {
    // Show preview
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
          this.toast.success('Please login again', 'Avatar successfully changed');
          this.router.navigate(['/login'], { queryParams: { returnUrl: '/account-manager-avatar' } });
        }
      },
        (error: ErrorReportViewModel) => {
          this.toast.success(error.friendlyMessage, 'An error occurred');
        }
      );
  }
}
