import { DOCUMENT } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { LoginService } from 'src/app/core/services/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  constructor(
    private authenticationservice: AuthenticationService,
    @Inject(DOCUMENT) private document: Document,
    private router: Router,
    private loginService: LoginService
  ) {}

  ngOnInit(): void {}

  googleLogIn() {
    let provider = 'provider=Google';
    let returnUrl = 'returnUrl=' + 'http://localhost:4200/recipeCategories';

    this.document.location.href =
      'https://localhost:5001/api/Login/signin-google' +
      '?' +
      provider +
      '&' +
      returnUrl;
  }
}
