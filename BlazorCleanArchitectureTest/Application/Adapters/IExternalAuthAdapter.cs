namespace Application.Adapters;

public interface IExternalAuthAdapter
{
    HttpClient GetAuthClient();
}