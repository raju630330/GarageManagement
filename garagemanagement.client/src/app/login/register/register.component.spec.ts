import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { RegisterComponent } from './register.component';
import { AuthService } from '../../auth.service';

class MockAuthService {
  register(data: any) {
    return of({}); // default success
  }
}

describe('RegisterComponent', () => {
  let component: RegisterComponent;
  let fixture: ComponentFixture<RegisterComponent>;
  let authService: AuthService;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RegisterComponent],
      imports: [ReactiveFormsModule],
      providers: [
        { provide: AuthService, useClass: MockAuthService },
        { provide: Router, useValue: { navigate: jasmine.createSpy('navigate') } }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(RegisterComponent);
    component = fixture.componentInstance;
    authService = TestBed.inject(AuthService);
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  it('should create the register component', () => {
    expect(component).toBeTruthy();
  });

  //it('should call authService.register with correct data', () => {
  //  spyOn(authService, 'register').and.returnValue(of({}));
  //  component.registerForm.setValue({ username: 'test', email: 'a@a.com', password: '12345' });
  //  component.onSubmit();
  //  expect(authService.register).toHaveBeenCalledWith(component.registerForm.value);
  //});

  //it('should navigate to login on successful registration', () => {
  //  spyOn(authService, 'register').and.returnValue(of({}));
  //  component.registerForm.setValue({ username: 'test', email: 'a@a.com', password: '12345' });
  //  component.onSubmit();
  //  expect(router.navigate).toHaveBeenCalledWith(['/login']);
  //});

  //it('should show error alert when registration fails', () => {
  //  spyOn(window, 'alert');
  //  spyOn(authService, 'register').and.returnValue(throwError(() => new Error('fail')));
  //  component.registerForm.setValue({ username: 'test', email: 'a@a.com', password: '12345' });
  //  component.onSubmit();
  //  expect(window.alert).toHaveBeenCalledWith('Registration failed');
  //});
});
