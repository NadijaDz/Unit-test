import { Injectable } from '@angular/core';
import {HttpRequest, HttpHandler, HttpEvent, HttpInterceptor} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import {  Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router:Router, private toastr:ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(error => {
        if(error){
          switch(error.status){
            case 400:
              this.toastr.error('Server cannot or will not process the request');
              break;
              case 401:
                this.toastr.error('You are not authorized, please login');
              break;
              case 404:
                this.toastr.error('Not found');
              break;
              case 500:
                this.toastr.error('Something went wrong on server');
              break;
              
              default:
                this.toastr.error('Something unexpected went wrong');
              break;
          }
        }
        return throwError(error);
      })
    )
  }
}
