import { Component, OnInit, ViewEncapsulation, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { ParentErrorStateMatcher } from '../../../validators';
import { Router } from '@angular/router';
import { ErrorReportViewModel } from '../../../_models/ErrorReportViewModel';
import { BirdSummaryViewModel } from '../../../_models/BirdSummaryViewModel';
import { BirdsService } from '../../birds.service';
import { LocationViewModel } from '../../../_models/LocationViewModel';
import { GeocodeService } from '../../geocode.service';
import { UserService } from '../../user.service';
import { UserViewModel } from '../../../_models/UserViewModel';
import { ObservationViewModel } from '../../../_models/ObservationViewModel';
import { ObservationService } from '../../observation.service';
import { TokenService } from '../../token.service';
import { BirderStatus } from '../../../_models/BirdIndexOptions';

@Component({
  selector: 'app-observation-add',
  templateUrl: './observation-add.component.html',
  styleUrls: ['./observation-add.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ObservationAddComponent implements OnInit {
  addObservationForm: FormGroup;
  birdsSpecies: BirdSummaryViewModel[];
  parentErrorStateMatcher = new ParentErrorStateMatcher();
  errorReport: ErrorReportViewModel;
  invalidAddObservation: boolean;
  //
  geolocation: string;
  searchAddress = '';
  geoError: string;
  user: UserViewModel;
  //
  hideAlert = false;

  addObservation_validation_messages = {
    'quantity': [
      { type: 'required', message: 'Quantity is required' }
    ],
    'birdId': [
      { type: 'required', message: 'The observed species is required' }
    ]
  };

  constructor(private router: Router
    , private birdsService: BirdsService
    , private observationService: ObservationService
    // , private userService: UserService
    , private tokenService: TokenService
    , private formBuilder: FormBuilder
    , private geocodeService: GeocodeService
    , private ref: ChangeDetectorRef) { }

  ngOnInit() {
    this.getUser();
    this.getBirds(BirderStatus.Common);
  }

  getGeolocation(): void {
    this.geocodeService.reverseGeocode(this.user.defaultLocationLatitude, this.user.defaultLocationLongitude)
      .subscribe(
        (data: LocationViewModel) => {
          this.geolocation = data.formattedAddress;

          this.ref.detectChanges();
        },
        (error: any) => {
          //
        }
      );
  }

  useGeolocation(searchValue: string) {
    this.geocodeService.geocodeAddress(searchValue)
      .subscribe((location: LocationViewModel) => {
        this.addObservationForm.get('locationLatitude').setValue(location.latitude);
        this.addObservationForm.get('locationLongitude').setValue(location.longitude);
        this.geolocation = location.formattedAddress;
        this.searchAddress = '';
        this.ref.detectChanges();
      }
      );
  }

  closeAlert() {
    this.geoError = null;
  }

  getCurrentPosition() {
    if (window.navigator.geolocation) {
      window.navigator.geolocation.getCurrentPosition(
        (position) => {
          this.useGeolocation(position.coords.latitude.toString() + ',' + position.coords.longitude.toString());
        }, (error) => {
          switch (error.code) {
            case 3: // ...deal with timeout
              this.geoError = 'The request to get user location timed out...';
              break;
            case 2: // ...device can't get data
              this.geoError = 'Location information is unavailable...';
              break;
            case 1: // ...user said no ☹️
              this.geoError = 'User denied the request for Geolocation...';
              break;
            default:
              this.geoError = 'An error occurred with Geolocation...';
          }
        });
    } else {
      this.geoError = 'Geolocation not supported in this browser';
    }
  }

  placeMarker($event) {
    this.geocodeService.reverseGeocode($event.coords.lat, $event.coords.lng)
      .subscribe(
        (location: LocationViewModel) => {
          this.addObservationForm.get('locationLatitude').setValue(location.latitude);
          this.addObservationForm.get('locationLongitude').setValue(location.longitude);
          this.geolocation = location.formattedAddress;
          this.ref.detectChanges();
        },
        (error: any) => { }
      );
  }

  createForms(): void {
    this.addObservationForm = this.formBuilder.group({
      locationLatitude: new FormControl(this.user.defaultLocationLatitude),
      locationLongitude: new FormControl(this.user.defaultLocationLongitude),
      quantity: new FormControl(1, Validators.compose([
        Validators.required
      ])),
      birdId: new FormControl('', Validators.compose([
        Validators.required
      ])),
      observationDateTime: new FormControl((new Date()).toISOString(), Validators.compose([
        Validators.required
      ])),
      noteGeneral: new FormControl(''),
      noteHabitat: new FormControl(''),
      noteWeather: new FormControl(''),
      noteAppearance: new FormControl(''),
      noteBehaviour: new FormControl(''),
      noteVocalisation: new FormControl(''),
    });
  }

  onSubmit(value): void {
    this.observationService.addObservation(value)
      .subscribe(
        (data: ObservationViewModel) => {
          this.addObservationForm.reset();
          this.router.navigate(['/observation-detail/' + data.observationId.toString()]);
        },
        (error: ErrorReportViewModel) => {
          // console.log(error); alert('hello');
          this.errorReport = error;
          this.invalidAddObservation = true;
          console.log(error);
          console.log(error.friendlyMessage);
          console.log('unsuccessful add observation');
        }
      );
  }

  getBirds(filter: BirderStatus): void {
    this.birdsService.getBirds(filter)
      .subscribe(
        (data: BirdSummaryViewModel[]) => { this.birdsSpecies = data; },
        (error: ErrorReportViewModel) => {
          console.log('could not get the birds ddl');
        });
  }

  getUser(): void {
    this.tokenService.getAuthenticatedUserDetails()
      .subscribe(
        (data: UserViewModel) => {
          this.user = data;
          this.createForms();
          this.getGeolocation();
        },
        (error: any) => {
          console.log('could not get the user, using default coordinates');
          const userTemp = <UserViewModel>{
            userName: '',
            profileImage: '',
            defaultLocationLatitude: 54.972237,
            defaultLocationLongitude: -2.4608560000000352,
          };
          this.user = userTemp;
          this.createForms();
          this.getGeolocation();
        });
  }

  dismissAlert(): void {
    alert(this.hideAlert);
    this.hideAlert = true;
  }
}
