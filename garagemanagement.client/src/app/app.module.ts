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
import { CalendarComponent } from './calendar/calendar.component';
import { RepairOrderComponent } from './repair-order/repair-order.component';
import { SparePartIssueDetailsComponent } from './spare-part-issue-details/spare-part-issue-details.component';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';
import { ToBeFilledBySupervisorComponent } from './to-be-filled-by-supervisor/to-be-filled-by-supervisor.component';
import { AdditionalJobObserveDetailsComponent } from './additional-job-observe-details/additional-job-observe-details.component';
import { LabourDetailsComponent } from './labour-details/labour-details.component';
import { NavbarComponent } from './navbar/navbar.component';
import { InventoryComponent } from './inventory/inventory.component';
import { SettingsComponent } from './settings/settings.component';
import { ComponentNameComponent } from './component-name/component-name.component';
import { TotalRepairCostComponent } from './total-repair-cost/total-repair-cost.component';
import { TechnicianMCComponent } from './TechnicianMC/TechnicianMC.component';
import { HasRoleDirective } from './directives/has-role.directive';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
import { GlobalAlertComponent } from './global-alert/global-alert.component';
import { LoaderComponent } from './loader/loader.component';
import { PopupTimePickerComponent } from './popup-time-picker/popup-time-picker.component';
import { JobCardsComponent } from './job-cards/job-cards.component';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatMenuModule } from '@angular/material/menu';
import { NewJobCardComponent } from './new-jobcard/new-jobcard.component';
import { EstimationComponent } from './estimation/estimation.component';
import { GlobalPopupComponent } from './global-popup/global-popup.component';
import { MatDialogModule } from '@angular/material/dialog';
import { AutocompleteComponent } from './autocomplete/autocomplete.component';
import { OverlayModule } from '@angular/cdk/overlay';
import { NumbersOnlyDirective } from './directives/appNumbersOnly.directive';
import { DecimalNumbersDirective } from './directives/appDecimalNumbers.directive';
import { TwoDecimalNumbersDirective } from './directives/appTwoDecimalNumbers.directive';

@NgModule({
  declarations: [
    AppComponent,
    UserprofileComponent,
    WorkshopComponent,
    ProfileComponent,
    CalendarComponent,
    LoginComponent,
    RegisterComponent,
    BookingAppointmentComponent,
    ForgetpasswordComponent,
    ResetpasswordComponent,
    RepairOrderComponent,             
    ToBeFilledBySupervisorComponent,
    AdditionalJobObserveDetailsComponent,
    SparePartIssueDetailsComponent,
    NavbarComponent,
    LabourDetailsComponent,
    InventoryComponent,
    SettingsComponent,
    ComponentNameComponent,
    TotalRepairCostComponent,
    TechnicianMCComponent,
    HasRoleDirective,
    UnauthorizedComponent,
    GlobalAlertComponent,
    LoaderComponent,
    PopupTimePickerComponent,
    JobCardsComponent,
    NewJobCardComponent,
    EstimationComponent,
    GlobalPopupComponent,
    AutocompleteComponent,
    NumbersOnlyDirective,
    DecimalNumbersDirective,
    TwoDecimalNumbersDirective
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    ReactiveFormsModule,
    MatExpansionModule,
    MatIconModule,
    MatExpansionModule,
    FormsModule,
    MatInputModule,
    MatDatepickerModule,
    MatTableModule,
    MatButtonModule,
    BrowserAnimationsModule,
    MatNativeDateModule,
    MatMenuModule,
    MatDialogModule,
    OverlayModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: authInterceptor, multi: true }
    
    
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
