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

        public WorkshopProfileController(ApplicationDbContext context, IAutoCompleteRepository autoCompleteRepository)
        {
            _context = context;
            _autoCompleteRepository = autoCompleteRepository;
        }


        [HttpPost("createworkshop")]
        public async Task<IActionResult> CreateOrUpdateWorkshop([FromForm] WorkshopCreateDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var workshop = await _context.WorkshopProfiles.Where(a => a.Id == dto.Id).FirstOrDefaultAsync();
                if (workshop == null)
                { workshop = new WorkshopProfile(); }

                /* =========================
                   1️⃣ CREATE OR UPDATE
                ========================= */
                workshop.Id = dto.Id;
                workshop.WorkshopName = dto.WorkshopName;
                workshop.OwnerName = dto.OwnerName;
                workshop.OwnerMobileNo = dto.OwnerMobileNo;
                workshop.EmailID = dto.EmailID;
                workshop.ContactPerson = dto.ContactPerson ?? "";
                workshop.ContactNo = dto.ContactNo ?? "";
                workshop.Landline = dto.Landline ?? "";
                workshop.InBusinessSince = DateTime.Parse(dto.InBusinessSince);
                workshop.AvgVehicleInflowPerMonth = dto.AvgVehicleInflowPerMonth ?? 0;
                workshop.NoOfEmployees = dto.NoOfEmployees ?? 0;
                workshop.DealerCode = dto.DealerCode ?? "";
                workshop.IsGdprAccepted = dto.IsGdprAccepted;

                if (dto.Id == 0)
                {
                    await _context.WorkshopProfiles.AddAsync(workshop);
                }
                else
                {
                    _context.WorkshopProfiles.Update(workshop);
                }

                await _context.SaveChangesAsync();


                /* =========================
                   2️⃣ ADDRESS (UPSERT)
                ========================= */

                var address = await _context.WorkshopAddresses
                    .FirstOrDefaultAsync(x => x.WorkshopId == workshop.Id);

                if (address == null)
                {
                    address = new WorkshopAddress { WorkshopId = workshop.Id };
                    _context.WorkshopAddresses.Add(address);
                }

                address.FlatNo = dto.Address.FlatNo ?? "";
                address.Street = dto.Address.Street ?? "";
                address.Location = dto.Address.Location ?? "";
                address.City = dto.Address.City ?? "";
                address.State = dto.Address.State ?? "";
                address.StateCode = dto.Address.StateCode ?? "";
                address.Country = dto.Address.Country ?? "India";
                address.Pincode = dto.Address.Pincode ?? "";
                address.Landmark = dto.Address.Landmark ?? "";
                address.BranchAddress = dto.Address.BranchAddress ?? "";

                /* =========================
                   3️⃣ TIMING (UPSERT)
                ========================= */


                var timing = await _context.WorkshopTimings
                    .FirstOrDefaultAsync(x => x.WorkshopId == workshop.Id);

                if (timing == null)
                {
                    timing = new WorkshopTiming { WorkshopId = workshop.Id };
                    _context.WorkshopTimings.Add(timing);
                }

                timing.StartTime = TimeSpan.Parse(dto.Timing.StartTime + ":00");
                timing.EndTime = TimeSpan.Parse(dto.Timing.EndTime + ":00");

                /* =========================
                   4️⃣ BUSINESS CONFIG (UPSERT)
                ========================= */

                var config = await _context.WorkshopBusinessConfigs
                    .FirstOrDefaultAsync(x => x.WorkshopId == workshop.Id);

                if (config == null)
                {
                    config = new WorkshopBusinessConfig { WorkshopId = workshop.Id };
                    _context.WorkshopBusinessConfigs.Add(config);
                }

                config.WebsiteLink = dto.BusinessConfig.WebsiteLink;
                config.GoogleReviewLink = dto.BusinessConfig.GoogleReviewLink;
                config.ExternalIntegrationUrl = dto.BusinessConfig.ExternalIntegrationUrl;
                config.GSTIN = dto.BusinessConfig.Gstin;
                config.MSME = dto.BusinessConfig.Msme;
                config.SAC = dto.BusinessConfig.Sac;
                config.SACPercentage = dto.BusinessConfig.SacPercentage;
                config.InvoiceCaption = dto.BusinessConfig.InvoiceCaption;
                config.InvoiceHeader = dto.BusinessConfig.InvoiceHeader;
                config.DefaultServiceType = dto.BusinessConfig.DefaultServiceType;

                /* =========================
                   5️⃣ SERVICES (DELETE + INSERT)
                ========================= */

                _context.WorkshopServices.RemoveRange(
                    _context.WorkshopServices.Where(x => x.WorkshopId == workshop.Id)
                );

                foreach (var s in dto.ServiceIds)
                {
                    if (long.TryParse(s, out var id))
                    {
                        _context.WorkshopServices.Add(new WorkshopService
                        {
                            WorkshopId = workshop.Id,
                            ServiceId = id
                        });
                    }
                }

                /* =========================
                   6️⃣ WORKING DAYS
                ========================= */

                _context.WorkshopWorkingDays.RemoveRange(
                    _context.WorkshopWorkingDays.Where(x => x.WorkshopId == workshop.Id)
                );

                foreach (var d in dto.WorkingDays)
                {
                    if (int.TryParse(d, out var day))
                    {
                        _context.WorkshopWorkingDays.Add(new WorkshopWorkingDay
                        {
                            WorkshopId = workshop.Id,
                            Day = (DayOfWeek)day
                        });
                    }
                }

                /* =========================
                   7️⃣ PAYMENT MODES
                ========================= */

                _context.WorkshopPaymentModes.RemoveRange(
                    _context.WorkshopPaymentModes.Where(x => x.WorkshopId == workshop.Id)
                );

                foreach (var p in dto.PaymentModeIds)
                {
                    if (long.TryParse(p, out var pid))
                    {
                        _context.WorkshopPaymentModes.Add(new WorkshopPaymentMode
                        {
                            WorkshopId = workshop.Id,
                            PaymentModeId = pid
                        });
                    }
                }

                /* =========================
                   8️⃣ MEDIA 
                ========================= */
                if (dto.MediaFiles != null && dto.MediaFiles.Any())
                {
                    string basePath = @"D:\GarageUploads\Workshops";
                    string folder = Path.Combine(basePath, workshop.Id.ToString());
                    Directory.CreateDirectory(folder);

                    foreach (var file in dto.MediaFiles)
                    {
                        var fileName = $"media_{DateTime.Now:yyyyMMddHHmmssfff}{Path.GetExtension(file.FileName)}";
                        var path = Path.Combine(folder, fileName);

                        using var stream = new FileStream(path, FileMode.Create);
                        await file.CopyToAsync(stream);

                        _context.WorkshopMedias.Add(new WorkshopMedia
                        {
                            WorkshopId = workshop.Id,
                            FilePath = $"Workshops/{workshop.Id}/{fileName}",
                            MediaType = file.ContentType
                        });
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    Id = workshop.Id,
                    Message = dto.Id > 0 ? "Workshop updated successfully" : "Workshop created successfully"
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(ex.Message);
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
            var result = await _context.WorkshopUsers.Include(a => a.Workshop).Include(b => b.User).ThenInclude(a => a.Role)
                                        .Where(a => a.UserId == id)
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
                    Gstin = x.WorkshopBusinessConfigs.GSTIN,
                    Location = x.WorkshopBusinessConfigs.GoogleReviewLink
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
                serviceIds = workshop.Services
                    .Select(x => x.ServiceId.ToString())
                    .ToArray(),

                workingDays = workshop.WorkingDays
                    .Select(x => ((int)x.Day).ToString())
                    .ToArray(),

                paymentModeIds = workshop.WorkshopPaymentModes
                    .Select(x => x.PaymentModeId.ToString())
                    .ToArray(),

                mediaFiles = workshop.WorkshopMedias
                                .Where(x => x.MediaType.StartsWith("image"))
                                .Select(x => new
                                {
                                    fileName = Path.GetFileName(x.FilePath),
                                    filePath = x.FilePath,
                                    fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{x.FilePath}",
                                    mediaType = x.MediaType
                                })
                                .ToArray(),

                // STEP 7
                isGdprAccepted = workshop.IsGdprAccepted
            };

            return Ok(result);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAssignedUsers()
        {
            var data = await _context.WorkshopUsers.Include(a => a.User).Include(b => b.Workshop)
                .AsNoTracking()
                .Select(x => new
                {
                    x.Id,
                    x.User.Username,
                    x.Workshop.WorkshopName,
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
            if(id <= 0)
            {
                return BadRequest("id not found");
            }

            var data = await _context.WorkshopUsers.Include(a => a.User).Include(b => b.Workshop)
                .Where(a=> a.Id == id)
                .AsNoTracking()
                .Select(x => new
                {
                    id = x.Id,

                    workshopSearch = $"{x.Workshop.WorkshopName} ({x.Workshop.OwnerMobileNo})",
                    workshopId = x.Workshop.Id,
                    userSearch = $"{x.User.Username} ({x.User.Email})",
                    userId = x.User.Id,
                    isActive = x.IsActive
                })
                .FirstOrDefaultAsync(); 

            return Ok(new
            {
                isSuccess = true,
                Data = data
            });
        }


    }
}
