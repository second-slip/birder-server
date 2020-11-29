import { Component } from '@angular/core';
import { NoteModel } from '@app/_models/NoteModel';
import { ObservationNoteType } from '@app/_models/ObservationNote';

@Component({
  selector: 'app-add-notes',
  templateUrl: './add-notes.component.html',
  styleUrls: ['./add-notes.component.scss']
})
export class AddNotesComponent {
  notes: NoteModel[] = [];
  // keys = Object.keys;
  keys1(): Array<string> { var keys = Object.keys(this.powers); return keys.slice(keys.length / 2); }
  powers = ObservationNoteType;

  model: NoteModel = new NoteModel(0, 'General', '');

  addNote(note: NoteModel): void {
    this.notes.push(note);
  }

  removeNote(note: NoteModel): void {
    const i = this.notes.indexOf(note);
    this.notes.splice(i, 1);
  }


  onSubmit() {
    this.addNote(this.model);

    this.model = new NoteModel(0, 'General', '');
  }
}
