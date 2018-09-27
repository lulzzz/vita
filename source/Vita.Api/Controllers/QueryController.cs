using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vita.Domain.Infrastructure.EventFlow;
using Vita.Domain.Services;

namespace Vita.Api.Controllers
{
    [Route("api/[controller]")]
    public class QueryController : Controller
    {
        private readonly ISerializedQueryProcessor _serializedQueryProcessor;
        private readonly IRequestContext _requestContext;

        public QueryController(ISerializedQueryProcessor serializedQueryProcessor, IRequestContext requestContext)
        {
            _serializedQueryProcessor = serializedQueryProcessor;
            _requestContext = requestContext;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> ProcessQueryAsync(string name)
        {
        //    if (!IsAuthorizedToQuery(name)) LoginManager.ReportSessionError("Invalid or expired login.");

            var queryJson = _requestContext.GetQueryStringParamsAsJson();
            //var result = await Try.Retry(() => _serializedQueryProcessor.ProcessSerilizedQueryAsync(name, queryJson, CancellationToken.None)).ConfigureAwait(false);
            await Task.CompletedTask;
            return Ok("");
        }

        private bool IsAuthorizedToQuery(string name)
        {
            return true;
        }
    }
}