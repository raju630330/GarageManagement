import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PopupTimePickerComponent } from './popup-time-picker.component';

describe('PopupTimePickerComponent', () => {
  let component: PopupTimePickerComponent;
  let fixture: ComponentFixture<PopupTimePickerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PopupTimePickerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PopupTimePickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
