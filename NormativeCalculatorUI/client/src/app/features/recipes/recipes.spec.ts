import { ComponentFixture, fakeAsync, TestBed, tick, waitForAsync } from '@angular/core/testing';
import { RecipesService } from 'src/app/core/services/recipes.service';
import { RecipesComponent } from './recipes.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { NgxBootstrapConfirmModule } from 'ngx-bootstrap-confirm';
import { ToastrModule } from 'ngx-toastr';
import { By } from '@angular/platform-browser';
import { MeasureUnit } from 'src/app/core/models/measure-unit.model';
import { of } from 'rxjs';
import { Recipe } from 'src/app/core/models/recipe.model';
import Spy = jasmine.Spy;
import { RecipeDetailComponent } from './recipe-details/recipe-detail/recipe-detail.component';

describe('RecipesComponent', () => {
  let component: RecipesComponent;
  let fixture: ComponentFixture<RecipesComponent>;
  let recipeService: RecipesService;
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

  
  // describe('Recipes details', () => {
  //   it('Details about recipes', fakeAsync(() => {
  //     let response = {
  //       "name": 'Recipe 1',
  //       "description": 'Description for Recipe 1',
  //       "recipeCategoryId": 1,
  //       "createdAt": Date.now,
  //       "ingredients": [
  //         {
  //           "ingredientId": 1,
  //           "measureUnit": MeasureUnit.kg,
  //           "unitQuantity": 5,
  //         }]
  //       };

     

  //   }));
  // });

//   describe('Category', () => {
//   it('should fetch data from services', () => {
//     // component.ngOnInit();
//     expect(recipeService.get).toHaveBeenCalled();
//   });
// });

it('should open the recipe details  when clicking on Detail button', fakeAsync(() => {
  let buttonElement = fixture.debugElement.query(By.css('.details'));
  
  spyOn(component, 'getRecipeDetails');
  //Trigger click event after spyOn
  buttonElement.triggerEventHandler('click', null);
  tick();
  expect(component.getRecipeDetails).toHaveBeenCalled();
})); 

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
