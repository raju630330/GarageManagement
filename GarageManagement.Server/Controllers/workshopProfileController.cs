using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace GarageManagement.Server.Controllers
{
    public class WorkshopProfileController : BaseAuthorizationController
    {
        private readonly ApplicationDbContext _context;
        private readonly IAutoCompleteRepository _autoCompleteRepository;

        public WorkshopProfileController(ApplicationDbContext context,IAutoCompleteRepository autoCompleteRepository)
        {
            _context = context;
            _autoCompleteRepository = autoCompleteRepository;
        }


        [HttpPost("createworkshop")]
        public async Task<IActionResult> CreateWorkshop([FromBody] WorkshopCreateDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1️⃣ Workshop Profile
                var workshop = new WorkshopProfile
                {
                    WorkshopName = dto.WorkshopName,
                    OwnerName = dto.OwnerName,
                    OwnerMobileNo = dto.OwnerMobileNo,
                    EmailID = dto.EmailID,
                    ContactPerson = dto.ContactPerson ?? "",
                    ContactNo = dto.ContactNo ?? "",
                    Landline = dto.Landline ?? "",
                    InBusinessSince = DateTime.Parse(dto.InBusinessSince), // "2026-02" → DateTime
                    AvgVehicleInflowPerMonth = dto.AvgVehicleInflowPerMonth ?? 0,
                    NoOfEmployees = dto.NoOfEmployees ?? 0,
                    DealerCode = dto.DealerCode ?? "",
                    IsGdprAccepted = dto.IsGdprAccepted
                };

                _context.WorkshopProfiles.Add(workshop);
                await _context.SaveChangesAsync();

                // 2️⃣ Address
                _context.WorkshopAddresses.Add(new WorkshopAddress
                {
                    WorkshopId = workshop.Id,
                    FlatNo = dto.Address.FlatNo ?? "",
                    Street = dto.Address.Street ?? "",
                    Location = dto.Address.Location ?? "",
                    City = dto.Address.City ?? "",
                    State = dto.Address.State ?? "",
                    StateCode = dto.Address.StateCode ?? "",
                    Country = dto.Address.Country ?? "India",
                    Pincode = dto.Address.Pincode ?? "",
                    Landmark = dto.Address.Landmark ?? "",
                    BranchAddress = dto.Address.BranchAddress ?? ""
                });

                // 3️⃣ Timing - ✅ Now strings, convert to TimeSpan
                _context.WorkshopTimings.Add(new WorkshopTiming
                {
                    WorkshopId = workshop.Id,
                    StartTime = TimeSpan.Parse(dto.Timing.StartTime + ":00"), // "16:26" → TimeSpan
                    EndTime = TimeSpan.Parse(dto.Timing.EndTime + ":00")      // "22:32" → TimeSpan
                });

                // 4️⃣ Business Config
                _context.WorkshopBusinessConfigs.Add(new WorkshopBusinessConfig
                {
                    WorkshopId = workshop.Id,
                    WebsiteLink = dto.BusinessConfig.WebsiteLink,
                    GoogleReviewLink = dto.BusinessConfig.GoogleReviewLink,
                    ExternalIntegrationUrl = dto.BusinessConfig.ExternalIntegrationUrl,
                    GSTIN = dto.BusinessConfig.Gstin,
                    MSME = dto.BusinessConfig.Msme,
                    SAC = dto.BusinessConfig.Sac,
                    SACPercentage = decimal.TryParse(dto.BusinessConfig.SacPercentage, out var sac) ? sac : null,
                    InvoiceCaption = dto.BusinessConfig.InvoiceCaption,
                    InvoiceHeader = dto.BusinessConfig.InvoiceHeader,
                    DefaultServiceType = dto.BusinessConfig.DefaultServiceType
                });

                // 5️⃣ Services - ✅ Convert string[] → long[]
                foreach (var serviceIdStr in dto.ServiceIds)
                {
                    if (long.TryParse(serviceIdStr, out var serviceId))
                    {
                        _context.WorkshopServices.Add(new WorkshopService
                        {
                            WorkshopId = workshop.Id,
                            ServiceId = serviceId
                        });
                    }
                }

                // 6️⃣ Working Days - ✅ Convert string[] → DayOfWeek
                foreach (var dayStr in dto.WorkingDays)
                {
                    if (int.TryParse(dayStr, out var dayNum) && Enum.IsDefined(typeof(DayOfWeek), dayNum))
                    {
                        _context.WorkshopWorkingDays.Add(new WorkshopWorkingDay
                        {
                            WorkshopId = workshop.Id,
                            Day = (DayOfWeek)dayNum  // "0" → Sunday, "1" → Monday, etc.
                        });
                    }
                }

                // 7️⃣ Payment Modes - ✅ Convert string[] → long[]
                foreach (var paymentIdStr in dto.PaymentModeIds)
                {
                    if (long.TryParse(paymentIdStr, out var paymentId))
                    {
                        _context.WorkshopPaymentModes.Add(new WorkshopPaymentMode
                        {
                            WorkshopId = workshop.Id,
                            PaymentModeId = paymentId
                        });
                    }
                }

                // 8️⃣ Media - List<string> → WorkshopMediaDto
                foreach (var mediaPath in dto.Media)
                {
                    _context.WorkshopMedias.Add(new WorkshopMedia
                    {
                        WorkshopId = workshop.Id,
                        FilePath = mediaPath ?? "",
                        MediaType = "image" // Default for now
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { Id = workshop.Id, Message = "Workshop created successfully" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("searchWorshops")]
        public async Task<IActionResult> searchWorshops([FromQuery] string query)
        {
            if (string.IsNullOrEmpty(query))
                return BadRequest("Query is required");

            var registrations = await _autoCompleteRepository.SearchWorkshops(query);
            return Ok(registrations);
        }
        [HttpGet("searchUsers")]
        public async Task<IActionResult> searchUsers([FromQuery] string query)
        {
            if (string.IsNullOrEmpty(query))
                return BadRequest("Query is required");

            var registrations = await _autoCompleteRepository.SearchUsers(query);
            return Ok(registrations);
        }
        [HttpPost("assignuser")]
        public async Task<IActionResult> AssignUsersToWorkshop(AssignUserToWorkshopDto model)
        { 
            var result = await _context.WorkshopUsers.FindAsync(model.Id);
            if (result == null)
            {
                result = new WorkshopUser();
            }
            result.Id = model.Id;   
            result.UserId = model.UserId;   
            result.WorkshopId = model.WorkshopId;
            result.IsActive = true;
            if (model.Id == 0)
            {
                await _context.WorkshopUsers.AddAsync(result);
            }
            else
            { 
                 _context.WorkshopUsers.Update(result);   
            }
            
            await _context.SaveChangesAsync();

            return Ok(new { isSuccess = true, Message = "User Assigned successfully" });
        }

        [HttpGet("GetWorkshopsByUser")]
        public async Task<IActionResult> GetWorkshopsByUser(long id)
        {
            var result = await _context.WorkshopUsers.Include(a=> a.Workshop).Include(b=> b.User).ThenInclude(a=> a.Role)
                                        .Where(a=> a.UserId == id)
                                        .Select(a => new
                                        {
                                            Id = a.Id,  
                                            UserName = a.User.Username,
                                            RoleName = a.User.Role.RoleName,
                                            WorshopName = a.Workshop.WorkshopName,               
                                        }).ToListAsync();

            if (result == null)
            { 
                return NotFound();  
            }
            return Ok(result);  
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllWorkshops(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.WorkshopProfiles
                .Include(x => x.Address)
                .Include(x => x.WorkshopBusinessConfigs)
                .OrderByDescending(x => x.Id)
                .AsQueryable();

            var totalRecords = await query.CountAsync();

            var workshops = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new WorkshopListDto
                {
                    Id = x.Id,
                    WorkshopName = x.WorkshopName,
                    OwnerName = x.OwnerName,
                    OwnerMobileNo = x.OwnerMobileNo,
                    City = x.Address.City,
                    Gstin = x.WorkshopBusinessConfigs.GSTIN
                })
                .ToListAsync();

            var response = new PagedResponse<WorkshopListDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = workshops
            };

            return Ok(response);
        }

        [HttpGet("getWorkshopById/{id}")]
        public async Task<IActionResult> GetWorkshopById(long id)
        {
            var workshop = await _context.WorkshopProfiles
                .Include(x => x.Address)
                .Include(x => x.Timing)
                .Include(x => x.WorkshopBusinessConfigs)
                .Include(x => x.Services)
                .Include(x => x.WorkingDays)
                .Include(x => x.WorkshopPaymentModes)
                .Include(x => x.WorkshopMedias)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (workshop == null)
                return NotFound();

            var result = new
            {
                id = workshop.Id,

                // STEP 1
                workshopName = workshop.WorkshopName,
                ownerName = workshop.OwnerName,
                ownerMobileNo = workshop.OwnerMobileNo,
                emailID = workshop.EmailID,
                contactPerson = workshop.ContactPerson,
                landline = workshop.Landline,

                // STEP 2 - Address
                address = workshop.Address == null ? null : new
                {
                    workshop.Address.FlatNo,
                    workshop.Address.Street,
                    workshop.Address.Location,
                    workshop.Address.City,
                    workshop.Address.State,
                    workshop.Address.StateCode,
                    workshop.Address.Country,
                    workshop.Address.Pincode,
                    workshop.Address.Landmark,
                    workshop.Address.BranchAddress
                },

                // STEP 3
                inBusinessSince = workshop.InBusinessSince,
                avgVehicleInflowPerMonth = workshop.AvgVehicleInflowPerMonth,
                noOfEmployees = workshop.NoOfEmployees,
                dealerCode = workshop.DealerCode,

                // STEP 4 - Timing
                timing = workshop.Timing == null ? null : new
                {
                    startTime = workshop.Timing.StartTime,
                    endTime = workshop.Timing.EndTime
                },

                // STEP 5 - Business Config
                businessConfig = workshop.WorkshopBusinessConfigs == null ? null : new
                {
                    websiteLink = workshop.WorkshopBusinessConfigs.WebsiteLink,
                    googleReviewLink = workshop.WorkshopBusinessConfigs.GoogleReviewLink,
                    externalIntegrationUrl = workshop.WorkshopBusinessConfigs.ExternalIntegrationUrl,
                    gstin = workshop.WorkshopBusinessConfigs.GSTIN,
                    msme = workshop.WorkshopBusinessConfigs.MSME,
                    sac = workshop.WorkshopBusinessConfigs.SAC,
                    sacPercentage = workshop.WorkshopBusinessConfigs.SACPercentage
                },

                // STEP 6 - Lists (IDs only)
                serviceIds = workshop.Services
                    .Select(x => x.ServiceId.ToString())
                    .ToArray(),

                workingDays = workshop.WorkingDays
                    .Select(x => ((int)x.Day).ToString())
                    .ToArray(),

                paymentModeIds = workshop.WorkshopPaymentModes
                    .Select(x => x.PaymentModeId.ToString())
                    .ToArray(),

                media = workshop.WorkshopMedias
                    .Select(x => x.FilePath)
                    .ToArray(),

                // STEP 7
                isGdprAccepted = workshop.IsGdprAccepted
            };

            return Ok(result);
        }



    }
}
