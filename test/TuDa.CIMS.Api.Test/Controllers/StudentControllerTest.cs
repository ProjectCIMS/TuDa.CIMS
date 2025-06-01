using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Controllers;
using TuDa.CIMS.Api.Test.Integration;
using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Dtos.Update;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Test.Faker;

namespace TuDa.CIMS.Api.Test.Controllers;

[TestSubject(typeof(StudentController))]
public class StudentControllerTest(CIMSApiFactory apiFactory) : ControllerTestBase(apiFactory)
{
    [Fact]
    public async Task RemoveAsync_ShouldRemoveStudent_WhenStudentPresent()
    {
        // Arrange
        List<Student> students = [new StudentFaker(), new StudentFaker()];
        WorkingGroup workingGroup = new WorkingGroupFaker().Generate();
        workingGroup.Students = students;
        await DbContext.WorkingGroups.AddAsync(workingGroup);
        await DbContext.Students.AddRangeAsync(students);
        await DbContext.SaveChangesAsync();

        var studentsToRemove = students.ToList();

        foreach (var student in studentsToRemove)
        {
            // Act
            var response = await Client.DeleteAsync(
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
        var response = await Client.DeleteAsync(
            $"/api/working-groups/{workingGroup.Id}/students/{Guid.NewGuid()}"
        );
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

        await DbContext.WorkingGroups.AddAsync(workingGroup);
        await DbContext.Students.AddRangeAsync(students);
        await DbContext.SaveChangesAsync();

        const string updatedName = "Updated Student Name";
        List<UpdateStudentDto> updateStudentDtos =
            new()
            {
                new UpdateStudentDto { Name = updatedName },
                new UpdateStudentDto { Name = updatedName },
            };

        // Act
        foreach (var (student, updateStudentDto) in students.Zip(updateStudentDtos))
        {
            var response = await Client.PatchAsync(
                $"/api/working-groups/{workingGroup.Id}/students/{student.Id}",
                JsonContent.Create(updateStudentDto)
            );

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var result = await DbContext.Students.SingleAsync(s => s.Id == student.Id);
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
        var response = await Client.PatchAsync(
            $"/api/working-groups/{workingGroup.Id}/students/{Guid.NewGuid()}",
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

        await DbContext.WorkingGroups.AddRangeAsync(workingGroup);
        await DbContext.SaveChangesAsync();

        var createStudentDto = new CreateStudentDto
        {
            Name = student.Name,
            FirstName = student.FirstName,
            Gender = student.Gender,
            PhoneNumber = student.PhoneNumber,
        };

        // Act
        var response = await Client.PostAsync(
            $"api/working-groups/{workingGroup.Id}/students",
            JsonContent.Create(createStudentDto)
        );

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var createResult = await response.Content.FromJsonAsync<Student>();

        var result = await DbContext.Students.SingleAsync(s => s.Id == createResult!.Id);

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
        var response = await Client.PostAsync(
            $"/api/working-groups/{Guid.NewGuid()}/students",
            JsonContent.Create(new CreateStudentDto() { Name = "New Student" })
        );
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
