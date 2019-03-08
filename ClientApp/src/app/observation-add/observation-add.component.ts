import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { ParentErrorStateMatcher } from '../../validators';
import { ObservationService } from '../observation.service';
import { Router } from '@angular/router';
import { Bird } from '../../_models/Bird';

@Component({
  selector: 'app-observation-add',
  templateUrl: './observation-add.component.html',
  styleUrls: ['./observation-add.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ObservationAddComponent implements OnInit {
  addObservationForm: FormGroup;

  birdsSpecies: Bird[];

  parentErrorStateMatcher = new ParentErrorStateMatcher();

  addObservation_validation_messages = {
    // 'username': [
    //   { type: 'required', message: 'Username is required' },
    //   { type: 'minlength', message: 'Username must be at least 5 characters long' },
    //   { type: 'maxlength', message: 'Username cannot be more than 25 characters long' },
    //   { type: 'pattern', message: 'Your username must contain only numbers and letters' },
    //   { type: 'validUsername', message: 'Your username has already been taken' }
    // ],
    'quantity': [
      { type: 'required', message: 'Quantity is required' }
      // { type: 'pattern', message: 'Enter a valid email' }
    ],
    // 'noteGeneral': [
    //   { type: 'required', message: 'Quantity is required' }
    // ]
    // 'confirm_password': [
    //   { type: 'required', message: 'Confirm password is required' },
    //   { type: 'areEqual', message: 'Password mismatch' }
    // ],
    // 'password': [
    //   { type: 'required', message: 'Password is required' }
    //   // { type: 'minlength', message: 'Password must be at least 5 characters long' }
    //   // { type: 'pattern', message: 'Your password must contain at least one uppercase, one lowercase, and one number' }
    // ],
    // 'rememberMe': [
    //   { type: 'pattern', message: 'You must accept terms and conditions' }
    // ]
  };

  constructor(private router: Router
    , private observationService: ObservationService
    , private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.getBirds();
    this.createForms();
  }

  createForms(): void {
    this.addObservationForm = this.formBuilder.group({
      // username: new FormControl('', Validators.compose([
      //  UsernameValidator.validUsername,
      //  Validators.maxLength(25),
      //  Validators.minLength(5),
      //  Validators.pattern('^(?=.*[a-zA-Z])(?=.*[0-9])[a-zA-Z0-9]+$'),
      //  Validators.required
      // ])),
      quantity: new FormControl(1, Validators.compose([
        Validators.required
      ])),
      birdId: new FormControl('', Validators.compose([
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
  }

  getBirds(): void {
    // TODO: Better implementation of this...
    this.observationService.getBirds()
    .subscribe(birds => { this.birdsSpecies = birds; },
      error => {
        console.log('could not get the birds ddl');
      });
  }

}
