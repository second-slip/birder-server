import { Component, OnInit } from '@angular/core';
import { ObservationNote, ObservationNoteType } from '@app/_models/ObservationNote';

@Component({
  selector: 'app-add-notes',
  templateUrl: './add-notes.component.html',
  styleUrls: ['./add-notes.component.scss']
})
export class AddNotesComponent implements OnInit {
  notes: ObservationNote[] = [];
  //
  // model: ObservationNote;
  keys = Object.keys;
  keys1() : Array<string> { var keys = Object.keys(this.powers); return keys.slice(keys.length / 2); }
  powers = ObservationNoteType; // ['Really Smart', 'Super Flexible',
  //'Super Hot', 'Weather Changer'];

  model = new NoteModel('General', '');

  constructor() { }

  ngOnInit(): void {
  }

  addNote(note: ObservationNote): void { 
    this.notes.push(note);
  }

  removeNote(note: ObservationNote): void { 
    const i = this.notes.indexOf(note);
    this.notes.splice(i);
  }



  submitted = false;

  onSubmit() { 
    // this.submitted = true; 
    console.log(this.keys1);
    console.log(this.model);
    this.model = new NoteModel('General', '');
  }

  // TODO: Remove this when we're done
  get diagnostic() { return JSON.stringify(this.model); }

  // newHero() {
  //   this.model = new NoteModel(ObservationNoteType.General, '');
  // }

  // skyDog(): Hero {
  //   const myHero =  new Hero(42, 'SkyDog',
  //                          'Fetch any object at any distance',
  //                          'Leslie Rollover');
  //   console.log('My hero is called ' + myHero.name); // "My hero is called SkyDog"
  //   return myHero;
  // }

  //////// NOT SHOWN IN DOCS ////////

  // Reveal in html:
  //   Name via form.controls = {{showFormControls(heroForm)}}
  showFormControls(form: any) {
    return form && form.controls.note &&
    form.controls.note.value; // Dr. IQ
  }

  /////////////////////////////

}

export class NoteModel {
  constructor(
    // public id: number,
    public noteType: string,
    public note: string,
  ) {  }

}

// export interface ObservationNote {
//   id: number;
//   noteType: ObservationNoteType; ///????
//   note: string;
//   obervationId: number;
//   // observation: ObservationViewModel; ????
// }

export class Hero {
  constructor(
    public id: number,
    public name: string,
    public power: string,
    public alterEgo?: string
  ) {  }
}
