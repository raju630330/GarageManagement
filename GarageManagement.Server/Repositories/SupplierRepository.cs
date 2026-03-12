
using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageManagement.Server.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ApplicationDbContext _db;

        public SupplierRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<SupplierDto>> GetAllActiveAsync(long workshopId)
        {
            return await _db.Suppliers
                .Where(s => s.WorkshopId == workshopId && s.IsActive && s.RowState == 1)
                .OrderBy(s => s.Name)
                .Select(s => new SupplierDto
                {
                    SupplierId = s.Id,
                    Name = s.Name,
                    ContactPerson = s.ContactPerson,
                    Phone = s.Phone
                })
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(long id, long workshopId)
        {
            return await _db.Suppliers
                .AnyAsync(s => s.Id == id
                            && s.WorkshopId == workshopId
                            && s.IsActive
                            && s.RowState == 1);
        }

        public async Task<long> CreateAsync(Supplier supplier)
        {
            _db.Suppliers.Add(supplier);
            await _db.SaveChangesAsync();
            return supplier.Id;
        }
    }
}