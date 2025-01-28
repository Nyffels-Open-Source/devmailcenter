import { Injectable } from "@angular/core";
import {ActivatedRouteSnapshot, Router, RouterStateSnapshot} from '@angular/router';
import {Observable, of} from 'rxjs';

@Injectable()
export class AuthenticatedGuard {
  constructor(private router: Router) {
  }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    const auth = localStorage.getItem('authlgn');
    if (!auth) {
      this.router.navigate(['/authentication/login'], {replaceUrl: true});
      return of(false);
    } else {
      return of(true);
      // TODO
    }
  }
}
