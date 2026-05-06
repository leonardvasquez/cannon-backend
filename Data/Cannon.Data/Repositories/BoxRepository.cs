using Microsoft.EntityFrameworkCore;
using Cannon.Data.Entities;

namespace Cannon.Data.Repositories;

public interface IBoxRepository
{
    Task<List<Box>> GetAllActiveAsync();
    Task<Box?> GetByIdAsync(int id);
    Task<Box?> GetByBoxCodeAsync(string boxCode);
    Task<int> GetCurrentCountAsync(int boxId);
    Task AddAsync(Box box);
    Task SaveChangesAsync();
}

public class BoxRepository : IBoxRepository
{
    private readonly AppDbContext _context;

    public BoxRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Box>> GetAllActiveAsync()
    {
        return await _context.Boxes
            .Where(b => b.IsActive)
            .ToListAsync();
    }

    public async Task<Box?> GetByIdAsync(int id)
    {
        return await _context.Boxes
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Box?> GetByBoxCodeAsync(string boxCode)
    {
        return await _context.Boxes
            .FirstOrDefaultAsync(b => b.BoxCode == boxCode);
    }

    public async Task<int> GetCurrentCountAsync(int boxId)
    {
        return await _context.Towels
            .CountAsync(t => t.BoxId == boxId && t.IsActive && t.Status == Enums.TowelStatus.PACKED);
    }

    public async Task AddAsync(Box box)
    {
        await _context.Boxes.AddAsync(box);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}