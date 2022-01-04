import { Component } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { AuthenticationService } from './core/services/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  cookieValue:any;
  isUserLoggedIn: boolean = false;
  constructor(private authenticationService: AuthenticationService) { }

ngOnInit() {
    this.isUserLoggedIn = this.authenticationService.isUserLoggedIn;
}
}
