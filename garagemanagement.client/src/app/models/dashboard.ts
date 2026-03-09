export interface KpiCard {
  label: string;
  value: string | number;
  sub: string;
  trend: 'up' | 'down' | 'neutral';
  trendValue: string;
  color: 'amber' | 'green' | 'red' | 'blue' | 'white';
  icon: string;
}

export type JobStatus = 'open' | 'in-progress' | 'awaiting-parts' | 'completed';

export interface RepairOrder {
  id: string;
  regNo: string;
  customerName: string;
  serviceType: string;
  technician: string;
  status: JobStatus;
  eta: string;
  bay?: number;
}

export type BayStatus = 'occupied' | 'free';

export interface Bay {
  number: number;
  status: BayStatus;
  regNo?: string;
  technician?: string;
  service?: string;
}

export interface Technician {
  initials: string;
  name: string;
  jobCount: number;
  bays: string;
  loadPercent: number;
}

export interface Appointment {
  time: string;
  customerName: string;
  regNo: string;
  vehicle: string;
  service: string;
  status: JobStatus;
}

export type StockStatus = 'ok' | 'low' | 'critical';

export interface StockItem {
  partName: string;
  partNo: string;
  quantity: number;
  status: StockStatus;
}

export interface Alert {
  level: 'red' | 'yellow' | 'blue' | 'green';
  title: string;
  sub: string;
  time: string;
  icon: string;
}

export interface WeeklyRevenueBar {
  day: string;
  height: number;
  active: boolean;
}

export interface RevenueStats {
  thisWeek: string;
  collected: string;
  balance: string;
  bars: WeeklyRevenueBar[];
}
