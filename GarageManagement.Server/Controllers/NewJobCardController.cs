using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Server.Controllers
{
    public class NewJobCardController : BaseAuthorizationController
    {

        private readonly ApplicationDbContext _context;

        public NewJobCardController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost("save-jobcard")]
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

            return Ok(new { registrationNo = job.RegistrationNo });
        }

        [HttpGet("search-registration")]
        public async Task<IActionResult> SearchRegistration([FromQuery] string query)
        {
            if (string.IsNullOrEmpty(query))
                return BadRequest("Query is required");

            var registrations = await _context.JobCards
                .Where(j => j.RegistrationNo.Contains(query))
                .Select(j => new IdNameDto { 
                    Id = j.Id,
                    Name = j.RegistrationNo
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
                    job.Vendor
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

    }
}
