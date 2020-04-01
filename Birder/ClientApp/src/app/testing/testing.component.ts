import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-testing',
  templateUrl: './testing.component.html',
  styleUrls: ['./testing.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TestingComponent implements OnInit {
  species = '';
  baseUrl = 'https://www.xeno-canto.org/api/2/recordings?query='; // =cnt:brazil';
  // searchString = 'flickrPhotoSearch = `${this.baseUrl}search&per_page=20&tags=`;';
  recordingLength = '+len_gt:40';

  tre: Xeno;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getFlickrPhotoSearch('troglodytes troglodytes')
    .subscribe((results: any) => {
      this.tre = results;
      console.log(results);
    });
  }


  getFlickrPhotoSearch(term: string): Observable<Xeno> {
    console.log(`${encodeURIComponent(term)}`);

    const replaced = term.split(' ').join('+');

    console.log(`${this.baseUrl}${replaced}${this.recordingLength}`);

    return this.http.get<Xeno>(`${this.baseUrl}${replaced}${this.recordingLength}`)
    .pipe(
      map(o => ({ // IProduct specified here ensures we get excess property checks
        numRecordings: o.numRecordings, // number in server interface, map to string 
        numSpecies: o.numSpecies,
        page: o.page,
        numPages: o.numPages,
        recordings: o.recordings
    }))




      // map((response: Xeno) => {

      //     =  <IObjOwner>response.data.owner
      //    this.contacts =  <Array<IOwnerContacts>>response.data.contacts
      //    this.locations = <IOwnerLocation>response.data.location
      // })
  );
  


    // return this.http.get(`${this.baseUrl}${encodeURIComponent(term)}`);
  }

}

export interface Xeno {
  numRecordings: string;
  numSpecies: string;
  page: string;
  numPages: string;
  recordings: [];
}

