using MiniHittegods.Application.Interfaces;
using MiniHittegods.Domain.Entities;
using MiniHittegods.Domain.Enums;

namespace MiniHittegods.Tests.Fakes;

public class FakeFoundItemRepository : IFoundItemRepository
{
    private readonly List<FoundItem> _items = new();

    public Task AddAsync(FoundItem item)
    {
        _items.Add(item);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _items.RemoveAll(x => x.Id == id);
        return Task.CompletedTask;
    }

    public Task<List<FoundItem>> GetAllAsync(
        FoundItemStatus? status,
        string? category,
        string? q)
    {
        IEnumerable<FoundItem> query = _items;

        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(x => x.Category == category);
        }

        if (!string.IsNullOrWhiteSpace(q))
        {
            query = query.Where(x =>
                x.Title.Contains(q) ||
                (x.Description != null && x.Description.Contains(q)));
        }

        return Task.FromResult(query.ToList());
    }

    public Task<FoundItem?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_items.FirstOrDefault(x => x.Id == id));
    }

    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }
}