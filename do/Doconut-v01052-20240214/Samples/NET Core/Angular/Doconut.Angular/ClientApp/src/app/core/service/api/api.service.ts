import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';


import { catchError } from 'rxjs/operators';
import { ConfigService } from '../config/config.service';

@Injectable({
  providedIn: 'root',
})
export class ApiService {

  baseUrl: string;

  constructor(
    private http: HttpClient, private configService: ConfigService,
  ) {
    const { protocol, baseUrl } = configService.getEnv('api');
    this.baseUrl = `${protocol}://${baseUrl}/`;
  }

  get<T>(path: string, params: HttpParams = new HttpParams()): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}${path}`, { params })
      .pipe(catchError(this.formatErrors));
  }
    
  put<T>(path: string, body: any = {}): Observable<T> {
    return this.http.put<T>(
      `${this.baseUrl}${path}`,
      JSON.stringify(body),
    ).pipe(catchError(this.formatErrors));
  }

  post<T>(path: string, body: any = {}): Observable<T> {
    return this.http.post<T>(
      `${this.baseUrl}${path}`,
      JSON.stringify(body),
    ).pipe(catchError(this.formatErrors));
  }

  delete<T>(path: string): Observable<T> {
    return this.http.delete<T>(
      `${this.baseUrl}${path}`,
    ).pipe(catchError(this.formatErrors));
  }

  private formatErrors(error: any) {
    return throwError(error.error);
  }
}
