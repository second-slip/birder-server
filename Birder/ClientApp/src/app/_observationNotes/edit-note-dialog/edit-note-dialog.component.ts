import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ObservationNoteType } from '@app/_models';
import { ObservationNoteModel } from '@app/_observationNotes/ObservationNoteModel';


@Component({
  selector: 'app-edit-note-dialog',
  templateUrl: './edit-note-dialog.component.html',
  styleUrls: ['./edit-note-dialog.component.scss']
})
export class EditNoteDialogComponent implements OnInit {
  keys1(): Array<string> { var keys = Object.keys(this.powers); return keys.slice(keys.length / 2); }
  powers = ObservationNoteType;

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: ObservationNoteModel) { }

  ngOnInit(): void {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
