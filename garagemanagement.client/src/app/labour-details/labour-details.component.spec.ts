import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LabourDetailsComponent } from './labour-details.component';

describe('LabourDetailsComponent', () => {
  let component: LabourDetailsComponent;
  let fixture: ComponentFixture<LabourDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [LabourDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LabourDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
