using Cannon.Business.DTOs;
using Cannon.Data.Entities;
using Cannon.Data.Enums;

namespace Cannon.Business.Services;

public interface IBoxBL
{
    Task<List<BoxDto>> GetAllActiveAsync();
    Task<BoxDto> CreateAsync(CreateBoxDto dto);
}

public class BoxBL : IBoxBL
{
    private readonly Data.Repositories.IBoxRepository _repository;

    public BoxBL(Data.Repositories.IBoxRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<BoxDto>> GetAllActiveAsync()
    {
        var boxes = await _repository.GetAllActiveAsync();
        var result = new List<BoxDto>();

        foreach (var box in boxes)
        {
            var currentCount = await _repository.GetCurrentCountAsync(box.Id);
            result.Add(new BoxDto
            {
                Id = box.Id,
                BoxCode = box.BoxCode,
                ProductCode = box.ProductCode,
                Capacity = box.Capacity,
                Status = box.Status.ToString(),
                CurrentCount = currentCount
            });
        }

        return result;
    }

    public async Task<BoxDto> CreateAsync(CreateBoxDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.BoxCode))
            throw new ArgumentException("BoxCode es requerido");

        if (string.IsNullOrWhiteSpace(dto.ProductCode))
            throw new ArgumentException("ProductCode es requerido");

        if (dto.Capacity <= 0)
            throw new ArgumentException("Capacity debe ser mayor a 0");

        var existing = await _repository.GetByBoxCodeAsync(dto.BoxCode);
        if (existing != null)
            throw new InvalidOperationException($"Ya existe una caja con BoxCode '{dto.BoxCode}'");

        var box = new Box
        {
            BoxCode = dto.BoxCode,
            ProductCode = dto.ProductCode,
            Capacity = dto.Capacity,
            Status = BoxStatus.OPEN,
            IsActive = true
        };

        await _repository.AddAsync(box);
        await _repository.SaveChangesAsync();

        return new BoxDto
        {
            Id = box.Id,
            BoxCode = box.BoxCode,
            ProductCode = box.ProductCode,
            Capacity = box.Capacity,
            Status = box.Status.ToString(),
            CurrentCount = 0
        };
    }
}