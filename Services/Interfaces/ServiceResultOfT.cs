namespace YmmoApi.Services.Interfaces;

public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; init; }

    public static ServiceResult<T> Success(T data) => new() { Succeeded = true, Data = data };

    public static new ServiceResult<T> Failure(string error) => new() { Succeeded = false, Error = error };

    public static new ServiceResult<T> Forbidden(string error) => new() { Succeeded = false, IsForbidden = true, Error = error };
}
