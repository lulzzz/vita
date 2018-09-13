using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Vita.Api.Controllers
{
    [Route("api/[controller]")]
    public class QueryController : Controller
    {
        private readonly ISerializedQueryProcessor _serializedQueryProcessor;
     
        public QueryController(ISerializedQueryProcessor serializedQueryProcessor)
        {
            _serializedQueryProcessor = serializedQueryProcessor;
        
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> ProcessQueryAsync(string name)
        {
        //    if (!IsAuthorizedToQuery(name)) LoginManager.ReportSessionError("Invalid or expired login.");

            //var queryJson = _request.GetQueryStringParamsAsJson();
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