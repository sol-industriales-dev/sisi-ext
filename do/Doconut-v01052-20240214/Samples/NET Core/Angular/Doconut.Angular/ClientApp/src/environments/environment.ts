// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  captcha: {
    key: '',
  },
  api: {
    protocol: 'https',
    baseUrl: 'localhost:44348',
  },
  web:
  {
    protocol: 'http',
    baseUrl: 'localhost:4200',
  },
 
  validation: {
    password: '(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])[A-Za-z0-9].{5,256}',
    email: '^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-z]{2,4}$',
  },
  config: {
    protocol: 'http',
    //baseUrl: 'localhost:5000/swagger/v1/swagger.json',
  },
  locale: {
    default: 'en',
  },
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
