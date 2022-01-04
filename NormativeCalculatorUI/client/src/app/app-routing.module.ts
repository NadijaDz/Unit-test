import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/authGuard/auth.guard';
import { IngredientsComponent } from './features/ingredients/ingredients.component';
import { LoginComponent } from './features/login/login/login.component';
import { RecipeCategoriesComponent } from './features/recipeCategories/recipeCategories.component';
import { UpsertRecipeCategoryComponent } from './features/recipeCategories/upsert-recipeCategory/upsert-recipe-category/upsert-recipe-category.component';
import { RecipesComponent } from './features/recipes/recipes.component';
import { UpsertRecipeComponent } from './features/recipes/upsert-recipe/upsert-recipe.component';

const routes: Routes = [

  {path:'login', component: LoginComponent},
  {path:'recipeCategories', component: RecipeCategoriesComponent},
  {path:'recipes/:id/:name', component: RecipesComponent},
  {
    path: '',
    runGuardsAndResolvers:'always',
    canActivate: [AuthGuard],
    children:[
    {path:'ingredients', component: IngredientsComponent},
    {path:'recipes/:id/:name/upsert-recipe', component: UpsertRecipeComponent},
    {path:'recipeCategories/upsert-recipeCategory', component: UpsertRecipeCategoryComponent},
    {path:'recipeCategories/upsert-recipeCategory/:id', component: UpsertRecipeCategoryComponent},
    {path:'recipes/:id/:name/upsert-recipe/:recipeId', component: UpsertRecipeComponent},
    ]},
   {path:'**', component: LoginComponent, pathMatch:'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
