import { Component, ViewEncapsulation, OnInit, ViewChild } from '@angular/core';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { BirdSummaryViewModel } from '@app/_models/BirdSummaryViewModel';
import { ParentErrorStateMatcher } from 'validators';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { Router, ActivatedRoute } from '@angular/router';
import { ObservationService } from '@app/_sharedServices/observation.service';
import { BirdsService } from '@app/_services/birds.service';
import { TokenService } from '@app/_services/token.service';
import { ToastrService } from 'ngx-toastr';
import { Location } from '@angular/common';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';
import { ObservationPosition } from '@app/_models/ObservationPosition';
import { ViewEditSingleMarkerMapComponent } from '@app/_maps/view-edit-single-marker-map/view-edit-single-marker-map.component';
import { EditNotesComponent } from '@app/_observationNotes/edit-notes/edit-notes.component';
import { ObservationNote, ObservationNoteType } from '@app/_models/ObservationNote';

@Component({
  selector: 'app-observation-edit',
  templateUrl: './observation-edit.component.html',
  styleUrls: ['./observation-edit.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ObservationEditComponent implements OnInit {
  @ViewChild(ViewEditSingleMarkerMapComponent)
  private timerComponent: ViewEditSingleMarkerMapComponent;
  @ViewChild(EditNotesComponent)
  private editNotesComponent: EditNotesComponent;
  requesting: boolean;
  observation: ObservationViewModel;
  editObservationForm: FormGroup;
  birdsSpecies: BirdSummaryViewModel[];
  parentErrorStateMatcher = new ParentErrorStateMatcher();
  errorReport: ErrorReportViewModel;

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
    , private location: Location) { }

  ngOnInit() {
    this.getObservation();
  }

  displayFn(bird: BirdSummaryViewModel): string {
    return bird && bird.englishName ? bird.englishName : null;
  }

  getBirdAutocompleteOptions() {
    this.filteredOptions = this.editObservationForm.controls['bird'].valueChanges.pipe(
      startWith(''),
      map(value => value.length >= 1 ? this._filter(value) : this.birdsSpecies)
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
    });
  }

  onSubmit(value: ObservationViewModel): void {
    this.requesting = true;

    const position = <ObservationPosition>{
      latitude: this.timerComponent.locationMarker.position.lat,
      longitude: this.timerComponent.locationMarker.position.lng,
      formattedAddress: this.timerComponent.geolocation
    }


    const notes: ObservationNote[] = this.editNotesComponent.notes.map(note => ({
      id: note.id,
      noteType: ObservationNoteType[note.noteType],
      note: note.note
    }));

    const observation = <ObservationViewModel>{
      quantity: value.quantity,
      observationDateTime: value.observationDateTime,
      bird: value.bird,
      birdId: value.bird.birdId,
      notes: notes,
      observationId: this.observation.observationId,
      user: this.observation.user,
      creationDate: this.observation.creationDate,
      hasPhotos: false, // might have a problem
      position: position,
      lastUpdateDate: new Date().toISOString()
    }

    this.observationService.updateObservation(this.observation.observationId, observation)
      .subscribe(
        (data: ObservationViewModel) => {
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
          // this.addMarker(observation.position.latitude, observation.position.longitude);
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
        (data: BirdSummaryViewModel[]) => {
          this.birdsSpecies = data;
          this.getBirdAutocompleteOptions();
        },
        (error: ErrorReportViewModel) => {
          console.log('could not get the birds ddl');
        });
  }
}
