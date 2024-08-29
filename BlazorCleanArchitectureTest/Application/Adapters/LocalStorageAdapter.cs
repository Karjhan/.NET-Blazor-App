using Application.Responses.Credentials;
using Application.Utilities;
using Domain.Exceptions;
using Domain.Primitives;
using Infrastructure.Constants;
using Microsoft.Extensions.Logging;
using NetcodeHub.Packages.Extensions.LocalStorage;

namespace Application.Adapters;

public class LocalStorageAdapter(
    ILocalStorageService localStorageService,
    ILogger<LocalStorageAdapter> logger
) : ILocalStorageAdapter
{
    public async Task<Result<LocalStorageDTO>> GetModelFromToken()
    {
        try
        {
            var token = await GetBrowserLocalStorage();
            var isTokenEmpty = string.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(token);
            if (isTokenEmpty)
            {
                return new LocalStorageDTO();
            }

            return JsonUtilities.DeserializeJsonString<LocalStorageDTO>(token);
        }
        catch (Exception e)
        {
            return new LocalStorageDTO();
        }
    }

    public async Task<Result> SetBrowserLocalStorage(LocalStorageDTO localStorageDto)
    {
        try
        {
            string token = JsonUtilities.SerializeObject(localStorageDto);
            await localStorageService.SaveAsEncryptedStringAsync(LocalStorageConstants.BrowserStorageKey, token);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(Error.SaveLocalStorageError);
        }
    }

    public async Task<Result> RemoveTokenFromBrowserLocalStorage()
    {
        try
        {
            await localStorageService.DeleteItemAsync(LocalStorageConstants.BrowserStorageKey);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(Error.DeleteFromLocalStorageError);
        }
    }
    
    private async Task<string> GetBrowserLocalStorage()
    {
        var result = await localStorageService.GetEncryptedItemAsStringAsync(LocalStorageConstants.BrowserStorageKey);
        return result;
    }
}