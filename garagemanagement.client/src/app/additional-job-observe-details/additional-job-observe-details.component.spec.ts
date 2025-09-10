import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdditionalJobObserveDetailsComponent } from './additional-job-observe-details.component';

describe('AdditionalJobObserveDetailsComponent', () => {
  let component: AdditionalJobObserveDetailsComponent;
  let fixture: ComponentFixture<AdditionalJobObserveDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdditionalJobObserveDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdditionalJobObserveDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
