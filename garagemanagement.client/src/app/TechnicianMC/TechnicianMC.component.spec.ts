import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TechanicianMCComponent } from './TechnicianMC.component';

describe('TechanicianMCComponent', () => {
  let component: TechanicianMCComponent;
  let fixture: ComponentFixture<TechanicianMCComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TechanicianMCComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TechanicianMCComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
