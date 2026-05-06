using Microsoft.EntityFrameworkCore;
using Cannon.Data.Entities;

namespace Cannon.Data.Repositories;

public interface ITowelRepository
{
    Task<List<Towel>> GetAllActiveAsync();
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
}