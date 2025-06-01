using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Dtos.Update;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IProfessorRepository
{
    public Task<Professor?> GetOneAsync(Guid id);

    public Task<ErrorOr<Updated>> UpdateAsync(Guid id, UpdateProfessorDto update);

    public Task<ErrorOr<Professor>> CreateAsync(CreateProfessorDto create);
}
