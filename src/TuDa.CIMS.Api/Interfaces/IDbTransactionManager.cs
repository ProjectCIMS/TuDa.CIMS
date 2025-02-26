namespace TuDa.CIMS.Api.Interfaces;

public interface IDbTransactionManager
{
    public Task RunInTransactionAsync(Func<Task> action);
    public Task<T> RunInTransactionAsync<T>(Func<Task<T>> action);
    public Task<ErrorOr<T>> RunInTransactionAsync<T>(Func<Task<ErrorOr<T>>> action);
}
