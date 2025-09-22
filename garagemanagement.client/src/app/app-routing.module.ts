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
import { LabourDetailsComponent } from './labour-details/labour-details.component';  
import { SparePartIssueDetailsComponent } from './spare-part-issue-details/spare-part-issue-details.component';
import { GarageManagementComponent } from './garage-management/garage-management.component';
import { AdditionalJobObserveDetailsComponent } from './additional-job-observe-details/additional-job-observe-details.component';
import { InventoryComponent } from './inventory/inventory.component';
import { SettingsComponent } from './settings/settings.component';

const routes: Routes = [
  {
    path: '',
    component: UserprofileComponent,
    children: [
      {
        path: 'dashaboard', component: DashboardComponent,
        children: [
          { path: 'bookappointment', component: BookingAppointmentComponent },
        ]
      },
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'forgot', component: ForgetpasswordComponent },
      { path: 'reset-password', component: ResetpasswordComponent },
      { path: 'repair-order', component: RepairOrderComponent },
      { path: 'spare-part', component: SparePartIssueDetailsComponent },
      { path: 'garagemanagement', component: GarageManagementComponent },
      { path: 'jobobservedetails', component: AdditionalJobObserveDetailsComponent },
      { path: 'labour-details', component: LabourDetailsComponent },
      { path: 'inventory', component: InventoryComponent },
      {
        path: 'workshop', component: WorkshopComponent,
        children: [
          { path: 'profile', component: ProfileComponent },
          { path: 'setting', component: SettingsComponent },

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
