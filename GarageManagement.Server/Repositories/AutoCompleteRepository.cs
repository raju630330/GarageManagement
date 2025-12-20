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
    }
}
