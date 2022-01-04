import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  cookieValue:any;
  constructor( private cookieService: CookieService, private router: Router ) { 
    this.cookieValue = this.cookieService.get('auth_cookie');
  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if(this.cookieValue != null && this.cookieValue != undefined && this.cookieValue!= ''){
      return true;
    }
    this.router.navigate(["/recipeCategories"]);
  }
}
