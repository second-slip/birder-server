import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';
import { NetworkService } from '@app/_services/network.service';
import { ToastrService } from 'ngx-toastr';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';

@Component({
  selector: 'app-network-suggestions',
  templateUrl: './network-suggestions.component.html',
  styleUrls: ['./network-suggestions.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class NetworkSuggestionsComponent implements OnInit {
  users: NetworkUserViewModel[];

  constructor(private networkService: NetworkService
    , private toast: ToastrService) { }

  ngOnInit() {
    this.getNetwork();
  }

  getNetwork(): void {
    this.networkService.getNetworkSuggestions()
      .subscribe(
        (data: NetworkUserViewModel[]) => {
          this.users = data;
        },
        (error: ErrorReportViewModel) => {
          this.toast.error(error.serverCustomMessage, 'An error occurred');
        });
  }
}
