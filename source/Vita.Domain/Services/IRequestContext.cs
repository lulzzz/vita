namespace Vita.Domain.Services
{
    public interface IRequestContext
    {
        string GetIpAddress();
        string GetLoginId();
        string GetQueryStringParamsAsJson();
    }
}
