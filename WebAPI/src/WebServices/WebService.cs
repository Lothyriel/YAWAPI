namespace WebAPI.WebServices
{
    public abstract class WebService
    {
        protected static HttpClient HttpClient { get; } = new();
    }
}