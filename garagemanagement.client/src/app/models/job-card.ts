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
