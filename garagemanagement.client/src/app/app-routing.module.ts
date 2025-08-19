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

const routes: Routes = [
  {
    path: '',
    component: UserprofileComponent,
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'forgot', component: ForgetpasswordComponent },
      { path: 'reset', component: ResetpasswordComponent },
      {
        path: 'workshop', component: WorkshopComponent,
        children: [
          { path: 'profile', component: ProfileComponent },
          { path: 'bookappointment', component: BookingAppointmentComponent }
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
