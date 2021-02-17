import { Component, OnInit, ChangeDetectorRef, ViewEncapsulation, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { SetLocationViewModel } from '@app/_models/SetLocationViewModel';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { UserViewModel } from '@app/_models/UserViewModel';
import { TokenService } from '@app/_services/token.service';
import { AccountManagerService } from '@app/_services/account-manager.service';
import { ObservationPosition } from '@app/_models/ObservationPosition';
import { ViewEditSingleMarkerMapComponent } from '@app/_maps/view-edit-single-marker-map/view-edit-single-marker-map.component';

@Component({
  selector: 'app-account-manager-location',
  templateUrl: './account-manager-location.component.html',
  styleUrls: ['./account-manager-location.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AccountManagerLocationComponent implements OnInit {
  @ViewChild(ViewEditSingleMarkerMapComponent)
  private mapComponent: ViewEditSingleMarkerMapComponent;
  requesting: boolean;
  defaultPosition: ObservationPosition;

  constructor(private router: Router
    , private tokenService: TokenService
    , private accountManager: AccountManagerService
    , private ref: ChangeDetectorRef) { }

  ngOnInit() {
    this.getUser();
  }

  getUser(): void {
    this.tokenService.getAuthenticatedUserDetails()
      .subscribe(
        (data: UserViewModel) => {
          this.defaultPosition = <ObservationPosition>{
            latitude: data.defaultLocationLatitude,
            longitude: data.defaultLocationLongitude,
            formattedAddress: '',
            shortAddress: ''
          };
        },
        (error: any) => {
          this.defaultPosition = <ObservationPosition>{
            latitude: 54.972237,
            longitude: -2.4608560000000352,
            formattedAddress: '',
            shortAddress: ''
          };
        });
  }

  onSubmit(): void {
    this.requesting = true;

    const model = <SetLocationViewModel>{
      defaultLocationLatitude: this.mapComponent.locationMarker.position.lat,
      defaultLocationLongitude: this.mapComponent.locationMarker.position.lng,
    };

    this.accountManager.postSetLocation(model)
      .subscribe(
        (data: SetLocationViewModel) => {
          this.router.navigate(['login']);
        },
        (error: ErrorReportViewModel) => {
          // if (error.status === 400) { }
          // this.errorReport = error;
          // this.unsuccessful = true;
          this.requesting = false;
          console.log(error.friendlyMessage);
          console.log('unsuccessful registration');
        });
  }
}
