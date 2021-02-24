import { Component, OnInit, ViewEncapsulation, OnDestroy } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { NetworkService } from '@app/_services/network.service';
import { UserNetworkDto } from '@app/_models/UserNetworkDto';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { Subscription } from 'rxjs';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';

@Component({
  selector: 'app-network',
  templateUrl: './network.component.html',
  styleUrls: ['./network.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class NetworkComponent implements OnInit, OnDestroy {
  network: UserNetworkDto;
  networkChangeSubscription: Subscription;
  tabstatus = {};
  active;
  
  constructor(private networkService: NetworkService) { }

    ngOnInit() {
      this.active = 1;
      this.tabstatus = {};
      
      this.getUserNetwork();
      this.networkChangeSubscription = this.networkService.networkChanged$
      .subscribe(_ => {
        this.onNetworkChanged();
      });
    }

    ngOnDestroy() {
      this.networkChangeSubscription.unsubscribe();
    }
  
    onNetworkChanged() {
      this.getUserNetwork();
    }
  
    getUserNetwork(): void {
      this.networkService.getUserNetwork()
        .subscribe(
          (data: UserNetworkDto) => {
            this.network = data;
          },
          (error: ErrorReportViewModel) => {
            console.log(error);
          });
    }

}
