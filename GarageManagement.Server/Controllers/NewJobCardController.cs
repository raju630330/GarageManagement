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
            var job = new JobCard
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
                VehicleData = new
                {
                    job.Id,
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
