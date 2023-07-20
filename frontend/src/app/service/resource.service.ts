import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs/internal/Observable';
import { resource } from '../Models/resource';

@Injectable({
  providedIn: 'root',
})
export class ResourcesService {

  baseApiUrl:  string = environment.baseApiUrl;

  constructor(private http: HttpClient) {}

  getAllResources(): Observable<resource[]> {
    return this.http.get<resource[]>(this.baseApiUrl + '/api/Resource');
  }
}
