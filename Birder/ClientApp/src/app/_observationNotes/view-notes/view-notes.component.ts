import { Component, Input, OnInit } from '@angular/core';
import { ObservationNote } from '@app/_models/ObservationNote';

@Component({
  selector: 'app-view-notes',
  templateUrl: './view-notes.component.html',
  styleUrls: ['./view-notes.component.scss']
})
export class ViewNotesComponent implements OnInit {
  @Input() notes: ObservationNote[];
  
  constructor() { }

  ngOnInit(): void {
    console.log(this.notes);
  }

}
