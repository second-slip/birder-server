import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';
import { BirdSummaryViewModel } from '@app/_models/BirdSummaryViewModel';
import { Router } from '@angular/router';
import { BirdsService } from '@app/_services/birds.service';
import { ObservationService } from '@app/_observations/observation.service';
import { TokenService } from '@app/_services/token.service';
import { ObservationAddDto, ObservationViewModel } from '@app/_models/ObservationViewModel';
import { ViewEditSingleMarkerMapComponent } from '@app/_maps/view-edit-single-marker-map/view-edit-single-marker-map.component';
import { ObservationPosition } from '@app/_models/ObservationPosition';
import { ObservationNote, ObservationNoteType } from '@app/_models';
import { AddNotesComponent } from '@app/_observationNotes/add-notes/add-notes.component';
import * as moment from 'moment';
import { ThemePalette } from '@angular/material/core';
import { ParentErrorStateMatcher, BirdsListValidator } from '@app/_validators';

@Component({
  selector: 'app-observation-add',
  templateUrl: './observation-add.component.html',
  styleUrls: ['./observation-add.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ObservationAddComponent implements OnInit {
  isLinear = false;
  @ViewChild(ViewEditSingleMarkerMapComponent)
  private mapComponent: ViewEditSingleMarkerMapComponent;
  @ViewChild(AddNotesComponent)
  private notesComponent: AddNotesComponent;

  requesting: boolean;
  public errorObject = null;
  addObservationForm: FormGroup;

  birdsSpecies: BirdSummaryViewModel[]
  filteredOptions$: Observable<BirdSummaryViewModel[]>;

  parentErrorStateMatcher = new ParentErrorStateMatcher();
  errorReport: any;
  invalidAddObservation: boolean;

  defaultPosition: ObservationPosition;

  hideAlert = false;

  @ViewChild('picker') picker: any;

  public showSpinners = true;
  public showSeconds = false;
  public touchUi = false;
  public enableMeridian = false;
  public minDate = moment().subtract(20, "years");// new Date().toISOString(); // moment.Moment;
  public maxDate = moment().format('yyyy-MM-dd 23:59:59'); // new Date().toISOString(); // moment.Moment;
  public stepHour = 1;
  public stepMinute = 1;
  public stepSecond = 1;
  public color: ThemePalette = 'primary';
  //

  addObservation_validation_messages = {
    'quantity': [
      { type: 'required', message: 'Quantity is required' }
    ],
    'bird': [
      { type: 'required', message: 'The observed species is required' },
      { type: 'notBirdListObject', message: 'You must select a bird species from the list.' }

    ],
    'observationDateTime': [
      { type: 'required', message: 'The date and time are required' },
      { type: 'invalidDate', message: 'Invalid date/time format. Use the control to choose a valid date/time.' }
    ]
  };


  constructor(private router: Router
    , private birdsService: BirdsService
    , private observationService: ObservationService
    , private tokenService: TokenService
    , private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.getLocation();
    this.getBirds();
  }

  getBirdAutocompleteOptions() {
    this.filteredOptions$ = this.addObservationForm.controls['bird'].valueChanges
      .pipe(
        startWith(''),
        map(value => value.length >= 1 ? this._filter(value) : this.birdsSpecies)
      );
  }

  getBirds(): void {
    this.birdsService.getBirdsDdl()
      .subscribe(
        (data: BirdSummaryViewModel[]) => {
          this.birdsSpecies = data;
          this.getBirdAutocompleteOptions();
        },
        (_ => {
          console.log('could not get the birds ddl');
        }));
  }

  displayFn(bird: BirdSummaryViewModel): string {
    return bird && bird.englishName ? bird.englishName : null;
  }

  private _filter(value: string): BirdSummaryViewModel[] {
    const filterValue = value.toLowerCase();
    return this.birdsSpecies.filter(option => option.englishName.toLowerCase().indexOf(filterValue) >= 0); // or !== 11
  }

  createForms(): void {
    this.addObservationForm = this.formBuilder.group({
      quantity: new FormControl(1, Validators.compose([
        Validators.required
      ])),
      bird: new FormControl('', Validators.compose([
        Validators.required,
        BirdsListValidator()
      ])),

      observationDateTime: new FormControl((moment()), Validators.compose([
        //new Date()).toISOString()
        Validators.required
        //DateValid(/\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}.\d{3}Z/) 
        // not really necessary as input is now readonly,
        // and it should be an async validator, regex is crap here
      ])),
    });
  }

  onSubmit(formValue: ObservationViewModel): void {
    this.requesting = true;

    const notes: ObservationNote[] = this.notesComponent.notes.map(note => ({
      id: 0,
      noteType: ObservationNoteType[note.noteType],
      note: note.note
    }));

    const position = <ObservationPosition>{
      latitude: this.mapComponent.locationMarker.position.lat,
      longitude: this.mapComponent.locationMarker.position.lng,
      formattedAddress: this.mapComponent.position.formattedAddress,
      shortAddress: this.mapComponent.position.shortAddress
    };

    // console.log(formValue.observationDateTime);
    // console.log(moment(formValue.observationDateTime).utc());
    // console.log(new Date(formValue.observationDateTime));
    // console.log(new Date(formValue.observationDateTime).toISOString());
    // console.log(new Date(formValue.observationDateTime).toUTCString());


    const observation = <ObservationAddDto>{
      quantity: formValue.quantity,
      observationDateTime: new Date(formValue.observationDateTime),
      bird: formValue.bird,
      birdId: formValue.bird.birdId,
      position: position,
      notes: notes,
    }

    console.log(observation.observationDateTime);
    console.log(observation);

    this.observationService.addObservation(observation)
      .subscribe(
        (data: ObservationViewModel) => {
          // this.addObservationForm.reset();
          this.router.navigate(['/observation-detail/' + data.observationId.toString()]);
        },
        (error: any) => {
          this.requesting = false;
          this.errorReport = error;
          this.invalidAddObservation = true;
          // console.log(error);
          // console.log(error.friendlyMessage);
          // console.log('unsuccessful add observation');
        }
      );
  }

  getLocation(): void {
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
    this.createForms();
  }

  public onStepperSelectionChange(evant: any) {
    this.scrollToSectionHook();
    //this.stepper.selectionChange.subscribe((event) => { this.scrollToSectionHook(event.selectedIndex); });
  }

  private scrollToSectionHook() {
    const element = document.querySelector('.stepperTop0');
    if (element) {
      setTimeout(() => {
        element.scrollIntoView({
          behavior: 'smooth', block: 'start', inline:
            'nearest'
        });
      }, 250);
    }
  }
}
