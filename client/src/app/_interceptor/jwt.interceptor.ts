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


// export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
//   const accountServie = inject(AccountService);
//   const router = inject(Router);

//    const currentUser = accountServie.currentUser();
//   const accessToken = currentUser?.token;

//   // Attach token to request
//   if (accessToken && !isTokenExpired(accessToken)) {
//     req = req.clone({
//       setHeaders: {
//         Authorization: `Bearer ${accessToken}`
//       }
//     });
//   }
//   // return next(req);

//   return next(req).pipe(
//     catchError(error => {
//       if (error.status === 401) {
//         // Try refreshing token
//         return accountServie.refreshToken().pipe(
//           switchMap(() => {
//             const newToken = accountServie.currentUser()?.token;
//             const newReq = req.clone({
//               setHeaders: {
//                 Authorization: `Bearer ${newToken}`
//               }
//             });
//             return next(newReq);
//           }),
//           catchError(err => {
//             // Refresh failed - logout
//             accountServie.logout();
//             router.navigate(['/']);
//             return throwError(() => err);
//           })
//         );
//       }

//       return throwError(() => error);
//     })
//   );
// };



// function isTokenExpired(token: string): boolean {
//   try {
//     const payload = JSON.parse(atob(token.split('.')[1]));
//     const exp = payload.exp;
//     if (!exp) return true;

//     const now = Math.floor(Date.now() / 1000);
//     return exp < now;
//   } catch (e) {
//     return true; // Invalid token
//   }
// }

