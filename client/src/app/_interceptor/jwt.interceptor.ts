import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AccountService } from '../_services/account.service';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const accountServie = inject(AccountService);

  req = req.clone({
    setHeaders:{
      Authorization: `Bearer ${accountServie.currentUser()?.token}`
    }
  })
  return next(req);
};
