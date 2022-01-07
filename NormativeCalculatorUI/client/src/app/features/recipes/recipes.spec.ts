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
import { async, of } from 'rxjs';
import { DebugElement, ElementRef, NO_ERRORS_SCHEMA } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ItemsList } from '@ng-select/ng-select/lib/items-list';

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
     imports: [HttpClientTestingModule, RouterTestingModule, NgxBootstrapConfirmModule, ToastrModule.forRoot(), FormsModule]
   })
     .compileComponents();
 });

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

  // it('should have recipes data', () => {
  //   expect(component.recipes).toBeTruthy();
  //   expect(component.recipes.length).toBeGreaterThan(0);
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
  fixture.detectChanges();
  expect(submitEl.nativeElement.querySelector('button').disabled).toBeTruthy();
}); 

it('should return load more data', () => {
  expect(component.loadMore).toBeTruthy();
}); 


beforeEach(
  () => {
    recipeService = jasmine.createSpyObj(['get', 'getById']);
   (recipeService.get as Spy).and.returnValue(of(recipes));
   (recipeService.getById as Spy).and.returnValue(of(recipes.filter((item)=> item.id == 1)));
});

it('should call get method from service', () => {
   recipeService.get(null).subscribe((data)=>{
    expect(data).toEqual(recipes);
   });
});

it('should call getById method from service', () => {
   recipeService.getById('1').subscribe((data)=>{
   expect(data).toEqual(recipes.filter((item)=> item.id == 1));
   });
});


// it('should test the table ', fakeAsync((done) => {
//   expect(component.recipes).toEqual(recipes);
//   tick(500);
//   fixture.detectChanges();
//   fixture.whenStable().then(() => {fixture.detectChanges();

//     let tableRows = fixture.nativeElement.querySelectorAll('tr');
//     expect(tableRows.length).toBe(1);

//     // Header row
//     let headerRow = tableRows[0];
//     expect(headerRow.cells[0].innerHTML).toBe('Name');
//     expect(headerRow.cells[1].innerHTML).toBe('Cost');
//     expect(headerRow.cells[2].innerHTML).toBe('Description');

//     // Data rows
//      let row1 = tableRows[1];

//     console.log('row ');

//     expect(row1.cells[0].innerHTML).toBe('Recipe 1');
//     expect(row1.cells[1].innerHTML).toBe('10');
//     expect(row1.cells[2].innerHTML).toBe('Description for recipe 1');

//     // Test more rows here..

//     done();
//   });
// }));

describe('Table', () => {
  let tableTh: DebugElement[];
  let tableTd: DebugElement[];
  beforeEach(() => {
    tableTh = fixture.debugElement.queryAll(By.css('table th'));
    tableTd = fixture.debugElement.queryAll(By.css('table tr'));
  });
  it('should have 5 th in table', () => {
    expect(tableTh.length).toEqual(5);
  });
  it('should have 5 td in table', () => {
    console.log('length component' + component.recipes.length)
    expect(tableTd.length).toEqual(1);
  }); 

  it('should render Recipe category 1', () => {
    component.nameofCategory = 'Recipe category 1';
    fixture.detectChanges();
    const debugElement = fixture.debugElement.query(By.css('h1'));
    const element: HTMLElement = debugElement.nativeElement;
    expect(element.innerText).toContain('Recipe category 1');
    });

    
  it('should have 2 recipe records', () => {

    fixture.detectChanges();
    const twoElement = fixture.debugElement.queryAll(By.css('tr'));
    const availableUserRecords = twoElement.slice(1);
    expect(availableUserRecords.length).toBe(2);
    });
 
 
 it('should call the details of recipe  On Details click', () => {

  fixture.detectChanges();
  const element = fixture.debugElement.query(By.css('.details')).triggerEventHandler('click', {});
  fixture.whenStable();
   expect(component.getRecipeDetails).toHaveBeenCalled();

  });

});



  const UnmockedDate = Date;
  const recipes: Recipe[] = 
    [{
      id: 1,
      name: 'Recipe 1',
      description: 'Description for recipe 1',
      recipeCategoryId: 1,
      createdAt: new UnmockedDate('2021-01-01'),
      userId: 1,
      isDeleted: false,
      recommendedPrice: 10
    },
    {
      id: 2,
      name: 'Recipe 2',
      description: 'Description for recipe 2',
      recipeCategoryId: 1,
      createdAt: new UnmockedDate('2021-01-01'),
      userId: 1,
      isDeleted: false,
      recommendedPrice: 20
    },
  ];

});
