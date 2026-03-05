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
        private readonly IHelperRepository _helperRepository;
        public WorkshopProfileController(ApplicationDbContext context, IAutoCompleteRepository autoCompleteRepository,IHelperRepository helperRepository)
        {
            _context = context;
            _autoCompleteRepository = autoCompleteRepository;
            _helperRepository = helperRepository;
        }

        [HttpPost("createworkshop")]
        public async Task<IActionResult> CreateOrUpdateWorkshop([FromForm] WorkshopCreateDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var currentWorkshopId = _helperRepository.GetWorkshopId();

                var workshop = await _context.WorkshopProfiles
                    .Include(x => x.Services)
                    .Include(x => x.WorkingDays)
                    .Include(x => x.WorkshopPaymentModes)
                    .Include(x => x.Address)
                    .Include(x => x.Timing)
                    .Include(x => x.WorkshopBusinessConfigs)
                    .Include(x => x.WorkshopMedias)
                    .FirstOrDefaultAsync(x => x.Id == dto.Id);

                bool isNew = workshop == null;

                if (isNew)
                {
                    workshop = new WorkshopProfile
                    {
                        ParentWorkshopId = currentWorkshopId > 0 ? currentWorkshopId : null,
                        Address = new WorkshopAddress(),
                        Timing = new WorkshopTiming(),
                        WorkshopBusinessConfigs = new WorkshopBusinessConfig(),
                        Services = new List<WorkshopService>(),
                        WorkingDays = new List<WorkshopWorkingDay>(),
                        WorkshopPaymentModes = new List<WorkshopPaymentMode>(),
                        WorkshopMedias = new List<WorkshopMedia>()
                    };
                    _context.WorkshopProfiles.Add(workshop);
                }
                else
                {
                    workshop.ParentWorkshopId = currentWorkshopId > 0 ? currentWorkshopId : null;
                    workshop.Address ??= new WorkshopAddress();
                    workshop.Timing ??= new WorkshopTiming();
                    workshop.WorkshopBusinessConfigs ??= new WorkshopBusinessConfig();
                    workshop.Services ??= new List<WorkshopService>();
                    workshop.WorkingDays ??= new List<WorkshopWorkingDay>();
                    workshop.WorkshopPaymentModes ??= new List<WorkshopPaymentMode>();
                    workshop.WorkshopMedias ??= new List<WorkshopMedia>();
                }

                // =====================
                // BASIC DETAILS
                // =====================
                workshop.WorkshopName = dto.WorkshopName?.Trim() ?? "";
                workshop.OwnerName = dto.OwnerName?.Trim() ?? "";
                workshop.OwnerMobileNo = dto.OwnerMobileNo?.Trim() ?? "";
                workshop.EmailID = dto.EmailID?.Trim() ?? "";
                workshop.ContactPerson = dto.ContactPerson?.Trim() ?? "";
                workshop.ContactNo = dto.ContactNo?.Trim() ?? "";
                workshop.Landline = dto.Landline?.Trim() ?? "";
                workshop.DealerCode = dto.DealerCode?.Trim() ?? "";
                workshop.InBusinessSince = DateTime.Parse(dto.InBusinessSince);
                workshop.AvgVehicleInflowPerMonth = dto.AvgVehicleInflowPerMonth ?? 0;
                workshop.NoOfEmployees = dto.NoOfEmployees ?? 0;
                workshop.IsGdprAccepted = dto.IsGdprAccepted;

                // =====================
                // ADDRESS
                // =====================
                workshop.Address.FlatNo = dto.Address.FlatNo?.Trim() ?? "";
                workshop.Address.Street = dto.Address.Street?.Trim() ?? "";
                workshop.Address.Location = dto.Address.Location?.Trim() ?? "";
                workshop.Address.City = dto.Address.City?.Trim() ?? "";
                workshop.Address.State = dto.Address.State?.Trim() ?? "";
                workshop.Address.StateCode = dto.Address.StateCode?.Trim() ?? "";
                workshop.Address.Country = dto.Address.Country?.Trim() ?? "India";
                workshop.Address.Pincode = dto.Address.Pincode?.Trim() ?? "";
                workshop.Address.Landmark = dto.Address.Landmark?.Trim() ?? "";
                workshop.Address.BranchAddress = dto.Address.BranchAddress?.Trim() ?? "";

                // =====================
                // TIMING
                // =====================
                workshop.Timing.StartTime = TimeSpan.Parse(dto.Timing.StartTime + ":00");
                workshop.Timing.EndTime = TimeSpan.Parse(dto.Timing.EndTime + ":00");

                // =====================
                // BUSINESS CONFIG
                // =====================
                workshop.WorkshopBusinessConfigs.WebsiteLink = dto.BusinessConfig.WebsiteLink?.Trim() ?? "";
                workshop.WorkshopBusinessConfigs.GoogleReviewLink = dto.BusinessConfig.GoogleReviewLink?.Trim() ?? "";
                workshop.WorkshopBusinessConfigs.ExternalIntegrationUrl = dto.BusinessConfig.ExternalIntegrationUrl?.Trim() ?? "";
                workshop.WorkshopBusinessConfigs.GSTIN = dto.BusinessConfig.Gstin?.Trim() ?? "";
                workshop.WorkshopBusinessConfigs.MSME = dto.BusinessConfig.Msme?.Trim() ?? "";
                workshop.WorkshopBusinessConfigs.SAC = dto.BusinessConfig.Sac?.Trim() ?? "";
                workshop.WorkshopBusinessConfigs.SACPercentage = dto.BusinessConfig.SacPercentage;
                workshop.WorkshopBusinessConfigs.InvoiceCaption = dto.BusinessConfig.InvoiceCaption?.Trim() ?? "";
                workshop.WorkshopBusinessConfigs.InvoiceHeader = dto.BusinessConfig.InvoiceHeader?.Trim() ?? "";
                workshop.WorkshopBusinessConfigs.DefaultServiceType = dto.BusinessConfig.DefaultServiceType?.Trim() ?? "";

                // =====================
                // SERVICES SYNC
                // =====================
                var newServiceIds = dto.ServiceIds.Select(long.Parse).ToHashSet();
                var existingServiceIds = workshop.Services.Select(x => x.ServiceId).ToHashSet();

                workshop.Services.RemoveAll(x => !newServiceIds.Contains(x.ServiceId));

                foreach (var id in newServiceIds.Except(existingServiceIds))
                    workshop.Services.Add(new WorkshopService { ServiceId = id });

                // =====================
                // WORKING DAYS SYNC
                // =====================
                var newDays = dto.WorkingDays.Select(x => (DayOfWeek)int.Parse(x)).ToHashSet();
                var existingDays = workshop.WorkingDays.Select(x => x.Day).ToHashSet();

                workshop.WorkingDays.RemoveAll(x => !newDays.Contains(x.Day));

                foreach (var day in newDays.Except(existingDays))
                    workshop.WorkingDays.Add(new WorkshopWorkingDay { Day = day });

                // =====================
                // PAYMENT MODES SYNC
                // =====================
                var newPaymentIds = dto.PaymentModeIds.Select(long.Parse).ToHashSet();
                var existingPaymentIds = workshop.WorkshopPaymentModes.Select(x => x.PaymentModeId).ToHashSet();

                workshop.WorkshopPaymentModes.RemoveAll(x => !newPaymentIds.Contains(x.PaymentModeId));

                foreach (var id in newPaymentIds.Except(existingPaymentIds))
                    workshop.WorkshopPaymentModes.Add(new WorkshopPaymentMode { PaymentModeId = id });

                // =====================
                // FIRST SAVE — generates workshop.Id for new records
                // =====================
                await _context.SaveChangesAsync();

                // =====================
                // MEDIA FILES — after first save so workshop.Id is valid
                // =====================
                if (dto.MediaFiles?.Any() == true)
                {
                    string folder = Path.Combine(@"C:\GarageUploads\Workshops", workshop.Id.ToString());
                    Directory.CreateDirectory(folder);

                    foreach (var file in dto.MediaFiles)
                    {
                        var ext = Path.GetExtension(file.FileName);
                        var fileName = $"media_{DateTime.Now:yyyyMMddHHmmssfff}{ext}";
                        var filePath = Path.Combine(folder, fileName);

                        using var stream = new FileStream(filePath, FileMode.Create);
                        await file.CopyToAsync(stream);

                        workshop.WorkshopMedias.Add(new WorkshopMedia
                        {
                            FilePath = $"Workshops/{workshop.Id}/{fileName}",
                            MediaType = file.ContentType
                        });
                    }

                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return Ok(new
                {
                    workshop.Id,
                    Message = isNew ? "Workshop created successfully" : "Workshop updated successfully"
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
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
            var result = await _context.WorkshopUsers
                                        .Where(a => a.UserId == id)
                                        .Select(a => new
                                        {
                                            a.Id,
                                            UserName = a.User != null ? a.User.Username : null,
                                            RoleName = a.User != null && a.User.Role != null
                                                        ? a.User.Role.RoleName
                                                        : null,
                                            WorkshopName = a.Workshop != null
                                                        ? a.Workshop.WorkshopName
                                                        : null
                                        })
                                        .ToListAsync();
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
          City = x.Address != null ? x.Address.City : string.Empty,
          Gstin = x.WorkshopBusinessConfigs != null ? x.WorkshopBusinessConfigs.GSTIN : string.Empty,
          Location = x.WorkshopBusinessConfigs != null ? x.WorkshopBusinessConfigs.GoogleReviewLink : string.Empty
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
                contactNo = workshop.ContactNo,
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
                serviceIds = workshop.Services?
                    .Select(x => x.ServiceId.ToString())
                    .ToArray() ?? Array.Empty<string>(),

                workingDays = workshop.WorkingDays?
                    .Select(x => ((int)x.Day).ToString())
                    .ToArray() ?? Array.Empty<string>(),

                paymentModeIds = workshop.WorkshopPaymentModes?
                    .Select(x => x.PaymentModeId.ToString())
                    .ToArray() ?? Array.Empty<string>(),

                mediaFiles = workshop.WorkshopMedias?
                    .Where(x => !string.IsNullOrEmpty(x.MediaType)
                             && x.MediaType.StartsWith("image"))
                    .Select(x => new
                    {
                        fileName = Path.GetFileName(x.FilePath ?? string.Empty),
                        filePath = x.FilePath,
                        fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{x.FilePath}",
                        mediaType = x.MediaType
                    })
                    .ToArray() ?? Array.Empty<object>(),

                // STEP 7
                isGdprAccepted = workshop.IsGdprAccepted
            };

            return Ok(result);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAssignedUsers()
        {
            var data = await _context.WorkshopUsers
                                    .AsNoTracking()
                                    .Select(x => new
                                    {
                                        x.Id,
                                        UserName = x.User != null ? x.User.Username : null,
                                        WorkshopName = x.Workshop != null ? x.Workshop.WorkshopName : null,
                                        x.IsActive
                                    })
                                    .ToListAsync();

            return Ok(new
            {
                isSuccess = true,
                Data = data
            });
        }

        [HttpGet("getAssignedUser/{id}")]

        public async Task<IActionResult> getAssignedUser(long id)
        {
            if (id <= 0)
            {
                return BadRequest("id not found");
            }

            var data = await _context.WorkshopUsers
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    id = x.Id,

                    workshopSearch = x.Workshop != null
                        ? $"{x.Workshop.WorkshopName} ({x.Workshop.OwnerMobileNo})"
                        : null,

                    workshopId = x.Workshop != null ? x.Workshop.Id : 0,

                    userSearch = x.User != null
                        ? $"{x.User.Username} ({x.User.Email})"
                        : null,

                    userId = x.User != null ? x.User.Id : 0,

                    isActive = x.IsActive
                })
                .FirstOrDefaultAsync();

            if (data == null)
            {
                return NotFound("Assigned user not found");
            }

            return Ok(new
            {
                isSuccess = true,
                Data = data
            });
        }

        [HttpGet("services")]
        public async Task<IActionResult> GetServices()
        {
            var services = await _context.Services
                .Where(x => x.RowState < 3)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name
                })
                .ToListAsync();

            return Ok(services);
        }

        [HttpGet("payment-modes")]
        public async Task<IActionResult> GetPaymentModes()
        {
            var modes = await _context.PaymentModes
                .Where(x => x.RowState < 3)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name
                })
                .ToListAsync();

            return Ok(modes);
        }
    }
}
