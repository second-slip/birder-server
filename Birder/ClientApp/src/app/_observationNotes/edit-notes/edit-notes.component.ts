import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ObservationNoteModel } from '@app/_observationNotes/ObservationNoteModel';
import { AddNoteDialogComponent } from '../add-note-dialog/add-note-dialog.component';
import { EditNoteDialogComponent } from '../edit-note-dialog/edit-note-dialog.component';

@Component({
  selector: 'app-edit-notes',
  templateUrl: './edit-notes.component.html',
  styleUrls: ['./edit-notes.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class EditNotesComponent implements OnInit {
  @Input() notes: ObservationNoteModel[] = [];

  constructor(public dialog: MatDialog) { }

  ngOnInit(): void { }

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
}

