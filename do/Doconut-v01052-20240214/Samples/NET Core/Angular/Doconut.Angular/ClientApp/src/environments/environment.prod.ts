export const environment = {
  production: true,
  captcha: {
    key: '',
  },
  api: {
    protocol: 'http',
    baseUrl: 'angular.doconut.com',
  },
  web:
  {
    protocol: 'http',
    baseUrl: 'angular.doconut.com',
  },

  validation: {
    password: '(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])[A-Za-z0-9].{5,256}',
    email: '^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-z]{2,4}$',
  },
  config: {
    protocol: 'http'
  },
  locale: {
    default: 'en',
  },
};
