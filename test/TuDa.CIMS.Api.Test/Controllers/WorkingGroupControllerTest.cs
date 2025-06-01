using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Controllers;
using TuDa.CIMS.Api.Test.Integration;
using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Shared.Dtos.Update;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Extensions;
using TuDa.CIMS.Shared.Test.Extensions;
using TuDa.CIMS.Shared.Test.Faker;

namespace TuDa.CIMS.Api.Test.Controllers;

[TestSubject(typeof(WorkingGroupController))]
public class WorkingGroupControllerTest(CIMSApiFactory apiFactory) : ControllerTestBase(apiFactory)
{
    [Fact]
    public async Task GetAsync_ShouldReturnWorkingGroup_WhenPurchasePresent()
    {
        // Arrange
        WorkingGroup workingGroup = new WorkingGroupFaker();

        await DbContext.WorkingGroups.AddRangeAsync(workingGroup);
        await DbContext.SaveChangesAsync();

        // Act
        var response = await Client.GetAsync($"api/working-groups/{workingGroup.Id}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var result = await response.Content.FromJsonAsync<WorkingGroupResponseDto>();

        result.Should().BeEquivalentTo(workingGroup.ToResponseDto());
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNotFound_WhenWorkingGroupNotPresent()
    {
        var response = await Client.GetAsync($"api/working-groups/{Guid.NewGuid()}");

        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllWorkingGroups_WhenPurchasesArePresent()
    {
        // Arrange
        List<WorkingGroup> workingGroups = new WorkingGroupFaker().GenerateBetween(2, 10);

        await DbContext.WorkingGroups.AddRangeAsync(workingGroups);
        await DbContext.SaveChangesAsync();

        // Act
        var response = await Client.GetAsync($"api/working-groups");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var result = await response.Content.FromJsonAsync<List<WorkingGroupResponseDto>>();

        result.Should().BeEquivalentTo(workingGroups.ToResponseDtos());
    }

    [Fact]
    public async Task RemoveAsync_ShouldRemoveWorkingGroup_WhenWorkingGroupPresent()
    {
        // Arrange
        WorkingGroup workingGroup = new WorkingGroupFaker();

        await DbContext.WorkingGroups.AddAsync(workingGroup);
        await DbContext.SaveChangesAsync();

        // Act
        var response = await Client.DeleteAsync($"api/working-groups/{workingGroup.Id}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var result = (await DbContext.WorkingGroups.ToListAsync());

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task RemoveAsync_ShouldReturnNotFound_WhenWorkingGroupNotPresent()
    {
        var response = await Client.DeleteAsync($"api/working-groups/{Guid.NewGuid()}");
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
        var response = await Client.PostAsync(
            $"api/working-groups",
            JsonContent.Create(createWorkingGroup)
        );

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var createResult = await response.Content.FromJsonAsync<WorkingGroupResponseDto>();

        var result = await DbContext.WorkingGroups.Include(wg => wg.Professor).SingleAsync();
        result
            .ToResponseDto()
            .Professor.Should()
            .BeEquivalentTo(
                professor,
                options => options.Excluding(s => s.Id).Excluding(s => s.Address.Id)
            );
        result.PhoneNumber.Should().BeEquivalentTo(phoneNumber);

        result.ToResponseDto().Should().BeEquivalentTo(createResult);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateWorkingGroup_WhenWorkingGroupIsUpdated()
    {
        // Arrange
        WorkingGroup workingGroup = new WorkingGroupFaker();
        await DbContext.WorkingGroups.AddAsync(workingGroup);
        await DbContext.SaveChangesAsync();

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
        var response = await Client.PatchAsync(
            $"api/working-groups/{workingGroup.Id}",
            JsonContent.Create(updateWorkingGroupDto)
        );

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var responseWorkingGroup = await response.Content.FromJsonAsync<WorkingGroupResponseDto>();

        workingGroup.Should().BeEquivalentTo(updatedWorkingGroup);

        workingGroup.ToResponseDto().Should().BeEquivalentTo(responseWorkingGroup);
        responseWorkingGroup!.Professor.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenWorkingGroupNotPresent()
    {
        var response = await Client.PatchAsync(
            $"api/working-groups/{Guid.NewGuid()}",
            JsonContent.Create<UpdateWorkingGroupDto>(new UpdateWorkingGroupDto())
        );
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
