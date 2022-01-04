import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login/login.component';
import { IngredientsComponent } from './ingredients/ingredients.component';
import { RecipeCategoriesComponent } from './recipeCategories/recipeCategories.component';
import { RecipeDetailComponent } from './recipes/recipe-details/recipe-detail/recipe-detail.component';
import { RecipesComponent } from './recipes/recipes.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from '../app-routing.module';
import { DataTablesModule } from 'angular-datatables';
import { UpsertRecipeCategoryComponent } from './recipeCategories/upsert-recipeCategory/upsert-recipe-category/upsert-recipe-category.component';
import { NgxBootstrapConfirmModule } from 'ngx-bootstrap-confirm';
import { UpsertRecipeComponent } from './recipes/upsert-recipe/upsert-recipe.component';
import { UpsertIngredientComponent } from './ingredients/upsert-ingredients/upsert-ingredient.component';

@NgModule({
  declarations: [
    LoginComponent,
    RecipesComponent,
    IngredientsComponent,
    RecipeCategoriesComponent,
    UpsertRecipeComponent,
    UpsertIngredientComponent,
    RecipeDetailComponent,
    UpsertRecipeCategoryComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    AppRoutingModule,
    ReactiveFormsModule,
    DataTablesModule,
    NgxBootstrapConfirmModule,
  ]
})
export class FeaturesModule { }
