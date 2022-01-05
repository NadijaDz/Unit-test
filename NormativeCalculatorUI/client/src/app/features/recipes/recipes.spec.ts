import { ComponentFixture, fakeAsync, TestBed, tick, waitForAsync } from '@angular/core/testing';
import { RecipesService } from 'src/app/core/services/recipes.service';
import { RecipesComponent } from './recipes.component';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { NgxBootstrapConfirmModule } from 'ngx-bootstrap-confirm';
import { ToastrModule } from 'ngx-toastr';
import { By } from '@angular/platform-browser';
import { Recipe } from 'src/app/core/models/recipe.model';
import { FormsModule } from '@angular/forms';
import Spy = jasmine.Spy;
import { of } from 'rxjs';
import { DebugElement, ElementRef, NO_ERRORS_SCHEMA } from '@angular/core';
import { HttpClient } from '@angular/common/http';

describe('RecipesComponent', () => {
  let component: RecipesComponent;
  let fixture: ComponentFixture<RecipesComponent>;
  let recipeService: RecipesService;
  let submitEl: DebugElement;

  beforeEach( () => {
    TestBed.configureTestingModule({
     declarations: [RecipesComponent],
     providers: [
       // {provide: RecipesService, useValue: recipeService}
     ],
     imports: [HttpClientTestingModule, RouterTestingModule, NgxBootstrapConfirmModule, ToastrModule.forRoot()]
   })
     .compileComponents();
 });

  beforeEach(
    waitForAsync(() => {
      recipeService = jasmine.createSpyObj(['get']);
     (recipeService.get as Spy).and.returnValue(of(recipes()));
  }));
 

  beforeEach(() => {
    fixture = TestBed.createComponent(RecipesComponent);
    component = fixture.debugElement.componentInstance;
    recipeService = TestBed.inject(RecipesService);
    fixture.detectChanges();
  });

  describe('Hello', () => {
    it('says hello', () => {
      expect(component.helloWorld()).toEqual('Hello world!');
    });
  });

  describe('Component create', () => {
    it('should creae an instance', () => {
      expect(component).toBeTruthy();
    });
  });

  describe('Category name as title', () => {
    it('Should have a category name for recipe', () => {
      const categoryName = fixture.debugElement.query(By.css('h1')).nativeElement;
      expect(categoryName.innerHTML).toBe('Recipes for category ');
    });
  });


  // it('should fetch data from services', () => {
  //   //debugger;
  //   // spyOn(component, 'getRecipes');
  //   // spyOn(recipeService, 'get');
  //   //component.ngOnInit();
  //   // component.getRecipes();
  //   // expect(component.getRecipes).toHaveBeenCalled();
  
  //   expect(recipeService.get).toHaveBeenCalled();
  // });


it('should load the data when click on Load more button', fakeAsync(() => {
  let buttonElement = fixture.debugElement.query(By.css('.lodaMore-btn')); 

  spyOn(component, 'loadMore');
  buttonElement.triggerEventHandler('click', null);
  tick();
  expect(component.loadMore).toHaveBeenCalled();

})); 


beforeEach(() => {
  submitEl = fixture.debugElement;
});

it('Disable load more set to false the load more buttons', () => {

  component.isLodaMore = false;
  console.log(component.isLodaMore);
 // component.recipes = null;
  fixture.detectChanges();
  expect(submitEl.nativeElement.querySelector('button').disabled).toBeTruthy();
}); 

it('should return load more data', () => {
  expect(component.loadMore).toBeTruthy();
}); 

  const UnmockedDate = Date;
  const recipes = (): Recipe[] => [
    {
      id: 1,
      name: 'Recipe 1',
      description: 'Description for recipe 1',
      recipeCategoryId: 1,
      createdAt: new UnmockedDate('2021-01-01'),
      userId: 1,
      isDeleted: false,
      recommendedPrice: 10
    }];

});
