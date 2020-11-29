import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { EditTestComponent } from '@app/_observationNotes/edit-test/edit-test.component';

@Component({
  selector: 'app-testing',
  templateUrl: './testing.component.html',
  styleUrls: ['./testing.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TestingComponent implements OnInit {
  @ViewChild(EditTestComponent)
  private editNotesComponent: EditTestComponent;

  constructor() { }

  ngOnInit() { }

}
