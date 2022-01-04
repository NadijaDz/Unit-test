import { Component, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { IngredientsService } from 'src/app/core/services/ingredients.service';
import { RecipesService } from 'src/app/core/services/recipes.service';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { Ingredient } from 'src/app/core/models/ingredient.model';
import { MeasureUnit } from 'src/app/core/models/measure-unit.model';
import { UpsertIngredientComponent } from '../../ingredients/upsert-ingredients/upsert-ingredient.component';

@Component({
  selector: 'app-upsert-recipe',
  templateUrl: './upsert-recipe.component.html',
  styleUrls: ['./upsert-recipe.component.css'],
})
export class UpsertRecipeComponent implements OnInit {
  recipeForm: FormGroup;
  categoryId: string;
  ingredients: Ingredient[] = [];
  measureUnits = MeasureUnit;
  enumKeys = [];
  totalCost: any = 0;
  nameofCategory: string;
  closeResult: string;
  modalOptions: NgbModalOptions;
  disabledMeasureUnits: boolean[] = new Array(
    false,
    false,
    false,
    false,
    false
  );
  ingredientEumKeys: any = [];
  isAddMode: boolean;
  recipeId: string;

  constructor(
    private route: ActivatedRoute,
    private ingredientsService: IngredientsService,
    private recipesService: RecipesService,
    private router: Router,
    private modalService: NgbModal,
    private toastr: ToastrService
  ) {
    this.enumKeys = Object.keys(this.measureUnits).filter(
      (f) => !isNaN(Number(f))
    );
  }

  ngOnInit() {
    this.categoryId = this.route.snapshot.paramMap.get('id');
    this.nameofCategory = this.route.snapshot.paramMap.get('name');
    this.recipeId = this.route.snapshot.paramMap.get('recipeId');
    this.isAddMode = !this.recipeId;
    this.initializeForm();
    this.getIngredients();
    if (!this.isAddMode) {
      this.getByRecipeId();
    }
  }

  initializeForm() {
    this.recipeForm = new FormGroup({
      name: new FormControl('', Validators.required),
      description: new FormControl('', Validators.required),
      ingredients: new FormArray([], Validators.required),
      recommendedPrice: new FormControl(null, Validators.required),
    });
  }

  getByRecipeId(){
    this.recipesService
    .getById(this.recipeId)
    .pipe(first())
    .subscribe((response) => {
      //add form controls for each ingredient row for push control
      response["recipeIngredient"].forEach((element, index) => {
        this.addNewIngredientRow();
      });

      this.recipeForm.patchValue({
        ingredients: response["recipeIngredient"],
        name: response["name"],
        description: response["description"],
        recommendedPrice: response ["recommendedPrice"]  
      })

      //after patch values need filter measure unit dropdowns and calculated total cost for ing
      setTimeout(() => {
        response["recipeIngredient"].forEach((element, index) => {
          this.changeMeasureUnit(index);
          this.totalCostPerIngredient(index);
        });
        }, 500);
    
    });
  }

  get form() {
    return this.recipeForm.controls;
  }

  onSubmit() {
    if (this.recipeForm.invalid) {
      return;
    }

    if (this.isAddMode) {
      this.addNewRecipe();
    } else {
      this.updateRecipe();
    }
  }

  addNewRecipe(){
    this.recipeForm.value.recipeCategoryId = this.categoryId;
    this.recipesService
      .save(this.recipeForm.value)
      .pipe(first())
      .subscribe(
        (data) => {
          this.toastr.success('Data is successfully saved!', 'Success!');
          this.router.navigate([
            '/recipes/' + this.categoryId + '/' + this.nameofCategory,
          ]);
        },
        (error) => {
          this.toastr.error('Something went wrong', 'Error!');
        }
      );
  }

  updateRecipe(){
    this.recipeForm.value.recipeCategoryId = this.categoryId;
    this.recipesService
      .update(this.recipeId, this.recipeForm.value)
      .pipe(first())
      .subscribe(
        (data) => {
          this.toastr.success('Data is successfully saved!', 'Success!');
          this.router.navigate([
            '/recipes/' + this.categoryId + '/' + this.nameofCategory,
          ]);
        },
        (error) => {
          this.toastr.error('Something went wrong', 'Error!');
        }
      );
  }

  getIngredients() {
    this.ingredientsService
      .get()
      .pipe(first())
      .subscribe((response: any) => {
        this.ingredients = response;
      });
  }

  addNewIngredientRow() {
    (<FormArray>this.recipeForm.get('ingredients')).push(
      new FormGroup({
        ingredientId: new FormControl(null, Validators.required),
        measureUnit: new FormControl(null, Validators.required),
        unitQuantity: new FormControl(null, Validators.required),
        costIngredient: new FormControl(0),
      })
    );
    this.ingredientEumKeys.push([]);
  }

  // Add New Ingredient Input in the form
  addIngredientInForm() {
    return (<FormArray>this.recipeForm.get('ingredients')).controls;
  }

  deleteIngredient(index: number) {
    this.totalCost -=
      this.recipeForm.get('ingredients')['controls'][index][
        'controls'
      ].costIngredient.value;
    (<FormArray>this.recipeForm.get('ingredients')).removeAt(index);
    //splice remove 1 item on index position
    this.ingredientEumKeys.splice(index,1);
  }

  totalCostPerIngredient(index) {
    var ingredientId =
      this.recipeForm.get('ingredients')['controls'][index]['controls']
        .ingredientId.value;
    var measureUnit =
      this.recipeForm.get('ingredients')['controls'][index]['controls']
        .measureUnit.value;
    var unitQuantity =
      this.recipeForm.get('ingredients')['controls'][index]['controls']
        .unitQuantity.value;

    if (ingredientId != null && measureUnit != null && unitQuantity != null) {
      var ingredient = this.ingredients.find((x) => x.id == ingredientId);
      var measureUnit = MeasureUnit[measureUnit].toString();

      var priceIngredient = 0;

      if (measureUnit == MeasureUnit.kg ||  measureUnit == MeasureUnit.L) {
        priceIngredient = ingredient.unitPrice * (unitQuantity * 1000);
      } 
      else {
        priceIngredient = ingredient.unitPrice * unitQuantity;
      }

      this.recipeForm
        .get('ingredients')
        ['controls'][index]['controls']['costIngredient'].setValue(
          priceIngredient
        );
      this.totalCost = 0;
      for (
        let i = 0;
        i < this.recipeForm.get('ingredients')['controls'].length;
        i++
      ) {
        this.totalCost +=
          this.recipeForm.get('ingredients')['controls'][i][
            'controls'
          ].costIngredient.value;
      }
    }
  }

  changeMeasureUnit(index) {
    var ingredientId = (<FormArray>this.recipeForm.get('ingredients')).controls[
      index
    ]['controls'].ingredientId.value;

    var ingredient = this.ingredients.find((x) => x.id == ingredientId);
    var measureUnitFromIngredient = ingredient.measureUnit;
    
    var measureUnitValueFromIngredient =
    MeasureUnit[measureUnitFromIngredient].toString();

    //set measure unit base measure unit from ingredient
    this.recipeForm
      .get('ingredients')
      ['controls'][index]['controls']['measureUnit'].setValue(
        measureUnitValueFromIngredient
      );

    //1=kg, 2=gr, 3=L, 4=ml, 5=kom
    switch (measureUnitValueFromIngredient) {
      case '1':
      case '2':
        var valueKgAndGr = Object.keys(this.measureUnits).filter(
          (f) => (!isNaN(Number(f)) && f == '1') || f == '2'
        );
        this.ingredientEumKeys[index] = valueKgAndGr;
        break;

      case '3':
      case '4':
        var valueLAndMl = Object.keys(this.measureUnits).filter(
          (f) => (!isNaN(Number(f)) && f == '3') || f == '4'
        );
        this.ingredientEumKeys[index] = valueLAndMl;
        break;

      case '5':
        var valueKom = Object.keys(this.measureUnits).filter(
          (f) => !isNaN(Number(f)) && f == '5'
        );
        this.ingredientEumKeys[index] = valueKom;
        break;

      default:
        break;
    }
  }

  open() {
    const modalRef = this.modalService.open(UpsertIngredientComponent, {
      scrollable: true,
    });

    modalRef.result.then(
      (result) => {
        this.ingredients.push(result);
      },
      (reason) => {}
    );
  }
}
