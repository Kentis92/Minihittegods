using MiniHittegods.Application.Services;
using MiniHittegods.Domain.Entities;
using MiniHittegods.Domain.Enums;
using MiniHittegods.Tests.Fakes;

namespace MiniHittegods.Tests.Application;

public class FoundItemsServiceTests
{
    [Fact]
    public void Service_can_be_created()
    {
        var repository = new FakeFoundItemRepository();

        var service = new FoundItemsService(repository);

        Assert.NotNull(service);
    }


    [Fact]
    public async Task Create_item_should_add_item_to_repository()
    {
        var repository = new FakeFoundItemRepository();
        var service = new FoundItemsService(repository);

        var item = new FoundItem(
            "Blå jakke",
            "Norge skrevet på ryggen",
            "Clothing",
            "Scene 2");

        await service.CreateAsync(item);

        var items = await repository.GetAllAsync(null, null, null);

        Assert.Single(items);
    }


    [Fact]
    public async Task Get_item_by_id_should_return_item()
    {
        var repository = new FakeFoundItemRepository();
        var service = new FoundItemsService(repository);

        var item = new FoundItem(
            "Svart lommebok",
            "Fant ved inngang",
            "Wallet",
            "Inngang A");

        await repository.AddAsync(item);

        var result = await service.GetByIdAsync(item.Id);

        Assert.NotNull(result);
        Assert.Equal("Svart lommebok", result.Title);
    }


    [Fact]
    public async Task Get_all_items_should_return_all_items()
    {
        var repository = new FakeFoundItemRepository();
        var service = new FoundItemsService(repository);

        var item1 = new FoundItem(
            "Blå jakke",
            "Norge skrevet på ryggen",
            "Clothing",
            "Scene 2");

        var item2 = new FoundItem(
            "Svart lommebok",
            "Fant ved inngangen",
            "Wallet",
            "Inngang A");

        await repository.AddAsync(item1);
        await repository.AddAsync(item2);

        var result = await service.GetAllAsync(null, null, null);

        Assert.Equal(2, result.Count());
    }


[Fact]
public async Task Claim_item_should_change_status_to_claimed()
{
    var repository = new FakeFoundItemRepository();
    var Service = new FoundItemsService(repository);

    var item = new FoundItem(
        "Blå jakke",
        "Norge skrevet på ryggen",
        "Clothing",
        "Scene 2");

    await repository.AddAsync(item);

    var result = await Service.ClaimAsync(
        item.Id,
        "Ola Nordmann");

    Assert.Equal(FoundItemStatus.Claimed, result.Status);
    Assert.Equal("Ola Nordmann", result.ClaimedBy);
}

[Fact]
public async Task Delete_available_item_should_remove_it()
    {
        var repository = new FakeFoundItemRepository();
        var Service = new FoundItemsService(repository);

        var item = new FoundItem(
            "Blå jakke",
            "Norge skrevet på ryggen",
            "Clothing",
            "Scene 2");

        await repository.AddAsync(item);

        await Service.DeleteAsync(item.Id);

        var result = await repository.GetByIdAsync(item.Id);

        Assert.Null(result);
    }
[Fact]
public async Task Delete_claimed_item_throw_exception()
    {
        var repository = new FakeFoundItemRepository();
        var Service = new FoundItemsService(repository);

        var item = new FoundItem(
            "Blå jakke",
            "Norge skrevet på ryggen",
            "Clothing",
            "Scene 2");

            await repository.AddAsync(item);

            item.Claim("Ola Nordmann");

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await Service.DeleteAsync(item.Id));
    }
[Fact]
public async Task Return_item_should_change_status_to_returned()
    {
        var repository = new FakeFoundItemRepository();
        var service = new FoundItemsService(repository);

        var item = new FoundItem(
            "Mobil",
            "iPhone",
            "Phone",
            "Scene 1");

            await repository.AddAsync(item);

            await service.ClaimAsync(item.Id, "Ola Nordmann");

            var result = await service.ReturnAsync(item.Id);

            Assert.Equal(FoundItemStatus.Returned, result.Status);
            Assert.NotNull(result.ReturnedAtUtc);

    }
}