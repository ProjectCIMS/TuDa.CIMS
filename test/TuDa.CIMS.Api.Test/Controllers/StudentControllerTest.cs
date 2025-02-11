using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TuDa.CIMS.Api.Controllers;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Test.Integration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Test.Faker;

namespace TuDa.CIMS.Api.Test.Controllers;

[TestSubject(typeof(StudentController))]
public class StudentControllerTest : IClassFixture<CIMSApiFactory>
{
    private readonly HttpClient _client;
    private readonly CIMSDbContext _dbContext;

    public StudentControllerTest(CIMSApiFactory apiFactory)
    {
        _client = apiFactory.CreateClient();

        var scope = apiFactory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<CIMSDbContext>();

        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.Migrate();
    }

    [Fact]
    public async Task RemoveAsync_ShouldRemoveStudent_WhenStudentPresent()
    {
        // Arrange
        List<Student> students = [new StudentFaker(), new StudentFaker()];
        WorkingGroup workingGroup = new WorkingGroupFaker().Generate();
        workingGroup.Students = students;
        await _dbContext.WorkingGroups.AddAsync(workingGroup);
        await _dbContext.Students.AddRangeAsync(students);
        await _dbContext.SaveChangesAsync();

        var studentsToRemove = students.ToList();

        foreach (var student in studentsToRemove)
        {
            // Act
            var response = await _client.DeleteAsync(
                $"/api/working-groups/{workingGroup.Id}/students/{student.Id}"
            );

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
        }

        workingGroup.Students.Should().BeEmpty();
    }

    [Fact]
    public async Task RemoveAsync_ShouldReturnNotFound_WhenStudentNotPresent()
    {
        WorkingGroup workingGroup = new WorkingGroupFaker().Generate();
        var response = await _client.DeleteAsync($"/api/working-groups/{workingGroup.Id}/students/{new Guid()}");
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateStudent_WhenStudentPresent()
    {
        // Arrange
        List<Student> students = new List<Student> { new StudentFaker(), new StudentFaker() };
        var workingGroup = new WorkingGroupFaker().Generate();
        workingGroup.Students = students;

        await _dbContext.WorkingGroups.AddAsync(workingGroup);
        await _dbContext.Students.AddRangeAsync(students);
        await _dbContext.SaveChangesAsync();

        const string updatedName = "Updated Student Name";
        List<UpdateStudentDto> updateStudentDtos = new ()
        {
            new UpdateStudentDto { Name = updatedName },
            new UpdateStudentDto { Name = updatedName }
        };

        // Act
        foreach (var (student, updateStudentDto) in students.Zip(updateStudentDtos))
        {
            var response = await _client.PatchAsync(
                $"/api/working-groups/{workingGroup.Id}/students/{student.Id}",
                JsonContent.Create(updateStudentDto)
            );

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var result = await _dbContext.Students.SingleAsync(s => s.Id == student.Id);
            result.Name.Should().Be(updatedName);
            result.UpdatedAt.Should().NotBeNull();
        }

        workingGroup.Students.Should().HaveCount(2);
        workingGroup.Students.All(s => s.Name == updatedName).Should().BeTrue();
    }


    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenStudentNotPresent()
    {
        var workingGroup = new WorkingGroupFaker().Generate();
        var response = await _client.PatchAsync(
            $"/api/working-groups/{workingGroup.Id}/students/{new Guid()}",
            JsonContent.Create(new UpdateStudentDto())
        );
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddAsync_ShouldAddStudent_WhenWorkingGroupPresent()
    {
        // Arrange
        var workingGroup = new WorkingGroupFaker().Generate();
        var student = new StudentFaker().Generate();

        await _dbContext.WorkingGroups.AddRangeAsync(workingGroup);
        await _dbContext.SaveChangesAsync();

        var createStudentDto = new CreateStudentDto
        {
            Name = student.Name,
            FirstName = student.FirstName,
            Gender = student.Gender,
            PhoneNumber = student.PhoneNumber,
        };

        // Act
        var response = await _client.PostAsync(
            $"api/working-groups/{workingGroup.Id}/students",
            JsonContent.Create(createStudentDto)
        );

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var createResult = await response.Content.FromJsonAsync<Student>();

        var result = await _dbContext.Students.SingleAsync(s => s.Id == createResult.Id);

        result.Should().NotBeNull();
        result.Name.Should().Be(student.Name);
        result.FirstName.Should().Be(student.FirstName);
        result.Gender.Should().Be(student.Gender);
        result.PhoneNumber.Should().Be(student.PhoneNumber);

        result.Should().BeEquivalentTo(createResult);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnNotFound_WhenWorkingGroupNotPresent()
    {
        var response = await _client.PostAsync(
            $"/api/working-groups/{new Guid()}/students",
            JsonContent.Create(new CreateStudentDto() { Name = "New Student" })
        );
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
