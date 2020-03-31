import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'environments/environment';
import { FlickrUrlsViewModel } from './_models/FlickrUrlsViewModel';

@Injectable({
  providedIn: 'root'
})
export class FlickrService {
  apiKey = environment.flickrApiKey;
  apiUrl = environment.flickrApiUrl;
  baseUrl = `${this.apiUrl}?api_key=${this.apiKey}&format=json&nojsoncallback=1&method=flickr.photos.`;
  flickrPhotoSearch = `${this.baseUrl}search&per_page=20&tags=`;

  constructor(private http: HttpClient) { }

  getSearchResults(page: number, term = null): Observable<{}> {
    // const tagMode = this.tagModeAll ? '&tag_mode=all' : '';
    // this.newTerm = term ? term : this.newTerm;
    // return this.getFlickrPhotoSearch(term ? term : this.newTerm, page, tagMode);
    return this.getFlickrPhotoSearch(term, page, '');
  }

  getFlickrPhotoSearch(term, page, tagMode) {
    return this.http.get(`${this.flickrPhotoSearch}${encodeURIComponent(term)}&page=${page}${tagMode}`);
  }

  getPhotoThumnail(data): FlickrUrlsViewModel[] {
    const urls: FlickrUrlsViewModel[] = [];

    data.forEach((element, index) => {
      urls.push({
        id: index + 1,
        url: `https://farm${element.farm}.staticflickr.com/${element.server}/${element.id}_${element.secret}_n.jpg`
      });
    });

    return urls;
  }
}
