using Microsoft.AspNetCore.Mvc;
using MiniHittegods.Api.DTOs;
using MiniHittegods.Application.Services;
using MiniHittegods.Domain.Entities;
using MiniHittegods.Domain.Enums;

namespace MiniHittegods.Api.Controllers;

[ApiController]
[Route("api/items")]
public class ItemsController : ControllerBase
{
    private readonly FoundItemsService _service;

    public ItemsController(FoundItemsService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateFoundItemRequest request)
    {
        var item = new FoundItem(
            request.Title,
            request.Description,
            request.Category,
            request.FoundLocation
        );

        await _service.CreateAsync(item);

        return CreatedAtAction(
            nameof(GetById),
            new { id = item.Id },
            item
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        FoundItemStatus? status,
        string? category,
        string? q)
    {
        var items = await _service.GetAllAsync(status, category, q);

        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var item = await _service.GetByIdAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [HttpPost("{id}/claim")]
    public async Task<IActionResult> Claim(Guid id, ClaimItemRequest request)
    {
        var item = await _service.GetByIdAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        try
        {
            var result = await _service.ClaimAsync(id, request.ClaimedBy);

            return Ok(result);
        }
        catch (InvalidOperationException)
        {
            return Conflict();
        }
    }

    [HttpPost("{id}/return")]
    public async Task<IActionResult> Return(Guid id)
    {
        var item = await _service.GetByIdAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        try
        {
            var result = await _service.ReturnAsync(id);

            return Ok(result);
        }
        catch (InvalidOperationException)
        {
            return Conflict();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var item = await _service.GetByIdAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        if (!item.CanBeDeleted())
        {
            return Conflict();
        }

        await _service.DeleteAsync(id);

        return NoContent();
    }
}