import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
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
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { ViewEditSingleMarkerMapComponent } from '@app/_maps/view-edit-single-marker-map/view-edit-single-marker-map.component';
import { ObservationPosition } from '@app/_models/ObservationPosition';
import { ObservationNote, ObservationNoteType } from '@app/_models/ObservationNote';
import { AddNotesComponent } from '@app/_observationNotes/add-notes/add-notes.component';
import { EditNotesComponent } from '@app/_observationNotes/edit-notes/edit-notes.component';
import { EditNoteDialogComponent } from '@app/_observationNotes/edit-note-dialog/edit-note-dialog.component';
import * as moment from 'moment';
import { DateValid } from 'validators/dateTime.validator';

@Component({
  selector: 'app-observation-add',
  templateUrl: './observation-add.component.html',
  styleUrls: ['./observation-add.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ObservationAddComponent implements OnInit {
  @ViewChild(ViewEditSingleMarkerMapComponent)
  private mapComponent: ViewEditSingleMarkerMapComponent;
  @ViewChild(AddNotesComponent)
  private notesComponent: AddNotesComponent;
  requesting: boolean;
  addObservationForm: FormGroup;
  birdsSpecies: BirdSummaryViewModel[]
  parentErrorStateMatcher = new ParentErrorStateMatcher();
  errorReport: ErrorReportViewModel;
  invalidAddObservation: boolean;
  filteredOptions: Observable<BirdSummaryViewModel[]>;
  user: UserViewModel;
  hideAlert = false;

  @ViewChild('picker') picker: any;
  //
  //public date: moment.Moment;
  public disabled = false;
  public showSpinners = true;
  public showSeconds = false;
  public touchUi = false;
  public enableMeridian = false;
  public minDate = moment().subtract(20, "years");// new Date().toISOString(); // moment.Moment;
  public maxDate = moment().format('YYYY-MM-DD 23:59:59'); // new Date().toISOString(); // moment.Moment;
  public stepHour = 1;
  public stepMinute = 1;
  public stepSecond = 1;
  //public color: ThemePalette = 'primary';
  //

  addObservation_validation_messages = {
    'quantity': [
      { type: 'required', message: 'Quantity is required' }
    ],
    'bird': [
      { type: 'required', message: 'The observed species is required' }
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
    this.getUser();
    this.getBirds();
  }

  // setDatePickerRange(): void {
  //   let date = new Date();
  //   date.setHours
  //   this.maxDate = 


  // }

  getBirdAutocompleteOptions() {
    this.filteredOptions = this.addObservationForm.controls['bird'].valueChanges.pipe(
      startWith(''),
      map(value => value.length >= 1 ? this._filter(value) : this.birdsSpecies)
    );
  }

  displayFn(bird: BirdSummaryViewModel): string {
    return bird && bird.englishName ? bird.englishName : null;
  }

  private _filter(value: string): BirdSummaryViewModel[] {
    const filterValue = value.toLowerCase();
    return this.birdsSpecies.filter(option => option.englishName.toLowerCase().indexOf(filterValue) === 0);
  }

  createForms(): void {
    this.addObservationForm = this.formBuilder.group({
      quantity: new FormControl(1, Validators.compose([
        Validators.required
      ])),
      bird: new FormControl('', Validators.compose([
        Validators.required
      ])),

      observationDateTime: new FormControl((new Date()).toISOString(), Validators.compose([
        Validators.required
        //DateValid(/\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}.\d{3}Z/) 
        // not really necessary as input is now readonly,
        // and it should be an async validator, regex is crap here
      ])),
    });
  }

  onSubmit(formValue: ObservationViewModel): void {
    this.requesting = true;

    // const testNotes: ObservationNote[] = [];
    const notes: ObservationNote[] = this.notesComponent.notes.map(note => ({
      id: 0,
      noteType: ObservationNoteType[note.noteType],
      note: note.note
    }));

    const position = <ObservationPosition>{
      latitude: this.mapComponent.locationMarker.position.lat,
      longitude: this.mapComponent.locationMarker.position.lng,
      formattedAddress: this.mapComponent.geolocation
    };

    const observation = <ObservationViewModel>{
      quantity: formValue.quantity,
      observationDateTime: new Date(formValue.observationDateTime).toISOString(), // formValue.observationDateTime,
      bird: formValue.bird,
      birdId: formValue.bird.birdId,
      position: position,
      notes: notes,
      observationId: 0,
      user: null,
      creationDate: new Date().toISOString(),
      hasPhotos: false,
      lastUpdateDate: new Date().toISOString()
    }

    this.observationService.addObservation(observation)
      .subscribe(
        (data: ObservationViewModel) => {
          // this.addObservationForm.reset();
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
        });
  }

  // dismissAlert(): void {
  //   alert(this.hideAlert);
  //   this.hideAlert = true;
  // }
}
