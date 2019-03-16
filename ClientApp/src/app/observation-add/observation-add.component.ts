import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { ParentErrorStateMatcher } from '../../validators';
import { ObservationService } from '../observation.service';
import { Router } from '@angular/router';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';
import { ObservationViewModel } from '../../_models/ObservationViewModel';
import { BirdSummaryViewModel } from '../../_models/BirdSummaryViewModel';
import { BirdsService } from '../birds.service';

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

  addObservation_validation_messages = {
    'quantity': [
      { type: 'required', message: 'Quantity is required' }
    ],
    'birdId': [
      { type: 'required', message: 'The observed species is required' }
    ]
  };

  constructor(private router: Router
    , private observationService: ObservationService
    , private birdsService: BirdsService
    , private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.getBirds();
    this.createForms();
  }

  createForms(): void {
    this.addObservationForm = this.formBuilder.group({
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

  getBirds(): void {
    this.birdsService.getBirds()
    .subscribe(
      (data: BirdSummaryViewModel[]) => { this.birdsSpecies = data; },
      (error: ErrorReportViewModel) => {
        console.log('could not get the birds ddl');
      });
  }

}
