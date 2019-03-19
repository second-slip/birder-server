import { Component, OnInit, ViewEncapsulation, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { ParentErrorStateMatcher } from '../../validators';
import { Router } from '@angular/router';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';
import { BirdSummaryViewModel } from '../../_models/BirdSummaryViewModel';
import { BirdsService } from '../birds.service';
import { LocationViewModel } from '../../_models/LocationViewModel';
import { GeocodeService } from '../geocode.service';

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
  //

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
            , private formBuilder: FormBuilder
            , private geocodeService: GeocodeService
            , private ref: ChangeDetectorRef) { }

  ngOnInit() {
    // ****
    this.useGeolocation('54.972237,-2.460856');
    // ****
    this.getBirds();
    this.createForms();
  }

  useGeolocation(searchValue: string) {
    // alert(searchValue);
    // this.loading = true;
    this.geocodeService.geocodeAddress(searchValue)
      .subscribe((location: LocationViewModel) => {
        this.addObservationForm.get('lat').setValue(location.latitude);
        this.addObservationForm.get('lng').setValue(location.longitude);
        this.geolocation = location.formattedAddress;
        this.searchAddress = '';
        this.ref.detectChanges();
        // this.geocodeService.reverseGeocode(this.location);
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
        this.addObservationForm.get('lat').setValue(location.latitude);
        this.addObservationForm.get('lng').setValue(location.longitude);
        this.geolocation = location.formattedAddress;
        this.ref.detectChanges();
      },
      (error: any) => {}
      );
  }

  createForms(): void {
    this.addObservationForm = this.formBuilder.group({
      lat: new FormControl(54.972237),
      lng: new FormControl(-2.460856),
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
    console.log(value);
    // this.observationService.addObservation(value)
    // .subscribe(
    //   (data: ObservationViewModel) => {
    //     this.addObservationForm.reset();
    //     this.router.navigate(['/observation-detail/' + data.observationId.toString()]);
    //   },
    //   (error: ErrorReportViewModel) => {
    //     // console.log(error); alert('hello');
    //     this.errorReport = error;
    //     this.invalidAddObservation = true;
    //     console.log(error);
    //     console.log(error.friendlyMessage);
    //     console.log('unsuccessful add observation');
    //   }
    // );
  }

  getBirds(): void {
    this.birdsService.getBirds()
    .subscribe(
      (data: BirdSummaryViewModel[]) => { this.birdsSpecies = data; },
      (error: ErrorReportViewModel) => {
        console.log('could not get the birds ddl');
      });
  }

}
