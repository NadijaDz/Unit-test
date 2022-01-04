import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { CookieService } from 'ngx-cookie-service';
import { LoginService } from './login.service';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  private currentUserSubject: BehaviorSubject<any>;
  public currentUser: Observable<any>;
  cookieValue:any;
  isUserLoggedIn: boolean = false;
  endpoint: string = 'Login';

  constructor(private http: HttpClient, private cookieService: CookieService,  private loginService: LoginService, private router: Router) {
    this.cookieValue = this.cookieService.get('auth_cookie');
    if(this.cookieValue != null && this.cookieValue != undefined && this.cookieValue!= ''){
      this.isUserLoggedIn=true;
    }
    else{
      this.isUserLoggedIn=false;
    }
  }

  public loginWithGoogle() {
    return this.http
      .get<any>(`https://localhost:5001/api/Login/signin-google`, {
        params: new HttpParams().set('provider', 'Google'),
        headers: new HttpHeaders()
          .set('Access-Control-Allow-Headers', 'Content-Type')
          .set('Access-Control-Allow-Methods', 'GET')
          .set('Access-Control-Allow-Origin', '*'),
      })
      .pipe(
        map((data) => {
          return data;
        })
      );
  }

  logout() {
   return this.loginService
      .signout()
      .subscribe(() => {
        this.router.navigate(['/login']);
        this.isUserLoggedIn = false;
      });
  }
}
