using MiniHittegods.Application.Interfaces;
using MiniHittegods.Domain.Entities;
using MiniHittegods.Domain.Enums;

namespace MiniHittegods.Application.Services;

public class FoundItemsService
{
    private readonly IFoundItemRepository _repository;

    public FoundItemsService(IFoundItemRepository repository)
    {
        _repository = repository;
    }

    public async Task CreateAsync(FoundItem item)
    {
        await _repository.AddAsync(item);
        await _repository.SaveChangesAsync();
    }

    public async Task<FoundItem?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<FoundItem>> GetAllAsync(
        FoundItemStatus? status,
        string? category,
        string? q)
    {
        return await _repository.GetAllAsync(status, category, q);
    }

    public async Task<FoundItem> ClaimAsync(Guid id, string claimedBy)
    {
        var item = await _repository.GetByIdAsync(id);

        if (item is null)
            throw new InvalidOperationException("Item not found.");

        item.Claim(claimedBy);

        await _repository.SaveChangesAsync();

        return item;
    }

    public async Task DeleteAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync(id);

        if (item is null)
            throw new InvalidOperationException("Item not found.");

        if (!item.CanBeDeleted())
            throw new InvalidOperationException("Item cannot be deleted.");

        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
    }

    public async Task<FoundItem> ReturnAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync(id);

        if (item is null)
            throw new InvalidOperationException("Item not found.");

        item.Return();

        await _repository.SaveChangesAsync();

        return item;
    }
}