import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { shareReplay, tap } from 'rxjs/operators';
import { ApiService } from '../../core/service/api/api.service';


@Injectable({
  providedIn: 'root',
})

export class SharedService {

  public isMobile: boolean=false;

  constructor(private apiService: ApiService) {
   
  }

 openDocument(fileName:string): Observable<any> {
   return this.apiService.post<any>(`Home/OpenDocument/${fileName}`, {}).pipe(shareReplay());
  }
 
  checkIsMobile(): boolean {
    if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
      this.isMobile = true;
    }
    return this.isMobile;
  }
}
