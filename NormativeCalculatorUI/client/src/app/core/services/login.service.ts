import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class LoginService {
  endpoint: string = 'Login';

  constructor(private http: HttpClient) {}

  signout() {
    return this.http.get<any[]>(
      `${environment.apiUrl}` + this.endpoint + '/signout-google',
      { withCredentials: true } //cuva cookie
    );
  }
  
  signin() {
    return this.http.get<any[]>(
      `${environment.apiUrl}` + this.endpoint + '/signin-google',
      { withCredentials: true }
    );
  }
}
