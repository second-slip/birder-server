import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-observations-feed-filter',
  templateUrl: './observations-feed-filter.component.html',
  styleUrls: ['./observations-feed-filter.component.scss']
})
export class ObservationsFeedFilterComponent implements OnInit {

  constructor(private _router: Router) { }

  ngOnInit() {
  }

  reloadComponent() {
    this._router.routeReuseStrategy.shouldReuseRoute = () => false;
    this._router.onSameUrlNavigation = 'reload';
    this._router.navigate(['/observation-feed']);


  }

}
