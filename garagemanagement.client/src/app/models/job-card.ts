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
