import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { NoteModel } from '@app/_models/NoteModel';
import { AddNoteDialogComponent } from '../add-note-dialog/add-note-dialog.component';

@Component({
  selector: 'app-edit-test',
  templateUrl: './edit-test.component.html',
  styleUrls: ['./edit-test.component.scss']
})
export class EditTestComponent implements OnInit {
  notes: NoteModel[] = [];

  constructor(public dialog: MatDialog) { }

  ngOnInit(): void { }

  openDialog(): void {
    const dialogRef = this.dialog.open(AddNoteDialogComponent, {
      width: '300px',
      data: new NoteModel(0, 'General', '')
    });

    dialogRef.afterClosed().subscribe(result => {
      // console.log('The dialog was closed');
      if (result) {
        this.addNote(result);
      }
    });
  }

  addNote(note: NoteModel): void {
    this.notes.push(note);
  }

  removeNote(note: NoteModel): void {
    const i = this.notes.indexOf(note);
    this.notes.splice(i, 1);
  }
}
