import {
  Component, OnInit, OnDestroy,
  ChangeDetectionStrategy, ChangeDetectorRef
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import {
  KpiCard, RepairOrder, Bay, Technician,
  Appointment, StockItem, Alert, RevenueStats,
  JobStatus, StockStatus
} from '../models/dashboard';
import { DashboardService } from '../services/dashboardservice';
import { Router } from '@angular/router';
// Uncomment when wiring real auth:
// import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  // ── Identity ──────────────────────────────────────────────────────────
  userName = 'Service Advisor';

  // ── Clock ─────────────────────────────────────────────────────────────
  currentTime = '';
  currentDate = new Date().toLocaleDateString('en-IN', {
    weekday: 'short', day: '2-digit', month: 'short', year: 'numeric'
  }).toUpperCase();

  // ── Data ──────────────────────────────────────────────────────────────
  kpis: KpiCard[] = [];
  repairOrders: RepairOrder[] = [];
  bays: Bay[] = [];
  technicians: Technician[] = [];
  appointments: Appointment[] = [];
  stockItems: StockItem[] = [];
  alerts: Alert[] = [];
  revenue!: RevenueStats;

  // ── KPI convenience props (bind directly to template) ─────────────────
  activeJobs = 0;
  activeJobsTrend = '';
  completedToday = 0;
  appointmentCount = 0;
  awaitingParts = 0;
  revenueToday = '';

  // ── Quick actions ──────────────────────────────────────────────────────
  quickActions = [
    { label: 'Appointment', icon: 'bi bi-calendar-plus', route: '/Calendar/bookappointment' },
    { label: 'Repair Order', icon: 'bi bi-tools', route: '/repair-order' },
    { label: 'Stock', icon: 'bi bi-box-seam', route: '/stock' },
  ];

  // ── Computed ───────────────────────────────────────────────────────────
  get occupiedBays() { return this.bays.filter(b => b.status === 'occupied').length; }
  get totalBays() { return this.bays.length; }
  get bayUtilPct() { return this.totalBays ? Math.round(this.occupiedBays / this.totalBays * 100) : 0; }

  constructor(
    private svc: DashboardService,
    private cdr: ChangeDetectorRef,
    private router: Router,
    // private auth: AuthService
  ) { }

  ngOnInit(): void {
    const mark = () => this.cdr.markForCheck();

    this.svc.kpis$.pipe(takeUntil(this.destroy$)).subscribe(v => { this.kpis = v; mark() });
    this.svc.repairOrders$.pipe(takeUntil(this.destroy$)).subscribe(v => { this.repairOrders = v; mark() });
    this.svc.bays$.pipe(takeUntil(this.destroy$)).subscribe(v => { this.bays = v; mark() });
    this.svc.technicians$.pipe(takeUntil(this.destroy$)).subscribe(v => { this.technicians = v; mark() });
    this.svc.appointments$.pipe(takeUntil(this.destroy$)).subscribe(v => { this.appointments = v; mark() });
    this.svc.stock$.pipe(takeUntil(this.destroy$)).subscribe(v => { this.stockItems = v; mark() });
    this.svc.alerts$.pipe(takeUntil(this.destroy$)).subscribe(v => { this.alerts = v; mark() });
    this.svc.revenue$.pipe(takeUntil(this.destroy$)).subscribe(v => { this.revenue = v; mark() });
    this.svc.currentTime$.pipe(takeUntil(this.destroy$)).subscribe(v => { this.currentTime = v; mark() });
  }

  ngOnDestroy(): void { this.destroy$.next(); this.destroy$.complete(); }

  // ── Alert actions ──────────────────────────────────────────────────────
  dismissAlert(i: number) { this.svc.dismissAlert(i); }
  clearAlerts() { this.alerts = []; this.cdr.markForCheck(); }

  // ── trackBy ────────────────────────────────────────────────────────────
  trackById(_: number, item: any) { return item.id ?? item; }

  // ── Label helpers ──────────────────────────────────────────────────────
  statusLabel(s: JobStatus): string {
    const map: Record<JobStatus, string> = {
      'open': 'Open',
      'in-progress': 'In Progress',
      'awaiting-parts': 'Awaiting Parts',
      'completed': 'Completed',
    };
    return map[s] ?? s;
  }

  apptLabel(s: JobStatus): string {
    const map: Record<JobStatus, string> = {
      'open': 'Expected',
      'in-progress': 'In Bay',
      'awaiting-parts': 'On Hold',
      'completed': 'Arrived',
    };
    return map[s] ?? s;
  }

  stockLabel(s: StockStatus): string {
    return ({ ok: 'In Stock', low: 'Low Stock', critical: 'Critical' })[s] ?? s;
  }

  techLevel(pct: number): 'low' | 'mid' | 'high' {
    if (pct >= 80) return 'high';
    if (pct >= 50) return 'mid';
    return 'low';
  }
  createNewJobCard() {
    this.router.navigate(['/newjobcard']);
  }
}
