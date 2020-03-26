import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { environment } from 'environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-testing',
  templateUrl: './testing.component.html',
  styleUrls: ['./testing.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TestingComponent implements OnInit {

  apiKey = environment.flickrApiKey;
  apiUrl = environment.flickrApiUrl;
  baseUrl = `${this.apiUrl}?api_key=${this.apiKey}&format=json&nojsoncallback=1&method=flickr.photos.`;
  flickrPhotoSearch = `${this.baseUrl}search&per_page=20&tags=`;
  flickrPhotoGetInfo = `${this.baseUrl}getInfo&photo_id=`;

  newTerm: string;
  tagModeAll: false;

  currentResults: any[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit() {

    this.x();
  }

  x() {
    // call this after successful getBird() request
    this.getSearchResults(1, 'Alca arctica')
    .subscribe((results: any) => {
      // console.log(results);

      // console.log(results.photos.photo);

      this.currentResults = this.getPhotoThumnail(results.photos.photo);
      // console.log(this.currentResults);

    });
  }

  getSearchResults(page: number, term = null): Observable<{}> {
    const tagMode = this.tagModeAll ? '&tag_mode=all' : '';
    this.newTerm = term ? term : this.newTerm;
    // return this.httpService.getFlickrPhotoSearch(term ? term : this.newTerm, page, tagMode);
    return this.getFlickrPhotoSearch(term ? term : this.newTerm, page, tagMode);
  }

  getFlickrPhotoSearch(term, page, tagMode) {
    return this.http.get(`${this.flickrPhotoSearch}${encodeURIComponent(term)}&page=${page}${tagMode}`);
  }

  // formatResults(response): Photo[] {
  //   return Object.keys(response).reduce((results, item: string, i) => {
  //     results[i] = this.formatPhoto(response[item].photo);
  //     return results;
  //   }, []);
  // }

  // formatPhoto(data): Photo {
  //   return {
  //     // thumbUrl: this.httpService.getPhotoThumnail(data),
  //     thumbUrl: this.getPhotoThumnail(data),
  //     link: data.urls.url[0]._content,
  //     title: data.title._content,
  //     description: data.description._content,
  //     tags: data.tags.tag.map(v => v.raw),
  //     owner: data.owner.realname,
  //     dateTaken: data.dates.taken.split(' ')[0],
  //   };
  // }

  getPhotoThumnail(data) {
    let g: any[] = [];
    data.forEach(element => {
      g.push(`https://farm${element.farm}.staticflickr.com/${element.server}/${element.id}_${element.secret}_n.jpg`);
    });
    console.log(g);
    return g;
    // return `https://farm${data.farm}.staticflickr.com/${data.server}/${data.id}_${data.secret}_n.jpg`;
  }

}


export interface Photo {
  thumbUrl: string;
  link: string;
  title: string;
  tags: string[];
  description: string;
  owner: string;
  dateTaken: Date;
}
