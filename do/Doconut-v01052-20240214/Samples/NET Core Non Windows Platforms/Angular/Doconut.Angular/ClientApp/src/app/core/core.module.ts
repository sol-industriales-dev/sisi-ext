import { CommonModule, registerLocaleData } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS, HttpClientJsonpModule } from '@angular/common/http';
import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { ApiService } from '../core/service/api/api.service'
import { ConfigService } from '../core/service/config/config.service'
import { ApiInterceptor } from './interceptors/api.interceptor';


@NgModule({
  declarations: [],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    HttpClientJsonpModule,
    BrowserAnimationsModule
  ],
  exports: [BrowserAnimationsModule, CommonModule],
  providers: [
    ApiService,
    ConfigService,
    { provide: HTTP_INTERCEPTORS, useClass: ApiInterceptor, multi: true },
    //CookieService
  ]
})
export class CoreModule {
  constructor(
    @Optional()
    @SkipSelf()
    parentModule: CoreModule
  ) {
    if (parentModule) {
      throw new Error('CoreModule has already been loaded. You should only import Core modules in the AppModule only.');
    }
  }

  static forRoot(): ModuleWithProviders<CoreModule> {
    return {
      ngModule: CoreModule,
      providers: [

      ]
    };
  }
}
