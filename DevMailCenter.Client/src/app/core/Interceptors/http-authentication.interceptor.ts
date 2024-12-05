import {HttpInterceptorFn} from '@angular/common/http';

export const authenticationInterceptor: HttpInterceptorFn = (req, next) => {
  const username = 'admin'; // TODO Get username from cache
  const password = "admin"; // TODO Get password from cache

  // Clone the request and add the authorization header
  const authReq = req.clone({
    setHeaders: {
      Authorization: `Basic ${btoa(`${username}:${password}`)}`
    }
  });

  // Pass the cloned request with the updated header to the next handler
  return next(authReq);
};
