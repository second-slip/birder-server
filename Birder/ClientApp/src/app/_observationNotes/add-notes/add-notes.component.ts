import { Component, OnInit } from '@angular/core';
import { ObservationNote, ObservationNoteType } from '@app/_models/ObservationNote';

@Component({
  selector: 'app-add-notes',
  templateUrl: './add-notes.component.html',
  styleUrls: ['./add-notes.component.scss']
})
export class AddNotesComponent {
  notes: NoteModel[] = [];
  // keys = Object.keys;
  keys1() : Array<string> { var keys = Object.keys(this.powers); return keys.slice(keys.length / 2); }
  powers = ObservationNoteType; 

  model: NoteModel = new NoteModel('General', '');

  addNote(note: NoteModel): void { 
    this.notes.push(note);
  }

  removeNote(note: NoteModel): void { 
    const i = this.notes.indexOf(note);
    this.notes.splice(i, 1);
  }


  onSubmit() { 
    // this.submitted = true; 
    // console.log(this.model);

    // const note = <ObservationNote> {
    //   id: 0,
    //   noteType: ObservationNoteType[this.model.noteType],
    //   note: this.model.note,
    //   obervationId: 0
    // }

    // console.log(note);

    this.addNote(this.model);

    this.model = new NoteModel('General', '');
  }
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

