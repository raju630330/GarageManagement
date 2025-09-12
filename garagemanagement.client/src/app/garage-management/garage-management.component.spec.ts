import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GarageManagementComponent } from './garage-management.component';

describe('GarageManagementComponent', () => {
  let component: GarageManagementComponent;
  let fixture: ComponentFixture<GarageManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GarageManagementComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GarageManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
