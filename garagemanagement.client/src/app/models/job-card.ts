import { ValidatorFn } from "@angular/forms";

export interface JobCard {
  refNo: string;
  jobCardNo: string;
  regNo: string;
  invoiceNo?: string;
  serviceType: string;
  vehicle: string;
  status: string;
  customerName: string;
  mobileNo: string;
  arrivalDate: string;
  arrivalTime: string;
  insuranceCorporate?: string;
  claimNo?: string;
  estDeliveryDate?: string;
  accidentDate?: string;
}
export interface JobCardDto {
  id: number;
  vehicleData: {
    registrationNo: string;
    odometerIn: string;
    avgKmsPerDay: string;
    vin: string;
    engineNo: string;
    vehicleColor: string;
    fuelType: string;
    serviceType: string;
    serviceAdvisor: string;
    technician: string;
    vendor: string;
  };

  customerInfo: {
    corporate: string;
    customerName: string;
    mobile: string;
    alternateMobile: string;
    email: string;
    deliveryDate: string;
    insuranceCompany: string;
  };

  concerns: {
    text: string;
    active: boolean;
  }[];

  advancePayment: {
    cash: string;
    bankName: string;
    chequeNo: string;
    amount: string;
    date: string;
  };
}

export interface VehicleDetailsUI {
  jobCardNumberForEstimation: string;
  regNo: string;
  jobCardNo: string;

  customerName: string;
  mobile: string;
  email: string;

  odometer: number;
  model: string;
  fuelType: string;
  vin: string;
  engineNo: string;
}

export interface EstimationItem {
  name: string;
  type: string;
  partNo: string;
  rate: number;
  discount: number;
  hsn: string;
  taxPercent: number;
  taxAmount: number;
  total: number;
  approval: string;
  reason: string;
}
export type ColumnType = 'text' | 'number' | 'date' | 'select';
export interface PopupColumnConfig {
  field: string;
  header: string;
  type: ColumnType;
  validators?: ValidatorFn[];
  options?: { label: string; value: any }[];
  getOptions?: (row: any) => { label: string; value: string }[];
  minDate?: string;
  maxDate?: string;
}

export interface PopupTabConfig {
  tabKey: string;
  label: string;
  columns?: PopupColumnConfig[];
  isTextarea?: boolean;
  allowAdd?: boolean;
  allowDelete?: boolean;
}
