import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-whats-new',
  templateUrl: './whats-new.component.html',
  styleUrls: ['./whats-new.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class WhatsNewComponent implements OnInit {
  features: IFeatures;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getJSON().subscribe(data => {
      this.features = data;
      console.log(data);
    });
  }

  getJSON(): Observable<IFeatures> {
    return this.http.get<IFeatures>("./assets/features.json");
  }
}


export interface IFeatures {
  id: number;
  feature: string;
  description: string;
  progress: string;
  priority: string;
  colourCode: string;
}
