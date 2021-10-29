import { Component, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddNoteDialogComponent } from '../add-note-dialog/add-note-dialog.component';
import { EditNoteDialogComponent } from '../edit-note-dialog/edit-note-dialog.component';
import { ObservationNoteModel } from '../ObservationNoteModel';

@Component({
  selector: 'app-add-notes',
  templateUrl: './add-notes.component.html',
  styleUrls: ['./add-notes.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AddNotesComponent {
  notes: ObservationNoteModel[] = [];

  constructor(public dialog: MatDialog) { }

  ngOnInit(): void { }

  openAddNoteDialog(): void {
    const dialogRef = this.dialog.open(AddNoteDialogComponent, {
      width: '325px',
      data: new ObservationNoteModel(0, 'General', '')
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.addNote(result);
      }
    });
  }

  openEditNoteDialog(note: ObservationNoteModel): void {
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

  addNote(note: ObservationNoteModel): void {
    this.notes.push(note);
  }

  editNote(note: ObservationNoteModel): void {
    // var foundIndex = items.findIndex(x => x.id == item.id);
    const i = this.notes.indexOf(note);
    this.notes[i] = note;
  }

  removeNote(note: ObservationNoteModel): void {
    const i = this.notes.indexOf(note);
    this.notes.splice(i, 1);
  }



  // keys = Object.keys;
  // keys1(): Array<string> { var keys = Object.keys(this.powers); return keys.slice(keys.length / 2); }
  // powers = ObservationNoteType;

  // model: ObservationNoteModel = new ObservationNoteModel(0, 'General', '');

  // addNote(note: ObservationNoteModel): void {
  //   this.notes.push(note);
  // }

  // removeNote(note: ObservationNoteModel): void {
  //   const i = this.notes.indexOf(note);
  //   this.notes.splice(i, 1);
  // }


  // onSubmit() {
  //   this.addNote(this.model);

  //   this.model = new ObservationNoteModel(0, 'General', '');
  // }
}
