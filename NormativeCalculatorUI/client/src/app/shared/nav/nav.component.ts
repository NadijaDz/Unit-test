import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/app/core/services/authentication.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  cookieValue: any;
  isUserLoggedIn: boolean = false;

  constructor(
    private authenticationService: AuthenticationService
  ) {}

  ngOnInit() {
    this.isUserLoggedIn = this.authenticationService.isUserLoggedIn;
  }

  logout() {
    this.authenticationService.logout();
      this.isUserLoggedIn = false;
  }
}
