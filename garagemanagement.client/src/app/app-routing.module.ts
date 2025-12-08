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
import { RepairOrderComponent } from './repair-order/repair-order.component';
import { LabourDetailsComponent } from './labour-details/labour-details.component';  
import { SparePartIssueDetailsComponent } from './spare-part-issue-details/spare-part-issue-details.component';
import { GarageManagementComponent } from './garage-management/garage-management.component';
import { AdditionalJobObserveDetailsComponent } from './additional-job-observe-details/additional-job-observe-details.component';
import { InventoryComponent } from './inventory/inventory.component';
import { SettingsComponent } from './settings/settings.component';
import { CalendarComponent } from './calendar/calendar.component';
import { authGuard } from './auth.guard';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
import { JobCardsComponent } from './job-cards/job-cards.component';
import { NewJobCardComponent } from './new-jobcard/new-jobcard.component';
import { EstimationComponent } from './estimation/estimation.component';

const routes: Routes = [
  {
    path: '',
    component: UserprofileComponent,
    children: [
      {
        path: 'Calendar',
        component: CalendarComponent,canActivate: [authGuard],
        children: [
          { path: 'bookappointment', component: BookingAppointmentComponent }
        ]
      },

      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'forgot', component: ForgetpasswordComponent },
      { path: 'reset-password', component: ResetpasswordComponent },

      { path: 'repair-order', component: RepairOrderComponent, canActivate: [authGuard], data: { roles: ['Admin', 'Manager'] } },
      { path: 'spare-part', component: SparePartIssueDetailsComponent, canActivate: [authGuard] },
      { path: 'garagemanagement', component: GarageManagementComponent, canActivate: [authGuard] },
      { path: 'jobobservedetails', component: AdditionalJobObserveDetailsComponent, canActivate: [authGuard] },
      { path: 'labour-details', component: LabourDetailsComponent, canActivate: [authGuard] },
      { path: 'inventory', component: InventoryComponent, canActivate: [authGuard] },

      { path: 'profile', component: ProfileComponent, canActivate: [authGuard], data: { roles: ['Admin', 'Manager'] } },
      { path: 'settings', component: SettingsComponent, canActivate: [authGuard] },
      { path: 'jobcardlist', component: JobCardsComponent, canActivate: [authGuard] },
      { path: 'newjobcard', component: NewJobCardComponent, canActivate: [authGuard] },
      { path: 'estimate', component: EstimationComponent, canActivate: [authGuard] },
      { path: 'unauthorized', component: UnauthorizedComponent, data: { public: true } }
    ]
  },
];




@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
