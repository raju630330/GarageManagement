import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignUserToWorkshopComponent } from './assign-user-to-workshop.component';

describe('AssignUserToWorkshopComponent', () => {
  let component: AssignUserToWorkshopComponent;
  let fixture: ComponentFixture<AssignUserToWorkshopComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AssignUserToWorkshopComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AssignUserToWorkshopComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
