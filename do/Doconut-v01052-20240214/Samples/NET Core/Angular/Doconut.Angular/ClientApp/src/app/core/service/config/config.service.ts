import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ConfigService {

  private readonly config: any;
  private readonly env: any;
  private ready = false;

  constructor(private http: HttpClient) {
    this.config = {};
    this.env = environment;
  }

  public getConfig(key: string): any {
    return this.config[key];
  }

  public getEnv(key: string): any {
    return this.env[key];
  }

  /**
   * Indicates if config is ready.
   */
  public isReady(): boolean {
    return this.ready;
  }

  
}
