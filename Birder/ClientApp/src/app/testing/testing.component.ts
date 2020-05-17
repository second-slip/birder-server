import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';
import { BirdSummaryViewModel, BirdsDdlDto } from '@app/_models/BirdSummaryViewModel';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { BirdsService } from '@app/_services/birds.service';

@Component({
  selector: 'app-testing',
  templateUrl: './testing.component.html',
  styleUrls: ['./testing.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TestingComponent implements OnInit {
  myControl = new FormControl('');
  options: string[] = ['One', 'Two', 'Three'];
  // filteredOptions: Observable<string[]>;

  filteredOptions: Observable<BirdsDdlDto[]>;
  birdsSpecies: BirdsDdlDto[]

  constructor(private birdsService: BirdsService) { }

  ngOnInit() {
    this.getBirds();
  }

  displayFn(bird: BirdsDdlDto): string {
    // console.log(bird);
    // console.log(bird.englishName);
    return bird && bird.englishName ? bird.englishName : null;
  }

  init() {
    this.filteredOptions = this.myControl.valueChanges.pipe(
      startWith(''),
      map(value => value.length >= 1 ? this._filter(value): this.birdsSpecies)
      // map(value => this._filter(value))
    );
  }

  private _filter(value: string): BirdsDdlDto[] {
    const filterValue = value.toLowerCase();
    

    return this.birdsSpecies.filter(option => option.englishName.toLowerCase().indexOf(filterValue) === 0);
  }

  getBirds(): void {
    this.birdsService.getBirdsDdl()
      .subscribe(
        (data: BirdSummaryViewModel[]) => { 
          this.birdsSpecies = data;
          this.init();
         },
        (error: ErrorReportViewModel) => {
          console.log('could not get the birds ddl');
        });
  }
}
