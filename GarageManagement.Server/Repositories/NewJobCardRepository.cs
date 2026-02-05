using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;

namespace GarageManagement.Server.Repositories
{
    public class NewJobCardRepository:INewJobCardRepsoitory
    {
        private readonly ApplicationDbContext _context;
        public NewJobCardRepository(ApplicationDbContext context) { _context = context; }

        public async Task<List<JobCardListDto>> GetJobCards()
        {
            var jobCards = await _context.JobCards
                .Select(j => new JobCardListDto
                {
                    RefNo = "REF" + j.Id.ToString().PadLeft(3, '0'),
                    JobCardNo = j.JobCardNo,
                    InvoiceNo = "SRT-I" + (2214 + j.Id),
                    ClaimNo = "CLM" + j.Id.ToString().PadLeft(3, '0'),
                    RegNo = j.RegistrationNo ?? "",
                    Vehicle = j.VehicleColor ?? "",
                    ServiceType = j.ServiceType ?? "",
                    Status = j.Status,
                    CustomerName = j.CustomerName ?? "",
                    MobileNo = string.IsNullOrEmpty(j.Mobile) || j.Mobile.Length < 4
                        ? "******"
                        : "******" + j.Mobile.Substring(j.Mobile.Length - 4),
                    InsuranceCorporate = j.InsuranceCompany ?? "",
                    ArrivalDate = DateTime.Now.Date,
                    ArrivalTime = DateTime.Now.ToString("hh:mm tt"),
                    EstDeliveryDate = j.DeliveryDate,
                    AccidentDate = DateTime.Now.Date
                })
                .OrderByDescending(x => x.ArrivalDate)
                .ToListAsync();

            return jobCards;
        }

        public async Task<BaseResultDto> SaveJobCard(JobCardDto dto)
        {
            var job = dto.Id == 0
               ? new JobCard()
               : await _context.JobCards
                   .Include(x => x.Concerns)
                   .Include(x => x.AdvancePayment)
                   .FirstOrDefaultAsync(x => x.Id == dto.Id);
            job.RepairOrderId = dto.RepairOrderId;  
            job.RegistrationNo = dto.VehicleData.RegistrationNo;
            job.OdometerIn = dto.VehicleData.OdometerIn;
            job.AvgKmsPerDay = dto.VehicleData.AvgKmsPerDay;
            job.Vin = dto.VehicleData.Vin;
            job.EngineNo = dto.VehicleData.EngineNo;
            job.VehicleColor = dto.VehicleData.VehicleColor;
            job.FuelType = dto.VehicleData.FuelType;
            job.ServiceType = dto.VehicleData.ServiceType;
            job.ServiceAdvisor = dto.VehicleData.ServiceAdvisor;
            job.Technician = dto.VehicleData.Technician;
            job.Vendor = dto.VehicleData.Vendor;
            job.Corporate = dto.CustomerInfo.Corporate;
            job.CustomerName = dto.CustomerInfo.CustomerName;
            job.Mobile = dto.CustomerInfo.Mobile;
            job.AlternateMobile = dto.CustomerInfo.AlternateMobile;
            job.Email = dto.CustomerInfo.Email;
            job.DeliveryDate = dto.CustomerInfo.DeliveryDate;
            job.InsuranceCompany = dto.CustomerInfo.InsuranceCompany;
            job.Status = "Estimation Pending";
            job.Concerns ??= new List<JobCardConcern>();
            job.Concerns.Clear();
            foreach (var c in dto.Concerns)
            {
                job.Concerns.Add(new JobCardConcern
                {
                    Text = c.Text,
                    Active = c.Active
                });
            }

            job.AdvancePayment ??= new JobCardAdvancePayment();
            job.AdvancePayment.Cash = dto.AdvancePayment.Cash;
            job.AdvancePayment.BankName = dto.AdvancePayment.BankName;
            job.AdvancePayment.ChequeNo = dto.AdvancePayment.ChequeNo;
            job.AdvancePayment.Amount = dto.AdvancePayment.Amount;
            job.AdvancePayment.Date = dto.AdvancePayment.Date;

            if (dto.Id == 0)
            { 
                await _context.JobCards.AddAsync(job); 
            }
            
            await _context.SaveChangesAsync();

            if (dto.Id == 0)
            {
                job.JobCardNo = "SRT-J" + (2992 + job.Id).ToString().PadLeft(6, '0');
                await _context.SaveChangesAsync();
            }
            return new BaseResultDto() {Id = job.Id ,IsSuccess = true,Message="Job Card Saved Sucessfully"};
        }

        public async Task<JobCardDto> GetJobCard(long id)
        {
            var job = await _context.JobCards
                .Include(a=> a.RepairOrder)
                .Include(j => j.Concerns)
                .Include(j => j.AdvancePayment)
                .Where(a=> a.RepairOrderId == id)
                .FirstOrDefaultAsync();


            if (job == null)
                return new JobCardDto() { IsSuccess= false,Message="Not Found"};

            var result = new JobCardDto
            {
                Id = job.Id,
               
                VehicleData = new VehicleDataDto
                {
                    RegistrationNo = job.RegistrationNo,
                    OdometerIn = job.OdometerIn,
                    AvgKmsPerDay = job.AvgKmsPerDay,
                    Vin = job.Vin,
                    EngineNo = job.EngineNo,
                    VehicleColor = job.VehicleColor,
                    FuelType = job.FuelType,
                    ServiceType = job.ServiceType,
                    ServiceAdvisor = job.ServiceAdvisor,
                    Technician = job.Technician,
                    Vendor = job.Vendor,
                    JobCardNo = job.JobCardNo,
                },
                CustomerInfo = new CustomerInfoDto
                {
                    Corporate = job.Corporate,
                    CustomerName = job.CustomerName,
                    Mobile = job.Mobile,
                    AlternateMobile = job.AlternateMobile,
                    Email = job.Email,
                    DeliveryDate = job.DeliveryDate,
                    InsuranceCompany = job.InsuranceCompany
                },
                Concerns = job.Concerns
                    .Select(c => new ConcernDto { Text = c.Text, Active = c.Active })
                    .ToList(),
                AdvancePayment = job.AdvancePayment != null ? new AdvancePaymentDto
                {
                    Cash = job.AdvancePayment.Cash,
                    BankName = job.AdvancePayment.BankName,
                    ChequeNo = job.AdvancePayment.ChequeNo,
                    Amount = job.AdvancePayment.Amount,
                    Date = job.AdvancePayment.Date
                } : null,
                IsSuccess = true,
                Message = "Sucess"
            };

            return result;
        }

        public async Task<GetEstimationDto> GetEstimationDetails(long jobCardId)
        {
            var jobCard = await _context.JobCards
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == jobCardId);

            if (jobCard == null)
                return new GetEstimationDto
                {
                    IsSuccess = false,
                    Message = "JobCard not found"
                };

            var estimationItems = await _context.JobCardEstimationItems.Include(a=> a.Part)
                .AsNoTracking()
                .Where(e => e.JobCardId == jobCardId)
                .ToListAsync();

            var tyreBattery = await _context.JobCardTyreBatteries
                .AsNoTracking()
                .Where(x => x.JobCardId == jobCardId)
                .ToListAsync();

            var cancelledInvoices = await _context.JobCardCancelledInvoices
                .AsNoTracking()
                .Where(x => x.JobCardId == jobCardId)
                .ToListAsync();

            var collections = await _context.JobCardCollections
                .AsNoTracking()
                .Where(x => x.JobCardId == jobCardId)
                .ToListAsync();

            decimal grossAmount = estimationItems.Sum(i => i.Total);
            decimal totalDiscount = estimationItems.Sum(i => i.Discount);
            decimal paidAmount = jobCard.Paid;

            decimal netAmount = grossAmount - totalDiscount;
            decimal balanceAmount = netAmount - paidAmount;

            return new GetEstimationDto()
            {
                IsSuccess = true,
                Message = "Success",
                JobCardId = jobCardId,
                Status = jobCard.Status ?? string.Empty,

                VehicleData = new VehicleDataDto
                {
                    JobCardNo = jobCard.JobCardNo,
                    RegistrationNo = jobCard.RegistrationNo,
                    OdometerIn = jobCard.OdometerIn,
                    AvgKmsPerDay = jobCard.AvgKmsPerDay,
                    Vin = jobCard.Vin,
                    EngineNo = jobCard.EngineNo,
                    VehicleColor = jobCard.VehicleColor,
                    FuelType = jobCard.FuelType,
                    ServiceType = jobCard.ServiceType,
                    ServiceAdvisor = jobCard.ServiceAdvisor,
                    Technician = jobCard.Technician,
                    Vendor = jobCard.Vendor
                },
                CustomerInfo = new CustomerInfoDto
                {
                    Corporate = jobCard.Corporate,
                    CustomerName = jobCard.CustomerName,
                    Mobile = jobCard.Mobile,
                    AlternateMobile = jobCard.AlternateMobile,
                    Email = jobCard.Email,
                    DeliveryDate = jobCard.DeliveryDate,
                    InsuranceCompany = jobCard.InsuranceCompany
                },
                Estimation = new EstimationDto
                {
                    DiscountInput = totalDiscount,
                    PaidAmount = paidAmount,
                    GrossAmount = grossAmount,
                    NetAmount = netAmount,
                    BalanceAmount = balanceAmount,

                    Items = estimationItems.Select(i => new EstimationItemDto
                    {
                        Name = i.Part?.PartName,
                        Type = i.Type,
                        PartNo = i.Part?.PartNo,
                        Rate = i.Rate,
                        Discount = i.Discount,
                        HSN = i.HSN,
                        TaxPercent = i.TaxPercent,
                        TaxAmount = i.TaxAmount,
                        Total = i.Total,
                        Approval = i.ApprovalStatus
                    }).ToList()
                },

                Popup = new PopupDto
                {
                    TyreBattery = tyreBattery.Select(t => new TyreBatteryDto
                    {
                        Type = t.Type,
                        Brand = t.Brand,
                        Model = t.Model,
                        ManufactureDate = t.ManufactureDate,
                        ExpiryDate = t.ExpiryDate,
                        Condition = t.Condition
                    }).ToList(),

                    CancelledInvoices = cancelledInvoices.Select(c => new CancelledInvoiceDto
                    {
                        InvoiceNo = c.InvoiceNo,
                        Date = c.Date,
                        Amount = c.Amount
                    }).ToList(),

                    Collections = collections.Select(c => new CollectionDto
                    {
                        Type = c.Type,
                        Bank = c.Bank,
                        ChequeNo = c.ChequeNo,
                        Amount = c.Amount,
                        Date = c.Date,
                        InvoiceNo = c.InvoiceNo,
                        Remarks = c.Remarks
                    }).ToList(),

                    ServiceSuggestions = jobCard.ServiceSuggestions ?? string.Empty,
                    Remarks = jobCard.Remarks ?? string.Empty,                  
                },
            };
        }

        public async Task<BaseResultDto> SaveEstimationDetails(EstimationItemsSaveDto estimationItem)
        {
            var result = await _context.JobCardEstimationItems.Where(a => a.Id == estimationItem.Id).FirstOrDefaultAsync();
            if (result == null) { result = new JobCardEstimationItem(); };

            result.Id = estimationItem.Id;
            result.JobCardId = estimationItem.JobCardId;
            result.PartId = estimationItem.PartId;
            result.Type = estimationItem.Type;
            result.RequestedQuantity = estimationItem.Quantity; 
            result.Rate = estimationItem.Rate;
            result.Discount = estimationItem.Discount;
            result.HSN = estimationItem.HSN;
            result.TaxPercent = estimationItem.TaxPercent;
            result.TaxAmount = estimationItem.TaxAmount;
            result.Total = estimationItem.Total;
            result.ApprovalStatus = estimationItem.Approval;

            if (estimationItem.Id == 0)
            {
                result.IssuedQty = 0;
                await _context.JobCardEstimationItems.AddAsync(result);

            }
            else
            {
                _context.JobCardEstimationItems.Update(result);
            }
            await _context.SaveChangesAsync();

            return new BaseResultDto() { IsSuccess = true, Message = "SavedSuceesfully"};
        }
    }
}
