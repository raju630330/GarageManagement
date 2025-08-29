import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ProfileComponent } from './profile.component';

describe('ProfileComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProfileComponent],
      imports: [HttpClientTestingModule] 
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(ProfileComponent);
    const component = fixture.componentInstance;
    expect(component).toBeTruthy();
  });
});
