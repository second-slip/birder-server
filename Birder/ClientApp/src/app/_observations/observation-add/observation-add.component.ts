import { Component, OnInit, ViewEncapsulation, ChangeDetectorRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';
import { BirdSummaryViewModel } from '@app/_models/BirdSummaryViewModel';
import { ParentErrorStateMatcher } from 'validators';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { UserViewModel } from '@app/_models/UserViewModel';
import { Router } from '@angular/router';
import { BirdsService } from '@app/_services/birds.service';
import { ObservationService } from '@app/_sharedServices/observation.service';
import { TokenService } from '@app/_services/token.service';
import { GeocodeService } from '@app/_services/geocode.service';
import { LocationViewModel } from '@app/_models/LocationViewModel';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { GoogleMap, MapInfoWindow, MapMarker } from '@angular/google-maps';

@Component({
  selector: 'app-observation-add',
  templateUrl: './observation-add.component.html',
  styleUrls: ['./observation-add.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ObservationAddComponent implements OnInit {
  requesting: boolean;
  addObservationForm: FormGroup;
  birdsSpecies: BirdSummaryViewModel[]
  // birdsSpecies: BirdSummaryViewModel[];
  parentErrorStateMatcher = new ParentErrorStateMatcher();
  errorReport: ErrorReportViewModel;
  invalidAddObservation: boolean;
  //
  filteredOptions: Observable<BirdSummaryViewModel[]>;
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
    'bird': [
      { type: 'required', message: 'The observed species is required' }
    ]
  };

  @ViewChild(GoogleMap, { static: false }) map: GoogleMap
  // @ViewChild(MapMarker, { static: false }) mark: MapMarker
  @ViewChild(MapInfoWindow, { static: false }) infoWindow: MapInfoWindow
  zoom = 11;
  options: google.maps.MapOptions = {
    mapTypeId: 'terrain'
  }

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
    this.getBirds();
  }

  getBirdAutocompleteOptions() {
    // this.addObservationForm.controls['bird']
    this.filteredOptions = this.addObservationForm.controls['bird'].valueChanges.pipe(
      startWith(''),
      map(value => value.length >= 1 ? this._filter(value): this.birdsSpecies)
      // map(value => this._filter(value))
    );
  }

  displayFn(bird: BirdSummaryViewModel): string {
    return bird && bird.englishName ? bird.englishName : null;
  }


  private _filter(value: string): BirdSummaryViewModel[] {
    const filterValue = value.toLowerCase();
    return this.birdsSpecies.filter(option => option.englishName.toLowerCase().indexOf(filterValue) === 0);
  }


  getGeolocation(latitude: number, longitude:number): void {
    this.geocodeService.reverseGeocode(latitude, longitude)
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
        // this.addObservationForm.get('locationLatitude').setValue(location.latitude);
        // this.addObservationForm.get('locationLongitude').setValue(location.longitude);
        this.marker.position = {
          lat: location.latitude,
          lng: location.longitude
        };
        this.map.panTo(this.marker.position);

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

  createForms(): void {
    this.addObservationForm = this.formBuilder.group({
      // locationLatitude: new FormControl(this.user.defaultLocationLatitude),
      // locationLongitude: new FormControl(this.user.defaultLocationLongitude),
      quantity: new FormControl(1, Validators.compose([
        Validators.required
      ])),
      bird: new FormControl('', Validators.compose([
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

  marker; // make marker a property?
  addMarker(latitude: number, longitude:number) {
    this.marker = ({
      position: {
        lat: latitude,
        lng: longitude
      },
      label: {
        color: 'red',
        text: 'Marker label',
      },
      title: 'Marker title',
      options: { draggable: true },
    })

    this.getGeolocation(latitude, longitude);
  }
  // addMarker() {
  //   this.marker = ({
  //     position: {
  //       lat: this.user.defaultLocationLatitude,
  //       lng: this.user.defaultLocationLongitude
  //     },
  //     label: {
  //       color: 'red',
  //       text: 'Marker label',
  //     },
  //     title: 'Marker title',
  //     options: { draggable: true },
      
  //   })
  // }

  openInfoWindow(marker: MapMarker) {
    // console.log(marker);
    this.infoWindow.open(marker);
  }

  markerChanged(event: google.maps.MouseEvent): void {
    this.addMarker(event.latLng.lat(), event.latLng.lng());
    // console.log(event.latLng.lat());
    // console.log(event.latLng.lng());

    // this.marker.position =  {
    //   lat: event.latLng.lat(),
    //   lng: event.latLng.lng()
    // };

    // this.getGeolocation(event.latLng.lat(), event.latLng.lng());
  }

  onSubmit(value): void {
    this.requesting = true;

    console.log(value);
    console.log(typeof(value));

    const observation = <ObservationViewModel> {
      quantity: value.quantity,
      observationDateTime: value.observationDateTime,
      bird: value.bird,
      birdId: value.bird.birdId,
      noteAppearance: value.noteAppearance,
      noteBehaviour : value.noteAppearance,
      noteGeneral: value.noteGeneral,
      noteHabitat: value.noteHabitat,
      noteVocalisation: value.noteVocalisation,
      noteWeather: value.noteWeather,
      //
      locationLatitude: this.marker.position.lat,
      locationLongitude: this.marker.position.lng,
      // the below are set at the server-side
      observationId: 0,
      user: null,
      creationDate: new Date().toISOString(),
      hasPhotos: false,
      lastUpdateDate: new Date().toISOString() 
    }

    this.observationService.addObservation(observation)
      .subscribe(
        (data: ObservationViewModel) => {
          this.addObservationForm.reset();
          this.router.navigate(['/observation-detail/' + data.observationId.toString()]);
        },
        (error: ErrorReportViewModel) => {
          this.requesting = false;
          this.errorReport = error;
          this.invalidAddObservation = true;
          console.log(error);
          console.log(error.friendlyMessage);
          console.log('unsuccessful add observation');
        }
      );
  }

  getBirds(): void {
    this.birdsService.getBirdsDdl()
      .subscribe(
        (data: BirdSummaryViewModel[]) => { 
          this.birdsSpecies = data;
          this.getBirdAutocompleteOptions();
         },
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
          this.addMarker(data.defaultLocationLatitude, data.defaultLocationLongitude);
          // this.getGeolocation(this.user.defaultLocationLatitude, this.user.defaultLocationLongitude);
        },
        (error: any) => {
          console.log('could not get the user, using default coordinates');
          const userTemp = <UserViewModel>{
            userName: '',
            avatar: '',
            defaultLocationLatitude: 54.972237,
            defaultLocationLongitude: -2.4608560000000352,
          };
          this.user = userTemp;
          this.createForms();
          this.getGeolocation(this.user.defaultLocationLatitude, this.user.defaultLocationLongitude);
        });
  }

  dismissAlert(): void {
    alert(this.hideAlert);
    this.hideAlert = true;
  }
}
