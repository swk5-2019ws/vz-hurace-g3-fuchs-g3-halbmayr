import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(protected router: Router, private auth: AuthService) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean|UrlTree> | boolean {
    if (!this.auth.loggedIn) {
      this.router.navigate(['/home'],
      { queryParams: { returnUrl: state.url } });
      return false;
    } else {
      return this.auth.isAuthenticated$.pipe(
        tap(loggedIn => {
          if (!loggedIn) {
            this.auth.login(state.url);
          }
        })
      );
    }
  }
}
