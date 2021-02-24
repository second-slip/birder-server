import { Component, ViewEncapsulation } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';
import { NetworkService } from '@app/_services/network.service';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';

@Component({
  selector: 'app-network-search',
  templateUrl: './network-search.component.html',
  styleUrls: ['./network-search.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class NetworkSearchComponent {
  users: NetworkUserViewModel[];
  // customSearch = false;
  searching = false;

  constructor(private networkService: NetworkService
    , private toast: ToastrService) { }

  searchNetwork(value: any): void {
    // this.customSearch = true;
    this.searching = true;

    this.networkService.getSearchNetwork(value.searchTerm)
      .subscribe(
        (data: NetworkUserViewModel[]) => {
          this.users = data;
          if (this.users.length > 0) {
            this.toast.info(this.users.length.toString() + ' results were found', 'Search successful');
          } else {
            this.toast.warning('No results were found', 'Search unsuccessful');
          }
          this.searching = false;
        },
        (error: ErrorReportViewModel) => {
          if (error.type === 'client-side or network error occurred') {
            this.toast.error(error.serverCustomMessage, 'An error occurred');
          } else {
            this.toast.error('Try a different search query', 'Search unsuccessful');
          }
          this.searching = false;
        });
  }
}
