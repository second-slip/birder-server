import { Component, OnInit } from '@angular/core';
import { AccountManagerService } from '../account-manager.service';
import { ToastrService } from 'ngx-toastr';
import { ManageProfileViewModel } from 'src/_models/ManageProfileViewModel';
import { ErrorReportViewModel } from 'src/_models/ErrorReportViewModel';
import { Router } from '@angular/router';

@Component({
  selector: 'app-account-manage',
  templateUrl: './account-manage.component.html',
  styleUrls: ['./account-manage.component.scss']
})
export class AccountManageComponent implements OnInit {
  user: ManageProfileViewModel;

  constructor(private toast: ToastrService
            , private router: Router
            , private accountManager: AccountManagerService) { }

  ngOnInit() {
  }

  getUserProfile() {
    this.accountManager.getUserProfile()
    .subscribe(
      (data: ManageProfileViewModel) => {
        this.user = data;
      },
      (error: ErrorReportViewModel) => {
        // console.log(error);
        this.toast.error(error.serverCustomMessage, 'An error occurred');
        // this.router.navigate(['/']);
      });
  }

}
