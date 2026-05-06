using Cannon.Business.DTOs;

namespace Cannon.Business.Services;

using Cannon.Data.Entities;
using Cannon.Data.Enums;

public interface ITowelBL
{
    Task<List<TowelDto>> GetAllActiveAsync();
    Task<TowelDto> CreateAsync(CreateTowelDto dto);
}

public class TowelBL : ITowelBL
{
    private readonly Data.Repositories.ITowelRepository _repository;

    public TowelBL(Data.Repositories.ITowelRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TowelDto>> GetAllActiveAsync()
    {
        var towels = await _repository.GetAllActiveAsync();
        return towels.Select(t => new TowelDto
        {
            Id = t.Id,
            ItemCode = t.ItemCode,
            ProductCode = t.ProductCode,
            Status = t.Status.ToString(),
            BoxId = t.BoxId
        }).ToList();
    }

    public async Task<TowelDto> CreateAsync(CreateTowelDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.ItemCode))
            throw new ArgumentException("ItemCode es requerido");

        if (string.IsNullOrWhiteSpace(dto.ProductCode))
            throw new ArgumentException("ProductCode es requerido");

        var existing = await _repository.GetByItemCodeAsync(dto.ItemCode);
        if (existing != null)
            throw new InvalidOperationException($"Ya existe un item con ItemCode '{dto.ItemCode}'");

        var towel = new Towel
        {
            ItemCode = dto.ItemCode,
            ProductCode = dto.ProductCode,
            Status = TowelStatus.LOOSE,
            IsActive = true
        };

        await _repository.AddAsync(towel);
        await _repository.SaveChangesAsync();

        return new TowelDto
        {
            Id = towel.Id,
            ItemCode = towel.ItemCode,
            ProductCode = towel.ProductCode,
            Status = towel.Status.ToString(),
            BoxId = towel.BoxId
        };
    }
}