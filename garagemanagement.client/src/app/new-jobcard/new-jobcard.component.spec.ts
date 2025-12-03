import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewJobcardComponent } from './new-jobcard.component';

describe('NewJobcardComponent', () => {
  let component: NewJobcardComponent;
  let fixture: ComponentFixture<NewJobcardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [NewJobcardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NewJobcardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
