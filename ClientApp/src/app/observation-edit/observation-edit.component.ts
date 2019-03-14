import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { ParentErrorStateMatcher } from '../../validators';
import { ObservationService } from '../observation.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';
import { ObservationViewModel } from '../../_models/ObservationViewModel';
import { BirdSummaryViewModel } from '../../_models/BirdSummaryViewModel';

@Component({
  selector: 'app-observation-edit',
  templateUrl: './observation-edit.component.html',
  styleUrls: ['./observation-edit.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ObservationEditComponent implements OnInit {
  observation: ObservationViewModel;
  editObservationForm: FormGroup;
  birdsSpecies: BirdSummaryViewModel[];
  parentErrorStateMatcher = new ParentErrorStateMatcher();
  errorReport: ErrorReportViewModel;

  editObservation_validation_messages = {
    'quantity': [
      { type: 'required', message: 'Quantity is required' }
    ],
    'birdId': [
      { type: 'required', message: 'The observed species is required' }
    ]
  };

  constructor(private router: Router
            , private route: ActivatedRoute
            , private observationService: ObservationService
            , private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.getObservation();
    this.getBirds();
    // this.createForms();
  }

  createForms(): void {
    this.editObservationForm = this.formBuilder.group({
      observationId: new FormControl(this.observation.observationId),
      quantity: new FormControl(this.observation.quantity, Validators.compose([
        Validators.required
      ])),
      birdId: new FormControl(this.observation.bird.birdId, Validators.compose([
        Validators.required
      ])),
      observationDateTime: new FormControl(this.observation.observationDateTime, Validators.compose([
        Validators.required
      ])),
      noteGeneral: new FormControl(this.observation.noteGeneral),
      noteHabitat: new FormControl(this.observation.noteHabitat),
      noteWeather: new FormControl(this.observation.noteWeather),
      noteAppearance: new FormControl(this.observation.noteAppearance),
      noteBehaviour: new FormControl(this.observation.noteBehaviour),
      noteVocalisation: new FormControl(this.observation.noteVocalisation),
    });
  }

  onSubmit(value): void {

    // console.log(value);
    this.observationService.updateObservation(this.observation.observationId, value)
    .subscribe(
      (data: ObservationViewModel) => {
        this.editObservationForm.reset();
        this.router.navigate(['/observation-detail/' + data.observationId.toString()]);
      },
      (error: ErrorReportViewModel) => {
        // console.log(error); alert('hello');
        this.errorReport = error;
        // this.invalidEditObservation = true;
        console.log(error);
        console.log(error.friendlyMessage);
        console.log('unsuccessful add observation');
      }
    );
  }

  getObservation(): void {
    const id = +this.route.snapshot.paramMap.get('id');

    this.observationService.getObservation(id)
    .subscribe(
      (observation: ObservationViewModel) => {
        this.observation = observation;
        this.createForms();
      },
      (error: ErrorReportViewModel) => {
        this.errorReport = error;
        // this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
      });


  }

  getBirds(): void {
    // TODO: Better implementation of this...
    this.observationService.getBirds()
    .subscribe(
      (data: BirdSummaryViewModel[]) => { this.birdsSpecies = data; },
      (error: ErrorReportViewModel) => {
        console.log('could not get the birds ddl');
      });
  }

}