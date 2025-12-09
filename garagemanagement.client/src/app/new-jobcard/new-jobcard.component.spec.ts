import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewJobCardComponent } from './new-jobcard.component';

describe('NewJobcardComponent', () => {
  let component: NewJobCardComponent;
  let fixture: ComponentFixture<NewJobCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [NewJobCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NewJobCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
