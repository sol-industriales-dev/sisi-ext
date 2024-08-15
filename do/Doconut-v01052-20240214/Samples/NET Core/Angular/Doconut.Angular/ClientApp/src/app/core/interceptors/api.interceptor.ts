import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable, of, Subject, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { catchError, switchMap, tap } from 'rxjs/operators';
import { ConfigService } from '../service/config/config.service';

@Injectable()
export class ApiInterceptor implements HttpInterceptor {

  readonly apiUrlBase: string;
  refreshTokenInProgress = false;

  tokenRefreshedSource = new Subject();
  tokenRefreshed$ = this.tokenRefreshedSource.asObservable();

  constructor(
    private router: Router,
    private configService: ConfigService,
  ) {
    const { protocol, baseUrl } = this.configService.getEnv('api');
    this.apiUrlBase = `${protocol}://${baseUrl}`;
  }

  logout() {
    this.router.navigate(['/login']);
  }

  handleResponseError(error: any, request?: HttpRequest<unknown>, next?: HttpHandler): Observable<any> {
    if (error.status === 400) {
     
    } else if (next && request && error.status === 401) {
      return  next.handle(request);
    } 
   
    return throwError(error);
  }

  setAuthHeader() {
    const headersConfig: Record<string, string> = {
      'Content-Type': 'application/json',
      Accept: 'application/json',
    };
    return headersConfig;
  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (request.url.startsWith(this.apiUrlBase)) {
      request = request.clone({ setHeaders: this.setAuthHeader(), withCredentials: true });
      return next.handle(request).pipe(catchError((error: any) => this.handleResponseError(error, request, next)));
    } else {
      return next.handle(request.clone());
    }
  }
}
