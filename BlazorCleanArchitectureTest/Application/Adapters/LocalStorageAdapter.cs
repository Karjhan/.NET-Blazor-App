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
        logger.LogInformation("Attempting to retrieve the token from browser local storage.");
        
        try
        {
            var token = await GetBrowserLocalStorage();
            var isTokenEmpty = string.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(token);
            if (isTokenEmpty)
            {
                logger.LogWarning("Token retrieved is empty or whitespace.");
                return new LocalStorageDTO();
            }

            logger.LogInformation("Token retrieved successfully, attempting to deserialize.");
            var result = JsonUtilities.DeserializeJsonString<LocalStorageDTO>(token);
            
            logger.LogInformation("Deserialization successful.");
            return result;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while getting the model from the token.");
            return new LocalStorageDTO();
        }
    }

    public async Task<Result> SetBrowserLocalStorage(LocalStorageDTO localStorageDto)
    {
        logger.LogInformation("Attempting to set browser local storage with provided DTO.");
        
        try
        {
            string token = JsonUtilities.SerializeObject(localStorageDto);
            await localStorageService.SaveAsEncryptedStringAsync(LocalStorageConstants.BrowserStorageKey, token);

            logger.LogInformation("Successfully set browser local storage.");
            return Result.Success();
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while setting browser local storage.");
            return Result.Failure(Error.SaveLocalStorageError);
        }
    }

    public async Task<Result> RemoveTokenFromBrowserLocalStorage()
    {
        try
        {
            await localStorageService.DeleteItemAsync(LocalStorageConstants.BrowserStorageKey);
            logger.LogInformation("Token successfully removed from browser local storage.");
            return Result.Success();
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while removing the token from browser local storage.");
            return Result.Failure(Error.DeleteFromLocalStorageError);
        }
    }
    
    private async Task<string> GetBrowserLocalStorage()
    {
        logger.LogInformation("Retrieving encrypted item as string from browser local storage.");
        var result = await localStorageService.GetEncryptedItemAsStringAsync(LocalStorageConstants.BrowserStorageKey);
        logger.LogInformation("Retrieved item from browser local storage.");
        return result;
    }
}