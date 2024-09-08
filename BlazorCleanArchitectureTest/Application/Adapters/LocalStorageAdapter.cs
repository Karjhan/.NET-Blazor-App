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
            var token = await GetBrowserLocalStorage(LocalStorageConstants.BrowserStorageKey);
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
    
    public async Task<Result<string>> GetCodeVerifier()
    {
        logger.LogInformation("Attempting to retrieve the code verifier from browser local storage.");
        
        try
        {
            var codeVerifier = await GetBrowserLocalStorage(LocalStorageConstants.CodeVerifierKey);
            var isCodeVerifierEmpty = string.IsNullOrEmpty(codeVerifier) || string.IsNullOrWhiteSpace(codeVerifier);
            if (isCodeVerifierEmpty)
            {
                logger.LogWarning("Code verifier retrieved is empty or whitespace.");
                return string.Empty;
            }

            logger.LogInformation("Code verifier retrieved successfully.");
            return codeVerifier;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while getting the code verifier.");
            return string.Empty;
        }
    }
    
    public async Task<Result> SetCodeVerifierInLocalStorage(string codeVerifier)
    {
        logger.LogInformation("Attempting to set code verifier in local storage.");
        
        try
        {
            await localStorageService.SaveAsEncryptedStringAsync(LocalStorageConstants.CodeVerifierKey, codeVerifier);

            logger.LogInformation("Successfully set code verifier in browser local storage.");
            return Result.Success();
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while setting code verifier in browser local storage.");
            return Result.Failure(Error.SaveLocalStorageError);
        }
    }

    public async Task<Result> RemoveCodeVerifierFromBrowserLocalStorage()
    {
        try
        {
            await localStorageService.DeleteItemAsync(LocalStorageConstants.CodeVerifierKey);
            logger.LogInformation("Code verifier successfully removed from browser local storage.");
            return Result.Success();
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while removing the code verifier from browser local storage.");
            return Result.Failure(Error.DeleteFromLocalStorageError);
        }
    }
    
    private async Task<string> GetBrowserLocalStorage(string key)
    {
        logger.LogInformation("Retrieving encrypted item with key {key} as string from browser local storage.", key);
        var result = await localStorageService.GetEncryptedItemAsStringAsync(key);
        logger.LogInformation("Retrieved item from browser local storage.");
        return result;
    }
}