using GarageManagement.Server.Data;
using GarageManagement.Server.Model;
using GarageManagement.Server.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace GarageManagement.Server.Repositories
{
    public class StockMovementRepository : IStockMovementRepository
    {
        private readonly ApplicationDbContext _db;

        public StockMovementRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddRangeAsync(IEnumerable<StockMovement> movements)
        {
            await _db.StockMovements.AddRangeAsync(movements);
            await _db.SaveChangesAsync();
        }
    }
}