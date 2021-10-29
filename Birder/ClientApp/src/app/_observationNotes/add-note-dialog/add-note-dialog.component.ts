import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ObservationNoteType } from '@app/_models';
import { ObservationNoteModel } from '../ObservationNoteModel';

@Component({
  selector: 'app-add-note-dialog',
  templateUrl: './add-note-dialog.component.html',
  styleUrls: ['./add-note-dialog.component.scss']
})
export class AddNoteDialogComponent implements OnInit {
  keys1() : Array<string> { var keys = Object.keys(this.powers); return keys.slice(keys.length / 2); }
  powers = ObservationNoteType; 

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public model: ObservationNoteModel) { }

  ngOnInit(): void {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
