import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { SetLocationViewModel } from '@app/_models/SetLocationViewModel';
import { TokenService } from '@app/_services/token.service';
import { ObservationPosition } from '@app/_models/ObservationPosition';
import { ViewEditSingleMarkerMapComponent } from '@app/_maps/view-edit-single-marker-map/view-edit-single-marker-map.component';
import { AccountManagerService } from '../account-manager.service';

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
  isLoading: boolean;
  defaultPosition: ObservationPosition;

  constructor(private router: Router
    , private tokenService: TokenService
    , private accountManager: AccountManagerService) { }

  ngOnInit() {
    this.getLocation();
  }

  getLocation(): void {
    this.isLoading = true;
    const defaultLocation = this.tokenService.getDefaultLocation();

    this.defaultPosition = <ObservationPosition>{
      latitude: defaultLocation.defaultLocationLatitude,
      longitude: defaultLocation.defaultLocationLongitude,
      formattedAddress: '',
      shortAddress: ''
    };

    if (!defaultLocation.defaultLocationLatitude || !defaultLocation.defaultLocationLongitude) {
      this.defaultPosition.latitude = 54.972237;
      this.defaultPosition.longitude = -2.4608560000000352;
    }
    this.isLoading = false;
  }

  onSubmit(): void {
    this.requesting = true;

    const model = <SetLocationViewModel>{
      defaultLocationLatitude: this.mapComponent.locationMarker.position.lat,
      defaultLocationLongitude: this.mapComponent.locationMarker.position.lng,
    };

    this.accountManager.postSetLocation(model)
      .subscribe(_ => {
        this.router.navigate(['login']);
      },
        (error => {
          console.log(error.friendlyMessage);
          console.log('unsuccessful registration');
        }),
        () => { this.requesting = false; }
      );
  }
}
