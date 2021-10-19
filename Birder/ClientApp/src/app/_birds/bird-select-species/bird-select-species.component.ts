import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';
import { BirdSummaryViewModel } from '@app/_models/BirdSummaryViewModel';

import { Router } from '@angular/router';
import { BirdsService } from '@app/_services/birds.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BirdsListValidator } from '@app/_validators';

@Component({
  selector: 'app-bird-select-species',
  templateUrl: './bird-select-species.component.html',
  styleUrls: ['./bird-select-species.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class BirdSelectSpeciesComponent implements OnInit {
  selectSpeciesForm: FormGroup;
  birdsSpecies: BirdSummaryViewModel[]
  filteredOptions$: Observable<BirdSummaryViewModel[]>;

  // selectSpeciesForm_validation_messages = {
  //   'bird': [
  //     //{ type: 'required', message: 'The observed species is required' },
  //     { type: 'notBirdListObject', message: 'You must select a bird species from the list.' }
  //   ]
  // };

  constructor(private birdsService: BirdsService, private formBuilder: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    this.getBirds();
    
  }

  onSubmit(selectedSpecies: any) {
    this.router.navigate(['/bird-detail/' + selectedSpecies.bird.birdId]);
  }

  createForms(): void {
    this.selectSpeciesForm = this.formBuilder.group({
      bird: new FormControl('', Validators.compose([
        //Validators.required,
        BirdsListValidator()
      ])),
    });
    //this.getBirdAutocompleteOptions();
  }

  getBirds(): void {
    this.birdsService.getBirdsDdl()
      .subscribe(
        (data: BirdSummaryViewModel[]) => {
          this.birdsSpecies = data;
          this.createForms();
          this.getBirdAutocompleteOptions();
        },
        (error: any) => {
          console.log('could not get the birds ddl');
        }
      );
  }

  getBirdAutocompleteOptions() {
    //console.log('getBirdAutocompleteOptions');
    this.filteredOptions$ = this.selectSpeciesForm.controls['bird'].valueChanges
      .pipe(
        startWith(''),
        map(value => value.length >= 1 ? this._filter(value) : this.birdsSpecies)
      );
  }

  displayFn(bird: BirdSummaryViewModel): string {
    return bird && bird.englishName ? bird.englishName : null;
  }

  private _filter(value: string): BirdSummaryViewModel[] {
    //console.log('_filter');
    const filterValue = value.toLowerCase();
    return this.birdsSpecies.filter(option => option.englishName.toLowerCase().indexOf(filterValue) >= 0); // or !== 11
  }
}
