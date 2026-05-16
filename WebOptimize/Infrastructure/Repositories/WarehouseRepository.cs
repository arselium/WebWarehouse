using WebOptimize.Domain.Entities;
using WebOptimize.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace WebOptimize.Infrastructure.Repositories
{
    public class WarehouseRepository
    {
        private readonly AppDbContext _context;
        public WarehouseRepository(AppDbContext context) => _context = context;

        public Task<List<Warehouse>> GetAllAsync() => _context.Warehouses.ToListAsync();
    }
}
