using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace GarageManagement.Server.Controllers
{
    public class SupplierController : BaseAuthorizationController   
    {
        private readonly ISupplierRepository _repo;

        public SupplierController(ISupplierRepository repo)
        {
            _repo = repo;
        }

        // GET /api/supplier
        // Populates the Vendor dropdown in the Angular order form
        [HttpGet]
        public async Task<IActionResult> GetSuppliers()
        {
            var suppliers = await _repo.GetAllActiveAsync(GetWorkshopId());
            return Ok(suppliers);
        }

        // POST /api/supplier
        [HttpPost]
        public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { isSuccess = false, message = "Invalid request data" });

            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(new { isSuccess = false, message = "Supplier name is required" });

            var supplierId = await _repo.CreateAsync(new Supplier
            {
                Name = dto.Name.Trim(),
                ContactPerson = dto.ContactPerson?.Trim() ?? string.Empty,
                Phone = dto.Phone?.Trim() ?? string.Empty,
                Email = dto.Email?.Trim() ?? string.Empty,
                GSTIN = dto.GSTIN?.Trim() ?? string.Empty,
                Address = dto.Address?.Trim() ?? string.Empty,
                IsActive = true,
                WorkshopId = GetWorkshopId(),
                RowState = 1,
                ModifiedBy = GetUserId(),
                ModifiedOn = DateTime.UtcNow
            });

            return Ok(new { isSuccess = true, supplierId, message = "Supplier created successfully" });
        }

        // Replace with your actual JWT claim helpers
        private long GetWorkshopId() =>
            long.Parse(User.FindFirst("WorkshopId")?.Value ?? "0");

        private long GetUserId() =>
            long.Parse(User.FindFirst("UserId")?.Value ?? "0");
    }
}