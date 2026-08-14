using Microsoft.EntityFrameworkCore;
using MiniHittegods.Api.Data;
using MiniHittegods.Application.Interfaces;
using MiniHittegods.Domain.Entities;
using MiniHittegods.Domain.Enums;

namespace MiniHittegods.Api.Repositories;

public class EfFoundItemRepository : IFoundItemRepository
{
    private readonly MiniHittegodsDbContext _context;

    public EfFoundItemRepository(MiniHittegodsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(FoundItem item)
    {
        await _context.FoundItems.AddAsync(item);
    }

    public async Task<FoundItem?> GetByIdAsync(Guid id)
    {
        return await _context.FoundItems
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<FoundItem>> GetAllAsync(
        FoundItemStatus? status,
        string? category,
        string? q)
    {
        var query = _context.FoundItems.AsQueryable();

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

        return await query.ToListAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var item = await GetByIdAsync(id);

        if (item != null)
        {
            _context.FoundItems.Remove(item);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}