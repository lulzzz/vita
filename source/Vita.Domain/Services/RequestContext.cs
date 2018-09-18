using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Vita.Domain.Services
{
    public class RequestContext  : IRequestContext
    {
        private readonly IHttpContextAccessor _http;

        public RequestContext(IHttpContextAccessor http)
        {
            _http = http;
        }

        public string GetIpAddress()
        {
            var ip = _http?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            return ip ?? "unknown";
        }

        public string GetLoginId()
        {
            var loginId = _http.HttpContext?.Request?.Headers["login-id"];
            if (string.IsNullOrEmpty(loginId.ToString())) loginId = _http.HttpContext?.Request?.Query["loginId"];
            if (string.IsNullOrEmpty(loginId.ToString())) loginId = _http.HttpContext?.Request?.Cookies["loginId"];
            return loginId;
        }

        public string GetQueryStringParamsAsJson()
        {
            var query = _http.HttpContext?.Request?.Query;
            var dict = query?.Keys.ToDictionary<string, string, string>(key => key, key => query[key]);
            var queryJson = JsonConvert.SerializeObject(dict);
            return queryJson;
        }
    }
}