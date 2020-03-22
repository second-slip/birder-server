import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-observations-list',
  templateUrl: './observations-list.component.html',
  styleUrls: ['./observations-list.component.scss']
})
export class ObservationsListComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
    alert('hello');
  }

}
