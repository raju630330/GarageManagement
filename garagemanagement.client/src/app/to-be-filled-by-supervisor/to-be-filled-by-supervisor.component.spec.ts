import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ToBeFilledBySupervisorComponent } from './to-be-filled-by-supervisor.component';

describe('GarageManagementComponent', () => {
  let component: ToBeFilledBySupervisorComponent;
  let fixture: ComponentFixture<ToBeFilledBySupervisorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ToBeFilledBySupervisorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ToBeFilledBySupervisorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
