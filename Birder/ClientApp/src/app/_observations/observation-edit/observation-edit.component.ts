import { Component, ViewEncapsulation, OnInit, ChangeDetectorRef } from '@angular/core';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { BirdSummaryViewModel } from '@app/_models/BirdSummaryViewModel';
import { ParentErrorStateMatcher } from 'validators';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { Router, ActivatedRoute } from '@angular/router';
import { ObservationService } from '@app/_sharedServices/observation.service';
import { BirdsService } from '@app/_services/birds.service';
import { GeocodeService } from '@app/_services/geocode.service';
import { BirderStatus } from '@app/_models/BirdIndexOptions';
import { LocationViewModel } from '@app/_models/LocationViewModel';
import { TokenService } from '@app/_services/token.service';
import { ToastrService } from 'ngx-toastr';
import { Location } from '@angular/common';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';

@Component({
  selector: 'app-observation-edit',
  templateUrl: './observation-edit.component.html',
  styleUrls: ['./observation-edit.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ObservationEditComponent implements OnInit {
  requesting: boolean;
  observation: ObservationViewModel;
  editObservationForm: FormGroup;
  birdsSpecies: BirdSummaryViewModel[];
  parentErrorStateMatcher = new ParentErrorStateMatcher();
  errorReport: ErrorReportViewModel;
  geolocation: string;
  searchAddress = '';
  geoError: string;

  filteredOptions: Observable<BirdSummaryViewModel[]>;

  editObservation_validation_messages = {
    'quantity': [
      { type: 'required', message: 'Quantity is required' }
    ],
    'bird': [
      { type: 'required', message: 'The observed species is required' }
    ]
  };

  constructor(private router: Router
    , private toast: ToastrService
    , private route: ActivatedRoute
    , private observationService: ObservationService
    , private tokenService: TokenService
    , private birdsService: BirdsService
    , private formBuilder: FormBuilder
    , private location: Location
    , private geocodeService: GeocodeService
    , private ref: ChangeDetectorRef) { }

  ngOnInit() {
    this.getObservation();
    // this.getBirds();
  }

  displayFn(bird: BirdSummaryViewModel): string {
    // console.log(bird);
    // console.log(bird.englishName);
    return bird && bird.englishName ? bird.englishName : null;
  }

  // init() {
  //   this.filteredOptions = this.myControl.valueChanges.pipe(
  //     startWith(''),
  //     map(value => value.length >= 1 ? this._filter(value): this.birdsSpecies)
  //     // map(value => this._filter(value))
  //   );
  // }

  getBirdAutocompleteOptions() {
    // this.addObservationForm.controls['bird']
    this.filteredOptions = this.editObservationForm.controls['bird'].valueChanges.pipe(
      startWith(''),
      map(value => value.length >= 1 ? this._filter(value): this.birdsSpecies)
      // map(value => this._filter(value))
    );
  }

  private _filter(value: string): BirdSummaryViewModel[] {
    const filterValue = value.toLowerCase();
    return this.birdsSpecies.filter(option => option.englishName.toLowerCase().indexOf(filterValue) === 0);
  }

  createForms(): void {
    this.editObservationForm = this.formBuilder.group({
      observationId: new FormControl(this.observation.observationId),
      quantity: new FormControl(this.observation.quantity, Validators.compose([
        Validators.required
      ])),
      bird: new FormControl(this.observation.bird, Validators.compose([
        Validators.required
      ])),
      observationDateTime: new FormControl(this.observation.observationDateTime, Validators.compose([
        Validators.required
      ])),
      locationLatitude: new FormControl(this.observation.locationLatitude),
      locationLongitude: new FormControl(this.observation.locationLongitude),
      noteGeneral: new FormControl(this.observation.noteGeneral),
      noteHabitat: new FormControl(this.observation.noteHabitat),
      noteWeather: new FormControl(this.observation.noteWeather),
      noteAppearance: new FormControl(this.observation.noteAppearance),
      noteBehaviour: new FormControl(this.observation.noteBehaviour),
      noteVocalisation: new FormControl(this.observation.noteVocalisation),
    });
  }

  onSubmit(value): void {
    this.requesting = true;
    this.observationService.updateObservation(this.observation.observationId, value)
      .subscribe(
        (data: ObservationViewModel) => {
          this.editObservationForm.reset();
          this.router.navigate(['/observation-detail/' + data.observationId.toString()]);
        },
        (error: ErrorReportViewModel) => {
          this.requesting = false;
          this.errorReport = error;
          console.log(error.friendlyMessage);
          console.log('unsuccessful add observation');
        }
      );
  }

  goBack(): void {
    this.location.back();
  }

  getObservation(): void {
    const id = +this.route.snapshot.paramMap.get('id');

    this.observationService.getObservation(id)
      .subscribe(
        (observation: ObservationViewModel) => {
          this.observation = observation;
          if (this.tokenService.checkIsRecordOwner(observation.user.userName) === false) {
            this.toast.error(`Only the observation owner can edit their report`, `Not allowed`);
            this.router.navigate(['/observation-feed']);
            return;
          }
          this.createForms();
          this.getGeolocation();
          this.getBirds();
        },
        (error: ErrorReportViewModel) => {
          this.errorReport = error;
          // this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
        });
  }

  getBirds(): void {
    this.birdsService.getBirdsDdl()
      .subscribe(
        (data: BirdSummaryViewModel[]) => 
        {
           this.birdsSpecies = data; 
           this.getBirdAutocompleteOptions();
          },
        (error: ErrorReportViewModel) => {
          console.log('could not get the birds ddl');
        });
  }

  // new google maps methods...
  getGeolocation(): void {
    this.geocodeService.reverseGeocode(this.observation.locationLatitude, this.observation.locationLongitude)
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
        this.editObservationForm.get('locationLatitude').setValue(location.latitude);
        this.editObservationForm.get('locationLongitude').setValue(location.longitude);
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

  markerDragEnd($event: MouseEvent) {
    // placeMarker($event) {

      console.log('dragEnd', $event);

    // this.geocodeService.reverseGeocode($event.coords.lat, $event.coords.lng)
    
    // this.geocodeService.reverseGeocode($event.latLng.lat, $event.latLng.lng)
    //   .subscribe(
    //     (location: LocationViewModel) => {
    //       this.editObservationForm.get('locationLatitude').setValue(location.latitude);
    //       alert(this.editObservationForm.get('locationLatitude'));
    //       this.editObservationForm.get('locationLongitude').setValue(location.longitude);
    //       this.geolocation = location.formattedAddress;
    //       this.ref.detectChanges();
    //     },
    //     (error: any) => { }
    //   );
  }
}
