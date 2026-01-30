using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Server.Repositories
{  
    public class AutoCompleteRepository:IAutoCompleteRepository
    {
        private readonly ApplicationDbContext _context;
        public AutoCompleteRepository(ApplicationDbContext context)
        {
            _context = context; 
        }
        public async Task<IList<IdNameDto>> SearchRegistration(string query)
        {
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
                .Select(j => new IdNameDto
                {
                    Id = j.Id,
                    Name = $"{j.JobCardNo} ({j.RegistrationNo})"
                })
                .ToListAsync();

            return registrations;
        }
        public async Task<IList<IdNameDto>> SearchJobCradsForEstimation(string query)
        {
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
            return registrations;
        }

        public async Task<IList<IdNameDto>> SearchBookingCustomer(string query)
        {
            return await _context.Customers
                .Include(c => c.Vehicles)
                .Where(c =>
                    EF.Functions.Like(c.CustomerName, $"%{query}%") ||
                    EF.Functions.Like(c.MobileNo, $"%{query}%") ||
                    EF.Functions.Like(c.Email, $"%{query}%") ||
                    c.Vehicles.Any(v =>
                        EF.Functions.Like(v.RegNo, $"%{query}%"))
                )
                .Select(c => new IdNameDto
                {
                    Id = c.Id,
                    Name =
                        c.CustomerName +
                        " | " + c.MobileNo +
                        " | " + (c.Vehicles.FirstOrDefault().RegPrefix ?? "") +
                        (c.Vehicles.FirstOrDefault().RegNo ?? "")
                })
                .ToListAsync();
        }

        public async Task<IList<IdNameDto>> SearchBookingAppointment(string query)
        {
            return await _context.BookAppointments
                .Include(b => b.Customer)
                .Include(b => b.Vehicle)
                .Where(b =>
                    EF.Functions.Like(b.Customer.CustomerName, $"%{query}%") ||
                    EF.Functions.Like(b.Customer.MobileNo, $"%{query}%") ||
                    EF.Functions.Like(b.Vehicle.RegNo, $"%{query}%")
                )
                .Select(b => new IdNameDto
                {
                    Id = b.Id,   // 👈 booking id (separate)

                    Name =
                            
                            b.Customer.CustomerName +
                            " | " + b.Customer.MobileNo +
                            " | " +
                            (b.Vehicle.RegPrefix ?? "") +
                            (b.Vehicle.RegNo ?? "")+ " | " +
                            b.AppointmentDate.ToString("dd-MMM-yyyy") + " " +
                            DateTime.Parse(b.AppointmentTime).ToString("hh:mm tt") 

                })
                .OrderByDescending(b => b.Id)
                .Take(20)
                .ToListAsync();
        }

        public async Task<IList<IdNameDto>> SearchRepairOrder(string query)
        {
            return await _context.RepairOrders
                .Include(r => r.BookAppointment)
                    .ThenInclude(b => b.Customer)
                .Include(r => r.BookAppointment)
                    .ThenInclude(b => b.Vehicle)
                .Where(r =>
                    EF.Functions.Like(r.BookAppointment.Customer.CustomerName, $"%{query}%") ||
                    EF.Functions.Like(r.BookAppointment.Customer.MobileNo, $"%{query}%") ||
                    EF.Functions.Like(r.BookAppointment.Vehicle.RegNo, $"%{query}%") ||
                    EF.Functions.Like(r.VinNumber, $"%{query}%")
                )
                .Select(r => new IdNameDto
                {
                    Id = r.Id,  

                    Name =
                        r.BookAppointment.Customer.CustomerName +
                        " | " + r.BookAppointment.Customer.MobileNo +
                        " | " +
                        (r.BookAppointment.Vehicle.RegPrefix ?? "") +
                        (r.BookAppointment.Vehicle.RegNo ?? "")
                })
                .OrderByDescending(r => r.Id)
                .Take(20)
                .ToListAsync();
        }


    }
}
