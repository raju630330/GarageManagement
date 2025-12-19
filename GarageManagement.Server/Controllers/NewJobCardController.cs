using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GarageManagement.Server.Controllers
{
    public class NewJobCardController : BaseAuthorizationController
    {

        private readonly ApplicationDbContext _context;

        public NewJobCardController(ApplicationDbContext context)
        {
            _context = context;
        }


        /*[HttpPost("save-jobcard")]
        public async Task<IActionResult> SaveJobCard([FromBody] JobCardDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid data");

            JobCard job;

            if (dto.Id == 0)
            {
                // ADD NEW
                job = new JobCard
                {
                    RegistrationNo = dto.VehicleData.RegistrationNo, 
                    JobCardNo = "SRT-J" + (2992 + job.Id).ToString().PadLeft(6, '0'),
                    OdometerIn = dto.VehicleData.OdometerIn,
                    AvgKmsPerDay = dto.VehicleData.AvgKmsPerDay,
                    Vin = dto.VehicleData.Vin,
                    EngineNo = dto.VehicleData.EngineNo,
                    VehicleColor = dto.VehicleData.VehicleColor,
                    FuelType = dto.VehicleData.FuelType,
                    ServiceType = dto.VehicleData.ServiceType,
                    ServiceAdvisor = dto.VehicleData.ServiceAdvisor,
                    Technician = dto.VehicleData.Technician,
                    Vendor = dto.VehicleData.Vendor,

                    Corporate = dto.CustomerInfo.Corporate,
                    CustomerName = dto.CustomerInfo.CustomerName,
                    Mobile = dto.CustomerInfo.Mobile,
                    AlternateMobile = dto.CustomerInfo.AlternateMobile,
                    Email = dto.CustomerInfo.Email,
                    DeliveryDate = dto.CustomerInfo.DeliveryDate,
                    InsuranceCompany = dto.CustomerInfo.InsuranceCompany,

                    Concerns = dto.Concerns.Select(c => new JobCardConcern
                    {
                        Text = c.Text,
                        Active = c.Active
                    }).ToList(),

                    AdvancePayment = new JobCardAdvancePayment
                    {
                        Cash = dto.AdvancePayment.Cash,
                        BankName = dto.AdvancePayment.BankName,
                        ChequeNo = dto.AdvancePayment.ChequeNo,
                        Amount = dto.AdvancePayment.Amount,
                        Date = dto.AdvancePayment.Date
                    }
                };

                _context.JobCards.Add(job);
            }
            else
            {
                // UPDATE EXISTING
                job = await _context.JobCards
                    .Include(x => x.Concerns)
                    .Include(x => x.AdvancePayment)
                    .FirstOrDefaultAsync(x => x.Id == dto.Id);

                if (job == null)
                    return NotFound("JobCard not found");

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

                // Replace Concerns
                job.Concerns.Clear();
                foreach (var c in dto.Concerns)
                {
                    job.Concerns.Add(new JobCardConcern
                    {
                        Text = c.Text,
                        Active = c.Active
                    });
                }

                // Update AdvancePayment
                if (job.AdvancePayment == null)
                    job.AdvancePayment = new JobCardAdvancePayment();

                job.AdvancePayment.Cash = dto.AdvancePayment.Cash;
                job.AdvancePayment.BankName = dto.AdvancePayment.BankName;
                job.AdvancePayment.ChequeNo = dto.AdvancePayment.ChequeNo;
                job.AdvancePayment.Amount = dto.AdvancePayment.Amount;
                job.AdvancePayment.Date = dto.AdvancePayment.Date;

                _context.JobCards.Update(job);
            }

            await _context.SaveChangesAsync();



            return Ok(new { id = job.Id });
        }*/

        [HttpPost("save-jobcard")]
        public async Task<IActionResult> SaveJobCard([FromBody] JobCardDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid data");

            var job = dto.Id == 0
                ? new JobCard()
                : await _context.JobCards
                    .Include(x => x.Concerns)
                    .Include(x => x.AdvancePayment)
                    .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (job == null)
                return NotFound("JobCard not found");

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
                _context.JobCards.Add(job);

            await _context.SaveChangesAsync(); 

            if (dto.Id == 0)
            {
                job.JobCardNo = "SRT-J" + (2992 + job.Id).ToString().PadLeft(6, '0');
                await _context.SaveChangesAsync();
            }
            return Ok(new { id = job.Id });
        }

        [HttpGet("search-registration")]
        public async Task<IActionResult> SearchRegistration([FromQuery] string query)
        {
            if (string.IsNullOrEmpty(query))
                return BadRequest("Query is required");

            var registrations = await _context.JobCards
                .Where(j =>
                        EF.Functions.Like(j.RegistrationNo, $"%{query}%") ||
                        EF.Functions.Like(j.JobCardNo, $"%{query}%") ||
                        EF.Functions.Like(j.CustomerName, $"%{query}%") ||
                        EF.Functions.Like(j.Mobile, $"%{query}%") ||
                        EF.Functions.Like(j.Vin, $"%{query}%") ||
                        EF.Functions.Like(j.VehicleColor, $"%{query}%") ||
                        EF.Functions.Like(j.Vin, $"%{query}%") ||
                        EF.Functions.Like(j.Email, $"%{query}%"))
                .Select(j => new IdNameDto { 
                    Id = j.Id,
                    Name = $"{j.JobCardNo} ({j.RegistrationNo})"
                })
                .ToListAsync();

            return Ok(registrations);
        }
        [HttpGet("get-jobcard/{id}")]
        public async Task<IActionResult> GetJobCard(long id)
        {
            var job = await _context.JobCards
                .Include(j => j.Concerns)
                .Include(j => j.AdvancePayment)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (job == null)
                return NotFound();

            var result = new
            {
                job.Id,
                VehicleData = new
                {
                    
                    job.RegistrationNo,
                    job.OdometerIn,
                    job.AvgKmsPerDay,
                    job.Vin,
                    job.EngineNo,
                    job.VehicleColor,
                    job.FuelType,
                    job.ServiceType,
                    job.ServiceAdvisor,
                    job.Technician,
                    job.Vendor,
                    job.JobCardNo,
                    job.Status
                },
                CustomerInfo = new
                {
                    job.Corporate,
                    job.CustomerName,
                    job.Mobile,
                    job.AlternateMobile,
                    job.Email,
                    job.DeliveryDate,
                    job.InsuranceCompany
                },
                Concerns = job.Concerns.Select(c => new { c.Text, c.Active }),
                AdvancePayment = new
                {
                    job.AdvancePayment.Cash,
                    job.AdvancePayment.BankName,
                    job.AdvancePayment.ChequeNo,
                    job.AdvancePayment.Amount,
                    job.AdvancePayment.Date
                }
            };

            return Ok(result);
        }

        [HttpPost("save-estimation")]
        public async Task<IActionResult> saveEstimationDetails(JobCardSaveDto model)
        {
            if (model == null || model.JobCardId == 0)
                return BadRequest("Invalid JobCard data.");

            var jobCard = await _context.JobCards
                .Include(j => j.JobCardEstimationItems)
                .Include(j => j.Collections)
                .Include(j => j.TyreBatteries)
                .Include(j => j.CancelledInvoices)
                .FirstOrDefaultAsync(j => j.Id == model.JobCardId);

            if (jobCard == null)
                return NotFound("JobCard not found.");

            jobCard.JobCardEstimationItems.Clear();
            foreach (var item in model.Estimation.Items)
            {
                jobCard.JobCardEstimationItems.Add(new JobCardEstimationItem
                {
                    Name = item.Name,
                    Type = item.Type,
                    PartNo = item.PartNo,
                    Rate = item.Rate,
                    Discount = item.Discount,
                    HSN = item.HSN,
                    TaxPercent = item.TaxPercent,
                    TaxAmount = item.TaxAmount,
                    Total = item.Total,
                    ApprovalStatus = item.Approval
                });
            }

            jobCard.TyreBatteries.Clear();
            foreach (var tb in model.Popup.TyreBattery)
            {
                jobCard.TyreBatteries.Add(new JobCardTyreBattery
                {
                    Type = tb.Type,
                    Brand = tb.Brand,
                    Model = tb.Model,
                    ManufactureDate = tb.ManufactureDate,
                    ExpiryDate = tb.ExpiryDate,
                    Condition = tb.Condition
                });
            }

            jobCard.CancelledInvoices.Clear();
            foreach (var ci in model.Popup.CancelledInvoices)
            {
                jobCard.CancelledInvoices.Add(new JobCardCancelledInvoice
                {
                    InvoiceNo = ci.InvoiceNo,
                    Date = ci.Date,
                    Amount = ci.Amount
                });
            }

            jobCard.Collections.Clear();
            foreach (var c in model.Popup.Collections)
            {
                jobCard.Collections.Add(new JobCardCollection
                {
                    Type = c.Type,
                    Bank = c.Bank,
                    ChequeNo = c.ChequeNo,
                    Amount = c.Amount,
                    Date = c.Date,
                    InvoiceNo = c.InvoiceNo,
                    Remarks = c.Remarks
                });
            }

            jobCard.Discount = model.Estimation.DiscountInput;
            jobCard.Paid = model.Estimation.PaidAmount;
            jobCard.ServiceSuggestions = model.Popup.ServiceSuggestions;
            jobCard.Remarks = model.Popup.Remarks;
            jobCard.Status = "Estimation Completed";

            _context.JobCards.Update(jobCard);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, jobCardId = jobCard.Id });
        }

        [HttpGet("get-estimation/{jobCardId}")]
        public async Task<IActionResult> GetEstimationDetails(long jobCardId)
        {
            var jobCard = await _context.JobCards
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == jobCardId);

            if (jobCard == null)
                return NotFound("JobCard not found");

            var estimationItems = await _context.JobCardEstimationItems
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

            var result = new
            {
                vehicleData = new
                {
                    registrationNo = jobCard.RegistrationNo,
                    jobCardNumbetForEstimation = jobCard.JobCardNo,
                    odometerIn = jobCard.OdometerIn,
                    serviceType = jobCard.ServiceType,
                    fuelType = jobCard.FuelType,
                    vin = jobCard.Vin,
                    engineNo = jobCard.EngineNo
                },
                customerInfo = new
                {
                    customerName = jobCard.CustomerName,
                    mobile = jobCard.Mobile,
                    email = jobCard.Email
                },
                estimation = new
                {
                    discountInput = totalDiscount,
                    paidAmount = paidAmount,
                    grossAmount = grossAmount,
                    netAmount = netAmount,
                    balanceAmount = balanceAmount,

                    items = estimationItems.Select(i => new
                    {
                        name = i.Name,
                        type = i.Type,
                        partNo = i.PartNo,
                        rate = i.Rate,
                        discount = i.Discount,
                        hSN = i.HSN,
                        taxPercent = i.TaxPercent,
                        taxAmount = i.TaxAmount,
                        total = i.Total
                    })
                },
                popup = new
                {
                    tyreBattery = tyreBattery.Select(t => new
                    {
                        type = t.Type,
                        brand = t.Brand,
                        model = t.Model,
                        manufactureDate = t.ManufactureDate,
                        expiryDate = t.ExpiryDate,
                        condition = t.Condition
                    }),

                    cancelledInvoices = cancelledInvoices.Select(c => new
                    {
                        invoiceNo = c.InvoiceNo,
                        date = c.Date,
                        amount = c.Amount
                    }),

                    collections = collections.Select(c => new
                    {
                        type = c.Type,
                        bank = c.Bank,
                        chequeNo = c.ChequeNo,
                        amount = c.Amount,
                        date = c.Date,
                        invoiceNo = c.InvoiceNo,
                        remarks = c.Remarks
                    }),

                    serviceSuggestions = jobCard.ServiceSuggestions ?? string.Empty,
                    remarks = jobCard.Remarks ?? string.Empty
                }
            };

            return Ok(result);
        }

        [HttpGet("jobcards")]
        public IActionResult GetJobCards()
        {
            var jobCards = _context.JobCards
                .Select(j => new
                {
                    refNo = "REF" + j.Id.ToString().PadLeft(3, '0'),
                    jobCardNo = j.JobCardNo,
                    invoiceNo = "SRT-I" + (2214 + j.Id),
                    claimNo = "CLM" + j.Id.ToString().PadLeft(3, '0'),
                    regNo = j.RegistrationNo ?? "",
                    vehicle = j.VehicleColor ?? "",
                    serviceType = j.ServiceType ?? "",
                    status = j.Status,
                    customerName = j.CustomerName ?? "",
                    mobileNo = string.IsNullOrEmpty(j.Mobile) || j.Mobile.Length < 4
                        ? "******"
                        : "******" + j.Mobile.Substring(j.Mobile.Length - 4),
                    insuranceCorporate = j.InsuranceCompany ?? "",
                    arrivalDate = DateTime.Now.Date,
                    arrivalTime = DateTime.Now.ToString("hh:mm tt"),
                    estDeliveryDate = j.DeliveryDate,
                    accidentDate = DateTime.Now.Date
                })
                .OrderByDescending(x => x.arrivalDate)
                .ToList();

            return Ok(jobCards);
        }

        [HttpGet("searchJobCradsForEstimation")]
        public async Task<IActionResult> SearchJobCradsForEstimation([FromQuery] string query)
        {
            if (string.IsNullOrEmpty(query))
                return BadRequest("Query is required");

            var registrations = await _context.JobCards
                                     .Where(j =>
                                         !j.JobCardEstimationItems.Any() &&
                                         (
                                             EF.Functions.Like(j.RegistrationNo, $"%{query}%") ||
                                             EF.Functions.Like(j.JobCardNo, $"%{query}%") ||
                                             EF.Functions.Like(j.CustomerName, $"%{query}%") ||
                                             EF.Functions.Like(j.Mobile, $"%{query}%") ||
                                             EF.Functions.Like(j.Vin, $"%{query}%")
                                         )
                                     )
                                     .Select(j => new IdNameDto
                                     {
                                         Id = j.Id,
                                         Name = $"{j.JobCardNo} ({j.RegistrationNo})"
                                     }).ToListAsync();


            return Ok(registrations);
        }
        }

    }





    

