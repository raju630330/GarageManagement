import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociatedworkshopsComponent } from './associatedworkshops.component';

describe('AssociatedworkshopsComponent', () => {
  let component: AssociatedworkshopsComponent;
  let fixture: ComponentFixture<AssociatedworkshopsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AssociatedworkshopsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AssociatedworkshopsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
