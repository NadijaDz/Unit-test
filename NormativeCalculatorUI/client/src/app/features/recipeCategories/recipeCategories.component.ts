import { Component, OnInit } from '@angular/core';
import { NgxBootstrapConfirmService } from 'ngx-bootstrap-confirm';
import { ToastrService } from 'ngx-toastr';
import { first } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { RecipeCategoriesService } from 'src/app/core/services/recipe-categories.service';

@Component({
  selector: 'app-recipe-categories',
  templateUrl: './recipeCategories.component.html',
  styleUrls: ['./recipeCategories.component.css'],
})
export class RecipeCategoriesComponent implements OnInit {
  recipeCategories: any = [];
  recipes: any = [];
  request: any[] = [];
  skip: number = 0;
  isLodaMore: boolean = true;
  isUserLoggedIn: boolean = false;

  constructor(
    private recipeCategoriesService: RecipeCategoriesService,
    private authenticationService: AuthenticationService,
    private ngxBootstrapConfirmService: NgxBootstrapConfirmService,
    private toastr: ToastrService,
  ) {}

  ngOnInit() {
    this.getRecipeCategories();
    this.isUserLoggedIn = this.authenticationService.isUserLoggedIn;
  }

  getRecipeCategories() {
    this.recipeCategoriesService
      .get(this.skip)
      .pipe(first())
      .subscribe((response: any) => {
        response.data.forEach((val) =>
          this.recipeCategories.push(Object.assign({}, val))
        );
        if (this.recipeCategories.length == response.totalCount) {
          this.isLodaMore = false;
        }
      });
  }

  loadMore() {
    this.skip = this.recipeCategories.length;
    this.getRecipeCategories();
  }

  delete(id){
    let options ={
      title: 'Are you sure you want to delete?',
      confirmLabel: 'Delete',
      declineLabel: 'Cancel',
    }
    this.ngxBootstrapConfirmService.confirm(options).then((res: boolean) => {
      if(res){
        this.recipeCategoriesService
        .delete(id)
        .pipe(first())
        .subscribe((response: any) => {
          this.toastr.success('Data is successfully deleted!', 'Success!');
          this.recipeCategories = this.recipeCategories.filter(item=> item.id != response.id);
        },
        (error) => {
          this.toastr.error('Something went wrong', 'Error!');
        }
      );
      }
    });
  }
}
