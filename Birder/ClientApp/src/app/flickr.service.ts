import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FlickrService {
  apiKey = environment.flickrApiKey;
  apiUrl = environment.flickrApiUrl;
  baseUrl = `${this.apiUrl}?api_key=${this.apiKey}&format=json&nojsoncallback=1&method=flickr.photos.`;
  flickrPhotoSearch = `${this.baseUrl}search&per_page=20&tags=`;
  // flickrPhotoGetInfo = `${this.baseUrl}getInfo&photo_id=`;
  // newTerm: string;
  // tagModeAll: false;

  constructor(private http: HttpClient) { }

  getSearchResults(page: number, term = null): Observable<{}> {
    // const tagMode = this.tagModeAll ? '&tag_mode=all' : '';
    // console.log(tagMode);
    // this.newTerm = term ? term : this.newTerm;
    // return this.httpService.getFlickrPhotoSearch(term ? term : this.newTerm, page, tagMode);
    // return this.getFlickrPhotoSearch(term ? term : this.newTerm, page, tagMode);
    return this.getFlickrPhotoSearch(term, page, '');
  }

  getFlickrPhotoSearch(term, page, tagMode) {
    return this.http.get(`${this.flickrPhotoSearch}${encodeURIComponent(term)}&page=${page}${tagMode}`);
  }

  getPhotoThumnail(data): FlickrUrls[] {
    const uls: FlickrUrls[] = [];

    // for (let i = 0; i < data.length; i++) {
    //   gq.push({
    //     id: i + 1,
    //     url: `https://farm${data.farm}.staticflickr.com/${data.server}/${data.id}_${data.secret}_n.jpg`
    //   });
    // }

    data.forEach((element, index) => {
      uls.push({
        id: index + 1,
        url: `https://farm${element.farm}.staticflickr.com/${element.server}/${element.id}_${element.secret}_n.jpg`
      });
    });

    // console.log(uls);

    return uls;
  }
}

export interface FlickrUrls {
  id: number;
  url: string;
}
