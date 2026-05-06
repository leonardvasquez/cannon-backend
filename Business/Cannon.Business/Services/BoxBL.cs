using Cannon.Business.DTOs;
using Cannon.Data.Entities;
using Cannon.Data.Enums;

namespace Cannon.Business.Services;

public interface IBoxBL
{
    Task<List<BoxDto>> GetAllActiveAsync();
    Task<BoxDto> CreateAsync(CreateBoxDto dto);
    Task<TowelDto> PackAsync(int boxId, PackDto dto);
}

public class BoxBL : IBoxBL
{
    private readonly Data.Repositories.IBoxRepository _boxRepository;
    private readonly Data.Repositories.ITowelRepository _towelRepository;

    public BoxBL(Data.Repositories.IBoxRepository boxRepository, Data.Repositories.ITowelRepository towelRepository)
    {
        _boxRepository = boxRepository;
        _towelRepository = towelRepository;
    }

    public async Task<List<BoxDto>> GetAllActiveAsync()
    {
        var boxes = await _boxRepository.GetAllActiveAsync();
        var result = new List<BoxDto>();

        foreach (var box in boxes)
        {
            var currentCount = await _boxRepository.GetCurrentCountAsync(box.Id);
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

        var existing = await _boxRepository.GetByBoxCodeAsync(dto.BoxCode);
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

        await _boxRepository.AddAsync(box);
        await _boxRepository.SaveChangesAsync();

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

    public async Task<TowelDto> PackAsync(int boxId, PackDto dto)
    {
        var box = await _boxRepository.GetByIdAsync(boxId);
        if (box == null || !box.IsActive)
            throw new InvalidOperationException("La caja no existe o no está activa");

        if (box.Status != BoxStatus.OPEN)
            throw new InvalidOperationException("La caja no está abierta");

        var towel = await _towelRepository.GetByIdAsync(dto.TowelId);
        if (towel == null || !towel.IsActive)
            throw new InvalidOperationException("El item no existe o no está activo");

        var currentCount = await _boxRepository.GetCurrentCountAsync(boxId);
        if (currentCount >= box.Capacity)
            throw new InvalidOperationException("Capacidad completa");

        towel.Status = TowelStatus.PACKED;
        towel.BoxId = boxId;

        await _towelRepository.SaveChangesAsync();

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