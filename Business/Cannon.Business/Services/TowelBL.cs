using Cannon.Business.DTOs;

namespace Cannon.Business.Services;

public interface ITowelBL
{
    Task<List<TowelDto>> GetAllActiveAsync();
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
}