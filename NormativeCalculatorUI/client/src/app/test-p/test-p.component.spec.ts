import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestPComponent } from './test-p.component';

describe('TestPComponent', () => {
  let component: TestPComponent;
  let fixture: ComponentFixture<TestPComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TestPComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TestPComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
