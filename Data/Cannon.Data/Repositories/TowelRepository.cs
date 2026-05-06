using Microsoft.EntityFrameworkCore;
using Cannon.Data.Entities;

namespace Cannon.Data.Repositories;

public interface ITowelRepository
{
    Task<List<Towel>> GetAllActiveAsync();
    Task<Towel?> GetByIdAsync(int id);
    Task<Towel?> GetByItemCodeAsync(string itemCode);
    Task AddAsync(Towel towel);
    Task SaveChangesAsync();
}

public class TowelRepository : ITowelRepository
{
    private readonly AppDbContext _context;

    public TowelRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Towel>> GetAllActiveAsync()
    {
        return await _context.Towels
            .Where(t => t.IsActive)
            .ToListAsync();
    }

    public async Task<Towel?> GetByIdAsync(int id)
    {
        return await _context.Towels
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Towel?> GetByItemCodeAsync(string itemCode)
    {
        return await _context.Towels
            .FirstOrDefaultAsync(t => t.ItemCode == itemCode);
    }

    public async Task AddAsync(Towel towel)
    {
        await _context.Towels.AddAsync(towel);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}