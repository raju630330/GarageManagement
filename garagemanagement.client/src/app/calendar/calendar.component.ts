import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { WorkshopProfileService } from '../services/workshop-profile.service';
import { BookingAppointmentComponent } from '../booking-appointment/booking-appointment.component';



@Component({
  selector: 'app-calendar',
  standalone: false,
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.css']
})
export class CalendarComponent implements OnInit {
  currentWeekDates: Date[] = [];
  hours: string[] = [];
  viewMode: 'week' | 'month' | 'day' = 'week';
  currentDate: Date = new Date();
  appointments: any[] = [];
  filteredAppointments: any[] = [];
  searchTerm: string = '';

  constructor(
    private router: Router,
    private appointmentService: WorkshopProfileService
  ) {
    this.appointmentService.refreshNeeded$.subscribe(() => {
      this.loadAppointments();
    }); }

  ngOnInit(): void {
    this.generateHours();
    this.generateWeek(this.currentDate);
    this.loadAppointments();
  }

  generateHours() {
    this.hours = [];
    for (let h = 7; h <= 19; h++) {
      const label = h < 12 ? `${h} AM` : h === 12 ? '12 PM' : `${h - 12} PM`;
      this.hours.push(label);
    }
  }

  generateWeek(date: Date) {
    this.currentWeekDates = [];
    const startOfWeek = new Date(date);
    startOfWeek.setDate(date.getDate() - startOfWeek.getDay());
    for (let i = 0; i < 7; i++) {
      const day = new Date(startOfWeek);
      day.setDate(startOfWeek.getDate() + i);
      this.currentWeekDates.push(day);
    }
  }

/*  generateMonth(date: Date) {
    this.currentWeekDates = [];
    const start = new Date(date.getFullYear(), date.getMonth(), 1);
    const end = new Date(date.getFullYear(), date.getMonth() + 1, 0);
    for (let d = new Date(start); d <= end; d.setDate(d.getDate() + 1)) {
      this.currentWeekDates.push(new Date(d));
    }
  }*/

  generateMonth(date: Date) {
    this.currentWeekDates = [];

    const year = date.getFullYear();
    const month = date.getMonth();

    // 1st day of month
    const first = new Date(year, month, 1);
    // last day of month
    const last = new Date(year, month + 1, 0);

    // find Monday before the first day
    const start = new Date(first);
    start.setDate(start.getDate() - ((start.getDay() + 6) % 7)); // convert Sun=0 to Sun=6

    // find Sunday after last day
    const end = new Date(last);
    end.setDate(end.getDate() + (7 - ((end.getDay() + 6) % 7) - 1));

    // Fill all days into the array
    for (let d = new Date(start); d <= end; d.setDate(d.getDate() + 1)) {
      this.currentWeekDates.push(new Date(d));
    }
  }

  generateDay(date: Date) {
    this.currentWeekDates = [new Date(date)];
  }

  changeView(event: Event) {
    const mode = (event.target as HTMLSelectElement).value as 'week' | 'month' | 'day';
    this.viewMode = mode;
    if (mode === 'week') this.generateWeek(this.currentDate);
    if (mode === 'month') this.generateMonth(this.currentDate);
    if (mode === 'day') this.generateDay(this.currentDate);
  }
/*
  previousWeek() {
    this.currentDate.setDate(this.currentDate.getDate() - 7);
    this.generateWeek(this.currentDate);
  }

  nextWeek() {
    this.currentDate.setDate(this.currentDate.getDate() + 7);
    this.generateWeek(this.currentDate);
  }

  today() {
    this.currentDate = new Date();
    this.generateWeek(this.currentDate);
  }*/

  previousWeek() {
    if (this.viewMode === 'day') {
      this.currentDate.setDate(this.currentDate.getDate() - 1);
      this.generateDay(this.currentDate);
    }
    else if (this.viewMode === 'week') {
      this.currentDate.setDate(this.currentDate.getDate() - 7);
      this.generateWeek(this.currentDate);
    }
    else if (this.viewMode === 'month') {
      this.currentDate = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth() - 1, 1);
      this.generateMonth(this.currentDate);
    }
  }

  nextWeek() {
    if (this.viewMode === 'day') {
      this.currentDate.setDate(this.currentDate.getDate() + 1);
      this.generateDay(this.currentDate);
    }
    else if (this.viewMode === 'week') {
      this.currentDate.setDate(this.currentDate.getDate() + 7);
      this.generateWeek(this.currentDate);
    }
    else if (this.viewMode === 'month') {
      this.currentDate = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth() + 1, 1);
      this.generateMonth(this.currentDate);
    }
  }

  today() {
    this.currentDate = new Date();
    if (this.viewMode === 'day') this.generateDay(this.currentDate);
    else if (this.viewMode === 'week') this.generateWeek(this.currentDate);
    else if (this.viewMode === 'month') this.generateMonth(this.currentDate);
  }


  isToday(date: Date): boolean {
    const today = new Date();
    return (
      date.getDate() === today.getDate() &&
      date.getMonth() === today.getMonth() &&
      date.getFullYear() === today.getFullYear()
    );
  }

  loadAppointments() {
    this.appointmentService.getAllAppointments().subscribe({
      next: (data) => {
        this.appointments = data.map(a => ({
          ...a,
          start: new Date(a.date.substring(0, 10) + 'T' + a.time)
        }));
        this.filteredAppointments = [...this.appointments];
      },
      error: (err) => console.error('Failed to load appointments:', err)
    });
  }

  filterAppointments() {
    if (!this.searchTerm.trim()) {
      this.filteredAppointments = [...this.appointments];
      return;
    }

    const term = this.searchTerm.toLowerCase();
    this.filteredAppointments = this.appointments.filter(a =>
      a.service.toLowerCase().includes(term) ||
      a.regNo.toLowerCase().includes(term) ||
      a.customerType.toLowerCase().includes(term)
    );
  }

  getAppointmentsForSlot(day: Date, hour: string) {
    const hour24 = this.convertTo24Hour(hour);
    return this.filteredAppointments.filter(appt => {
      const apptDate = new Date(appt.start);
      return (
        apptDate.getDate() === day.getDate() &&
        apptDate.getMonth() === day.getMonth() &&
        apptDate.getFullYear() === day.getFullYear() &&
        apptDate.getHours() === hour24
      );
    });
  }

  convertTo24Hour(hourLabel: string): number {
    const [time, modifier] = hourLabel.split(' ');
    let h = parseInt(time);
    if (modifier === 'PM' && h !== 12) h += 12;
    if (modifier === 'AM' && h === 12) h = 0;
    return h;
  }
  //For mothly bookings update
  hasAppointments(date: Date): any[] {
    return this.filteredAppointments.filter(appt => {
      const d = new Date(appt.start);
      return d.getDate() === date.getDate() &&
        d.getMonth() === date.getMonth() &&
        d.getFullYear() === date.getFullYear();
    });
  }
}

