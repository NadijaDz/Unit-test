import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { first } from 'rxjs/operators';
import { RecipeCategoriesService } from 'src/app/core/services/recipe-categories.service';

@Component({
  selector: 'app-upsert-recipe-category',
  templateUrl: './upsert-recipe-category.component.html',
  styleUrls: ['./upsert-recipe-category.component.css'],
})
export class UpsertRecipeCategoryComponent implements OnInit {
  recipeCategoryForm: FormGroup;
  isAddMode: boolean;
  categoryId: string;

  constructor(
    private toastr: ToastrService,
    private router: Router,
    private recipeCategoriesService: RecipeCategoriesService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.categoryId = this.route.snapshot.paramMap.get('id');
    this.isAddMode = !this.categoryId;
    this.initializeForm();

    if (!this.isAddMode) {
      this.getByCategoryId();
    }
  }

  getByCategoryId() {
    this.recipeCategoriesService
      .getById(this.categoryId)
      .pipe(first())
      .subscribe((x) => this.recipeCategoryForm.patchValue(x));
  }

  initializeForm() {
    this.recipeCategoryForm = new FormGroup({
      categoryName: new FormControl('', Validators.required),
    });
  }

  get form() {
    return this.recipeCategoryForm.controls;
  }

  onSubmit() {
    if (this.recipeCategoryForm.invalid) {
      return;
    }

    if (this.isAddMode) {
      this.addNewCategory();
    } else {
      this.updateCategory();
    }
  }

  addNewCategory() {
    this.recipeCategoriesService
      .save(this.recipeCategoryForm.value)
      .pipe(first())
      .subscribe(
        (data) => {
          this.toastr.success('Data is successfully saved!', 'Success!');
          this.router.navigate(['/recipeCategories']);
        },
        (error) => {
          this.toastr.error('Something went wrong', 'Error!');
        }
      );
  }

  updateCategory() {
    this.recipeCategoriesService
      .update(this.categoryId, this.recipeCategoryForm.value)
      .pipe(first())
      .subscribe(
        (data) => {
          this.toastr.success('Data is successfully saved!', 'Success!');
          this.router.navigate(['/recipeCategories']);
        },
        (error) => {
          this.toastr.error('Something went wrong', 'Error!');
        }
      );
  }
}

