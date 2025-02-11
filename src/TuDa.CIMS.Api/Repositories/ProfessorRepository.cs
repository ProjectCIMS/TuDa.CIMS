using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Repositories;

[ScopedService]
public class ProfessorRepository : IProfessorRepository
{
    private readonly CIMSDbContext _context;

    public ProfessorRepository(CIMSDbContext context)
    {
        _context = context;
    }

    private IQueryable<Professor> ProfessorsFilledQuery =>
        _context.Professors.Include(p => p.Address);

    public Task<Professor?> GetOneAsync(Guid id) =>
        ProfessorsFilledQuery.SingleOrDefaultAsync(p => p.Id == id);

    public async Task<ErrorOr<Updated>> UpdateAsync(Guid id, UpdateProfessorDto update)
    {
        var professor = await GetOneAsync(id);

        if (professor is null)
        {
            return Error.NotFound(
                $"{nameof(ProfessorRepository)}.{nameof(UpdateAsync)}",
                $"Professor with id {id} could not be found."
            );
        }

        professor.Name = update.Name ?? professor.Name;
        professor.FirstName = update.FirstName ?? professor.FirstName;
        professor.Title = update.Title ?? professor.Title;
        professor.Gender = update.Gender ?? professor.Gender;
        professor.PhoneNumber = update.PhoneNumber ?? professor.PhoneNumber;
        professor.Address.City = update.AddressCity ?? professor.Address.City;
        professor.Address.Street = update.AddressStreet ?? professor.Address.Street;
        professor.Address.Number = update.AddressNumber ?? professor.Address.Number;
        professor.Address.ZipCode = update.AddressZipCode ?? professor.Address.ZipCode;


        await _context.SaveChangesAsync();

        return Result.Updated;
    }

    public async Task<ErrorOr<Professor>> CreateAsync(CreateProfessorDto create)
    {
        var professor = new Professor
        {
            Name = create.Name,
            FirstName = create.FirstName,
            PhoneNumber = create.PhoneNumber,
            Gender = create.Gender,
            Title = create.Title,
            Address = new Address
            {
                Street = create.AddressStreet,
                Number = create.AddressNumber,
                ZipCode = create.AddressZipCode,
                City = create.AddressCity,
            },
        };

        try
        {
            await _context.Professors.AddAsync(professor);
            await _context.SaveChangesAsync();

            return professor;
        }
        catch (Exception e)
        {
            return Error.Failure(e.GetType().Name, e.Message);
        }
    }
}
