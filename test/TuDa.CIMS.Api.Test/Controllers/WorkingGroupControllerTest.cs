using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TuDa.CIMS.Api.Controllers;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Test.Integration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Shared.Test.Extensions;
using TuDa.CIMS.Shared.Test.Faker;

namespace TuDa.CIMS.Api.Test.Controllers;

[TestSubject(typeof(WorkingGroupController))]
public class WorkingGroupControllerTest : IClassFixture<CIMSApiFactory>
{
    private readonly HttpClient _client;
    private readonly CIMSDbContext _dbContext;

    public WorkingGroupControllerTest(CIMSApiFactory apiFactory)
    {
        _client = apiFactory.CreateClient();

        var scope = apiFactory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<CIMSDbContext>();

        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.Migrate();
    }

    [Fact]
    public async Task GetAsync_ShouldReturnWorkingGroup_WhenPurchasePresent()
    {
        // Arrange
        WorkingGroup workingGroup = new WorkingGroupFaker();

        await _dbContext.WorkingGroups.AddRangeAsync(workingGroup);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"api/working-groups/{workingGroup.Id}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var result = await response.Content.FromJsonAsync<WorkingGroup>();

        result.Should().BeEquivalentTo(workingGroup);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNotFound_WhenWorkingGroupNotPresent()
    {
        var response = await _client.GetAsync($"api/working-groups/{Guid.NewGuid()}");

        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllWorkingGroups_WhenPurchasesArePresent()
    {
        // Arrange
        List<WorkingGroup> workingGroups = new WorkingGroupFaker().GenerateBetween(2, 10);

        await _dbContext.WorkingGroups.AddRangeAsync(workingGroups);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"api/working-groups");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var result = await response.Content.FromJsonAsync<List<WorkingGroup>>();

        result.Should().BeEquivalentTo(workingGroups);
    }

    [Fact]
    public async Task RemoveAsync_ShouldRemoveWorkingGroup_WhenWorkingGroupPresent()
    {
        // Arrange
        WorkingGroup workingGroup = new WorkingGroupFaker();

        await _dbContext.WorkingGroups.AddAsync(workingGroup);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.DeleteAsync($"api/working-groups/{workingGroup.Id}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var result = (await _dbContext.WorkingGroups.ToListAsync());

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task RemoveAsync_ShouldReturnNotFound_WhenWorkingGroupNotPresent()
    {
        var response = await _client.DeleteAsync($"api/working-groups/{Guid.NewGuid()}");
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateWorkingGroup_WhenWorkingGroupPresent()
    {
        // Arrange
        Professor professor = new PersonFaker<Professor>();
        string phoneNumber = "Phone";

        var createWorkingGroup = new CreateWorkingGroupDto
        {
            Professor = professor.ToCreateDto(),
            PhoneNumber = phoneNumber,
        };

        // Act
        var response = await _client.PostAsync(
            $"api/working-groups",
            JsonContent.Create(createWorkingGroup)
        );

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var createResult = await response.Content.FromJsonAsync<WorkingGroup>();

        var result = await _dbContext.WorkingGroups.Include(wg => wg.Professor).SingleAsync();
        result
            .Professor.Should()
            .BeEquivalentTo(
                professor,
                options => options.Excluding(s => s.Id).Excluding(s => s.Address.Id)
            );
        result.PhoneNumber.Should().BeEquivalentTo(phoneNumber);

        result.Should().BeEquivalentTo(createResult);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateWorkingGroup_WhenWorkingGroupIsUpdated()
    {
        // Arrange
        WorkingGroup workingGroup = new WorkingGroupFaker();
        await _dbContext.WorkingGroups.AddAsync(workingGroup);
        await _dbContext.SaveChangesAsync();

        const string updateProfName = "Changed";
        var updateWorkingGroupDto = new UpdateWorkingGroupDto
        {
            Professor = new() { Name = updateProfName },
        };
        var updatedWorkingGroup = workingGroup with
        {
            Professor = workingGroup.Professor with { Name = updateProfName },
        };

        // Act
        var response = await _client.PatchAsync(
            $"api/working-groups/{workingGroup.Id}",
            JsonContent.Create(updateWorkingGroupDto)
        );

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var responseWorkingGroup = await response.Content.FromJsonAsync<WorkingGroup>();

        workingGroup.Should().BeEquivalentTo(updatedWorkingGroup);

        workingGroup
            .Should()
            .BeEquivalentTo(
                responseWorkingGroup,
                options => options.Excluding(wg => wg!.Professor.UpdatedAt)
            );
        responseWorkingGroup!.Professor.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenWorkingGroupNotPresent()
    {
        var response = await _client.PatchAsync(
            $"api/working-groups/{Guid.NewGuid()}",
            JsonContent.Create<UpdateWorkingGroupDto>(new UpdateWorkingGroupDto())
        );
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
