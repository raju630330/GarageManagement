import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, interval } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import {
  KpiCard, RepairOrder, Bay, Technician,
  Appointment, StockItem, Alert, RevenueStats,
  JobStatus, StockStatus
} from '../models/dashboard';

@Injectable({ providedIn: 'root' })
export class DashboardService {

  private _kpis$ = new BehaviorSubject<KpiCard[]>(this.getKpis());
  private _repairOrders$ = new BehaviorSubject<RepairOrder[]>(this.getRepairOrders());
  private _bays$ = new BehaviorSubject<Bay[]>(this.getBays());
  private _technicians$ = new BehaviorSubject<Technician[]>(this.getTechnicians());
  private _appointments$ = new BehaviorSubject<Appointment[]>(this.getAppointments());
  private _stock$ = new BehaviorSubject<StockItem[]>(this.getStockItems());
  private _alerts$ = new BehaviorSubject<Alert[]>(this.getAlerts());
  private _revenue$ = new BehaviorSubject<RevenueStats>(this.getRevenue());

  get kpis$(): Observable<KpiCard[]> { return this._kpis$.asObservable(); }
  get repairOrders$(): Observable<RepairOrder[]> { return this._repairOrders$.asObservable(); }
  get bays$(): Observable<Bay[]> { return this._bays$.asObservable(); }
  get technicians$(): Observable<Technician[]> { return this._technicians$.asObservable(); }
  get appointments$(): Observable<Appointment[]> { return this._appointments$.asObservable(); }
  get stock$(): Observable<StockItem[]> { return this._stock$.asObservable(); }
  get alerts$(): Observable<Alert[]> { return this._alerts$.asObservable(); }
  get revenue$(): Observable<RevenueStats> { return this._revenue$.asObservable(); }

  get currentTime$(): Observable<string> {
    return interval(1000).pipe(
      startWith(0), // emit immediately
      map(() =>
        new Date().toLocaleTimeString('en-IN', {
          hour: '2-digit',
          minute: '2-digit',
          second: '2-digit'
        })
      )
    );
  }

  dismissAlert(index: number): void {
    const alerts = this._alerts$.value.filter((_, i) => i !== index);
    this._alerts$.next(alerts);
  }

  private getKpis(): KpiCard[] {
    return [
      { label: 'Active Jobs', value: 24, sub: 'from yesterday', trend: 'up', trendValue: '↑ 3', color: 'amber', icon: '🔧' },
      { label: 'Completed Today', value: 11, sub: 'vs avg', trend: 'up', trendValue: '↑ 18%', color: 'green', icon: '✅' },
      { label: 'Appointments', value: '07', sub: '4 today · 3 tomorrow', trend: 'neutral', trendValue: '', color: 'blue', icon: '📅' },
      { label: 'Awaiting Parts', value: '05', sub: 'new requests', trend: 'down', trendValue: '↑ 2', color: 'red', icon: '📦' },
      { label: 'Revenue Today', value: '₹48k', sub: 'vs last Mon', trend: 'up', trendValue: '↑ 12%', color: 'white', icon: '💰' },
    ];
  }

  private getRepairOrders(): RepairOrder[] {
    return [
      { id: 'RO-1001', regNo: 'MH12AB1234', customerName: 'Rajesh Kumar', serviceType: 'General Service', technician: 'Suresh V.', status: 'in-progress', eta: '14:30', bay: 1 },
      { id: 'RO-1002', regNo: 'KA05CD5678', customerName: 'Priya Sharma', serviceType: 'Engine Repair', technician: 'Mohan D.', status: 'awaiting-parts', eta: '17:00', bay: 2 },
      { id: 'RO-1003', regNo: 'TN09EF9012', customerName: 'Arun Patel', serviceType: 'Accident Repair', technician: 'Ravi K.', status: 'in-progress', eta: '16:00', bay: 3 },
      { id: 'RO-1004', regNo: 'DL01GH3456', customerName: 'Meena Singh', serviceType: 'Oil Change', technician: 'Kumar S.', status: 'completed', eta: '12:15', bay: 8 },
      { id: 'RO-1005', regNo: 'MH04IJ7890', customerName: 'Deepak Nair', serviceType: 'A/C Service', technician: 'Suresh V.', status: 'open', eta: '15:45', bay: 5 },
      { id: 'RO-1006', regNo: 'AP16KL2345', customerName: 'Lakshmi T.', serviceType: 'Brake Service', technician: 'Mohan D.', status: 'in-progress', eta: '13:00', bay: 6 },
    ];
  }
  private getBays(): Bay[] {
    return [
      { number: 1, status: 'occupied', regNo: 'MH12AB1234', technician: 'Suresh V.', service: 'Gen. Service' },
      { number: 2, status: 'occupied', regNo: 'KA05CD5678', technician: 'Mohan D.', service: 'Engine' },
      { number: 3, status: 'occupied', regNo: 'TN09EF9012', technician: 'Ravi K.', service: 'Accident' },
      { number: 4, status: 'free' },
      { number: 5, status: 'occupied', regNo: 'MH04IJ7890', technician: 'Kumar S.', service: 'A/C Svc' },
      { number: 6, status: 'occupied', regNo: 'AP16KL2345', technician: 'Mohan D.', service: 'Brakes' },
      { number: 7, status: 'free' },
      { number: 8, status: 'occupied', regNo: 'DL01GH3456', technician: 'Suresh V.', service: 'Oil Chg' },

      { number: 9, status: 'occupied', regNo: 'TS08MN1122', technician: 'Ramesh P.', service: 'Clutch' },
      { number: 10, status: 'free' },
      { number: 11, status: 'occupied', regNo: 'KA09PQ7788', technician: 'Arjun K.', service: 'Wheel Align' },
      { number: 12, status: 'occupied', regNo: 'MH20ZX4567', technician: 'Naresh T.', service: 'Battery' },
      { number: 13, status: 'free' },
      { number: 14, status: 'occupied', regNo: 'AP05BC9988', technician: 'Kiran M.', service: 'Suspension' },
      { number: 15, status: 'occupied', regNo: 'TN22GH3344', technician: 'Ravi K.', service: 'Full Service' },
      { number: 16, status: 'free' }
    ];
  }

  private getTechnicians(): Technician[] {
    return [
      { initials: 'SV', name: 'Suresh V.', jobCount: 3, bays: 'Bay 1, 8', loadPercent: 85 },
      { initials: 'MD', name: 'Mohan D.', jobCount: 3, bays: 'Bay 2, 6', loadPercent: 90 },
      { initials: 'RK', name: 'Ravi K.', jobCount: 2, bays: 'Bay 3', loadPercent: 60 },
      { initials: 'KS', name: 'Kumar S.', jobCount: 1, bays: 'Bay 5', loadPercent: 40 },
    ];
  }

  private getAppointments(): Appointment[] {
    return [
      { time: '09:00', customerName: 'Rajesh Kumar', regNo: 'MH12AB1234', vehicle: 'Swift Dzire', service: 'General Service', status: 'completed' },
      { time: '10:30', customerName: 'Priya Sharma', regNo: 'KA05CD5678', vehicle: 'Honda City', service: 'Engine Check', status: 'completed' },
      { time: '11:00', customerName: 'Meena Singh', regNo: 'MH04IJ7890', vehicle: 'Creta', service: 'A/C Service', status: 'open' },
      { time: '14:00', customerName: 'Arun Patel', regNo: 'TN09EF9012', vehicle: 'Innova', service: 'Accident Repair', status: 'in-progress' },
    ];
  }

  private getStockItems(): StockItem[] {
    return [
      { partName: 'Oil Filter', partNo: 'HF-204', quantity: 2, status: 'critical' },
      { partName: 'Brake Pads (Front)', partNo: 'BP-F112', quantity: 5, status: 'low' },
      { partName: 'Air Filter', partNo: 'AF-3910', quantity: 24, status: 'ok' },
      { partName: 'Engine Oil 5W30', partNo: 'EO-5W30', quantity: 18, status: 'ok' },
      { partName: 'Spark Plug NGK', partNo: 'SP-NGK4', quantity: 6, status: 'low' },
      { partName: 'Cabin Air Filter', partNo: 'CA-F210', quantity: 12, status: 'ok' },
    ];
  }

  private getAlerts(): Alert[] {
    return [
      { level: 'red', icon: '🔴', title: 'Stock Critical: Oil Filter (HF-204)', sub: 'Only 2 units remaining · Part #HF204', time: '09:12' },
      { level: 'yellow', icon: '🟡', title: 'Approval Needed: Estimation #1042', sub: 'KA05CD5678 · ₹8,400 pending approval', time: '08:55' },
      { level: 'blue', icon: '🔵', title: 'New Appointment: Meena Singh', sub: 'MH04IJ7890 · A/C Service · 11:00 AM', time: '08:30' },
      { level: 'yellow', icon: '🟡', title: 'Vehicle Overdue: TN09EF9012', sub: 'Expected delivery was 09:00 · +3.5 hrs', time: '08:10' },
      { level: 'green', icon: '🟢', title: 'Invoice Cleared: Deepak Nair', sub: '₹12,500 · Cash · Job #JC-0934', time: '07:58' },

      { level: 'red', icon: '🔴', title: 'Low Stock: Brake Pads (BP-332)', sub: 'Only 3 units left · Reorder required', time: '07:45' },
      { level: 'blue', icon: '🔵', title: 'Walk-in Customer: Ravi Kumar', sub: 'AP16KL2345 · Engine Check', time: '07:40' },
      { level: 'yellow', icon: '🟡', title: 'Pending Payment: Invoice #INV-221', sub: '₹6,200 · Awaiting customer payment', time: '07:30' },
      { level: 'green', icon: '🟢', title: 'Service Completed: KA01MN4455', sub: 'General Service finished · Ready for delivery', time: '07:20' },
      { level: 'blue', icon: '🔵', title: 'Technician Assigned: Bay B03', sub: 'Ravi K. assigned to Engine Repair', time: '07:10' },

      { level: 'yellow', icon: '🟡', title: 'Customer Waiting: Rahul Sharma', sub: 'MH12AB1234 · Waiting at reception', time: '07:05' },
      { level: 'red', icon: '🔴', title: 'Delayed Part Delivery', sub: 'Clutch Plate shipment delayed by supplier', time: '06:55' },
      { level: 'green', icon: '🟢', title: 'Payment Received: Online', sub: '₹9,800 · UPI · Job #JC-0938', time: '06:45' },
      { level: 'blue', icon: '🔵', title: 'New Booking: Oil Change', sub: 'DL01GH3456 · Slot booked at 02:00 PM', time: '06:30' },
      { level: 'yellow', icon: '🟡', title: 'Service Reminder Sent', sub: 'Customer notified for periodic service', time: '06:20' }
    ];
  }

  private getRevenue(): RevenueStats {
    return {
      thisWeek: '₹2.84L',
      collected: '₹2.21L',
      balance: '₹63k',
      bars: [
        { day: 'MON', height: 52, active: false },
        { day: 'TUE', height: 68, active: false },
        { day: 'WED', height: 85, active: true },
        { day: 'THU', height: 60, active: false },
        { day: 'FRI', height: 90, active: true },
        { day: 'SAT', height: 44, active: false },
        { day: 'SUN', height: 48, active: true },
      ]
    };
  }
}
