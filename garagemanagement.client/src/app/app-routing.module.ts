import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserprofileComponent } from './userprofile/userprofile.component';
import { WorkshopComponent } from './workshop/workshop.component';
import { ProfileComponent } from './profile/profile.component';
import { LoginComponent } from './login/login/login.component';
import { BookingAppointmentComponent } from './booking-appointment/booking-appointment.component';
import { RegisterComponent } from './login/register/register.component';
import { ForgetpasswordComponent } from './forgetpassword/forgetpassword.component';
import { ResetpasswordComponent } from './resetpassword/resetpassword.component';
import { adminGuard } from './admin.guard';
import { authGuard } from './auth.guard';
import { DashboardComponent } from './dashboard/dashboard.component';
import { RepairOrderComponent } from './repair-order/repair-order.component';
import { GarageManagementComponent } from './garage-management/garage-management.component';
import { AdditionalJobObserveDetailsComponent } from './additional-job-observe-details/additional-job-observe-details.component';

const routes: Routes = [
  {
    path: '',
    component: UserprofileComponent,
    children: [
      {
        path: 'dashaboard', component: DashboardComponent,
        children: [
          { path: 'bookappointment', component: BookingAppointmentComponent },
        ]},
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'forgot', component: ForgetpasswordComponent },
      { path: 'reset-password', component: ResetpasswordComponent },
      { path: 'repair-order', component: RepairOrderComponent },
      { path: 'garage-management', component: GarageManagementComponent },
      { path: 'addtional-job-observe-details', component: AdditionalJobObserveDetailsComponent },
      {
        path: 'workshop', component: WorkshopComponent,
        children: [
          { path: 'profile', component: ProfileComponent },
        ]
      }
    ]

  },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
