import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { NoteModel } from '@app/_models/NoteModel';
import { AddNoteDialogComponent } from '../add-note-dialog/add-note-dialog.component';
import { EditNoteDialogComponent } from '../edit-note-dialog/edit-note-dialog.component';

@Component({
  selector: 'app-add-notes',
  templateUrl: './add-notes.component.html',
  styleUrls: ['./add-notes.component.scss']
})
export class AddNotesComponent {
  notes: NoteModel[] = [];

  constructor(public dialog: MatDialog) { }

  ngOnInit(): void { }

  openAddNoteDialog(): void {
    const dialogRef = this.dialog.open(AddNoteDialogComponent, {
      width: '325px',
      data: new NoteModel(0, 'General', '')
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.addNote(result);
      }
    });
  }

  openEditNoteDialog(note: NoteModel): void {
    const dialogRef = this.dialog.open(EditNoteDialogComponent, {
      width: '325px',
      data: note
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.editNote(result);
      }
    });
  }

  addNote(note: NoteModel): void {
    this.notes.push(note);
  }

  editNote(note: NoteModel): void {
    // var foundIndex = items.findIndex(x => x.id == item.id);
    const i = this.notes.indexOf(note);
    this.notes[i] = note;
  }

  removeNote(note: NoteModel): void {
    const i = this.notes.indexOf(note);
    this.notes.splice(i, 1);
  }



  // keys = Object.keys;
  // keys1(): Array<string> { var keys = Object.keys(this.powers); return keys.slice(keys.length / 2); }
  // powers = ObservationNoteType;

  // model: NoteModel = new NoteModel(0, 'General', '');

  // addNote(note: NoteModel): void {
  //   this.notes.push(note);
  // }

  // removeNote(note: NoteModel): void {
  //   const i = this.notes.indexOf(note);
  //   this.notes.splice(i, 1);
  // }


  // onSubmit() {
  //   this.addNote(this.model);

  //   this.model = new NoteModel(0, 'General', '');
  // }
}
