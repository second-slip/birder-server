import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-testing',
  templateUrl: './testing.component.html',
  styleUrls: ['./testing.component.scss'],
  encapsulation: ViewEncapsulation.None
})
// export class TestingComponent implements OnInit {
export class TestingComponent {
  id: number;

  constructor(private route: ActivatedRoute) {

    route.params.subscribe(_ => {
      this.route.paramMap.subscribe(pmap => this.hello(+pmap.get('id')));
      
    })
  }

    hello(id: number): void {
      alert(id);
      this.id = id;
    }
  // ngOnInit() { }

}
