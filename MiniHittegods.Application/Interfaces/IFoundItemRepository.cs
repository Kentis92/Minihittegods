using MiniHittegods.Domain.Entities;
using MiniHittegods.Domain.Enums;

namespace MiniHittegods.Application.Interfaces;

public interface IFoundItemRepository
{
    Task<FoundItem?> GetByIdAsync(Guid id);

    Task<List<FoundItem>> GetAllAsync(
        FoundItemStatus? status,
        string? category,
        string? q);

    Task AddAsync(FoundItem item);

    Task DeleteAsync(Guid id);

    Task SaveChangesAsync();
}