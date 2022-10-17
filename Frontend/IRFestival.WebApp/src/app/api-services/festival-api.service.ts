import { Injectable } from '@angular/core';
import { Schedule } from '../api/models/schedule.model';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Artist } from '../api/models/artist.model';

@Injectable({
  providedIn: 'root'
})
export class FestivalApiService {
  private baseUrl = environment.apiBaseUrl + 'festival';

  constructor(private httpClient: HttpClient) { }

  getSchedule(): Observable<Schedule> {
    const header = new HttpHeaders().set("Ocp-Apim-Subscription-Key", "a382a4ac17dd422597e3a8662b867add");
    return this.httpClient.get<Schedule>(`${this.baseUrl}/lineup`, { "headers": header });
  }

  getArtists(): Observable<Artist[]> {
    const header = new HttpHeaders().set("Ocp-Apim-Subscription-Key", "a382a4ac17dd422597e3a8662b867add");
    return this.httpClient.get<Artist[]>(`${this.baseUrl}/artists`, { "headers": header });
  }
}
