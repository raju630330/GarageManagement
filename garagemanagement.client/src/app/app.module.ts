import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ProfileComponent } from './profile/profile.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './login/login/login.component';
import { RegisterComponent } from './login/register/register.component';
import { UserprofileComponent } from './userprofile/userprofile.component';
import { WorkshopComponent } from './workshop/workshop.component';
import { BookingAppointmentComponent } from './booking-appointment/booking-appointment.component';
import { ResetpasswordComponent } from './resetpassword/resetpassword.component';
import { ForgetpasswordComponent } from './forgetpassword/forgetpassword.component';
import { authInterceptor } from './auth.interceptor';
import { DashboardComponent } from './dashboard/dashboard.component';
import { RepairOrderComponent } from './repair-order/repair-order.component';
import { SparePartIssueDetailsComponent } from './spare-part-issue-details/spare-part-issue-details.component';
import { MatExpansionModule } from '@angular/material/expansion';
import { GarageManagementComponent } from './garage-management/garage-management.component';
import { AdditionalJobObserveDetailsComponent } from './additional-job-observe-details/additional-job-observe-details.component';
import { LabourDetailsComponent } from './labour-details/labour-details.component';
import { NavbarComponent } from './navbar/navbar.component';

@NgModule({
  declarations: [
    AppComponent,
    UserprofileComponent,
    WorkshopComponent,
    ProfileComponent,
    DashboardComponent,
    LoginComponent,
    RegisterComponent,
    BookingAppointmentComponent,
    ForgetpasswordComponent,
    ResetpasswordComponent,
    RepairOrderComponent,
    GarageManagementComponent,
    AdditionalJobObserveDetailsComponent,
    SparePartIssueDetailsComponent,
    NavbarComponent,
    LabourDetailsComponent
    
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    ReactiveFormsModule,
    MatExpansionModule,
    FormsModule,
    
    FormsModule,

  ],
  providers: [{ provide: HTTP_INTERCEPTORS, useClass: authInterceptor, multi: true }],
  bootstrap: [AppComponent]
})
export class AppModule { }
