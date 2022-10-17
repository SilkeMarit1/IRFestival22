import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PicturesApiService {
  private baseUrl = environment.apiBaseUrl + 'pictures';

  constructor(private httpClient: HttpClient) { }

  getAllUrls(): Observable<string[]> {
    const header = new HttpHeaders().set("Ocp-Apim-Subscription-Key", "a382a4ac17dd422597e3a8662b867add");
    return this.httpClient.get<string[]>(`${this.baseUrl}`, { "headers": header });
  }

  upload(file: File): Observable<never> {
    const data = new FormData();
    data.set('file', file);

    const header = new HttpHeaders().set("Ocp-Apim-Subscription-Key", "a382a4ac17dd422597e3a8662b867add");
    return this.httpClient.post<never>(`${this.baseUrl}`, data, { "headers": header });
  }
}
