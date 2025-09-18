import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SparePartIssueDetailsComponent } from './spare-part-issue-details.component';

describe('SparePartIssueDetailsComponent', () => {
  let component: SparePartIssueDetailsComponent;
  let fixture: ComponentFixture<SparePartIssueDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SparePartIssueDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SparePartIssueDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
