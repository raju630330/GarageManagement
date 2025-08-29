import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { LoginComponent } from './login.component';
import { AuthService } from '../../auth.service';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    authServiceSpy = jasmine.createSpyObj('AuthService', ['login']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      declarations: [LoginComponent],
      imports: [ReactiveFormsModule],
      providers: [
        { provide: AuthService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should mark usernameOrEmail as invalid if empty', () => {
    const control = component.loginForm.controls['usernameOrEmail'];
    control.setValue('');
    expect(control.valid).toBeFalse();
  });

  it('should accept a valid username', () => {
    const control = component.loginForm.controls['usernameOrEmail'];
    control.setValue('shraddha123');
    expect(control.valid).toBeTrue();
  });

  it('should accept a valid email', () => {
    const control = component.loginForm.controls['usernameOrEmail'];
    control.setValue('shraddha@gmail.com');
    expect(control.valid).toBeTrue();
  });

  it('should reject invalid username/email', () => {
    const control = component.loginForm.controls['usernameOrEmail'];
    control.setValue('shraddha@');
    expect(control.valid).toBeFalse();
    expect(control.errors?.['invalidUsernameOrEmail']).toBeTrue();
  });

  // ✅ Password Tests
  it('should reject password shorter than 8 characters', () => {
    const control = component.loginForm.controls['password'];
    control.setValue('Aa@1'); 
    expect(control.valid).toBeFalse();
    expect(control.errors?.['invalidPassword']).toBeTrue();
  });

  it('should reject password without uppercase', () => {
    const control = component.loginForm.controls['password'];
    control.setValue('test@1234'); 
    expect(control.valid).toBeFalse();
  });

  it('should reject password without lowercase', () => {
    const control = component.loginForm.controls['password'];
    control.setValue('TEST@1234'); 
    expect(control.valid).toBeFalse();
  });

  it('should reject password without special character', () => {
    const control = component.loginForm.controls['password'];
    control.setValue('Test1234'); // no special char
    expect(control.valid).toBeFalse();
  });

  it('should accept a valid password', () => {
    const control = component.loginForm.controls['password'];
    control.setValue('Test@1234'); // valid
    expect(control.valid).toBeTrue();
  });

  it('should mark form as valid when both fields are correct', () => {
    component.loginForm.setValue({
      usernameOrEmail: 'shraddha123',
      password: 'Test@1234'
    });
    expect(component.loginForm.valid).toBeTrue();
  });

  it('should mark form as invalid when one field is wrong', () => {
    component.loginForm.setValue({
      usernameOrEmail: 'shraddha@',  // invalid
      password: 'Test@1234'
    });
    expect(component.loginForm.valid).toBeFalse();
  });
});
