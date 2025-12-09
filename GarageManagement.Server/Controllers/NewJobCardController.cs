using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

    }
}
