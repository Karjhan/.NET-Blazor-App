using Application.Responses.Credentials;
using Domain.Primitives;

namespace Application.Adapters;

public interface ILocalStorageAdapter
{
    Task<Result<LocalStorageDTO>> GetModelFromToken();

    Task<Result> SetBrowserLocalStorage(LocalStorageDTO localStorageDto);
    
    Task<Result> RemoveTokenFromBrowserLocalStorage();
}