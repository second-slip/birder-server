import { Component, Input, OnInit } from '@angular/core';
import { ObservationNote } from '@app/_models/ObservationNote';

@Component({
  selector: 'app-view-only-notes',
  templateUrl: './view-only-notes.component.html',
  styleUrls: ['./view-only-notes.component.scss']
})
export class ViewOnlyNotesComponent implements OnInit {
  @Input() notes: ObservationNote[];
  
  constructor() { }

  ngOnInit(): void { }
}
