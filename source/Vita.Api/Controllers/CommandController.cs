using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using EventFlow.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Vita.Api.Controllers
{
    [Route("api/[controller]")]
    public class CommandController : Controller
    {
        private readonly ISerializedCommandPublisher _serializedCommandPublisher;
      
        public CommandController(ISerializedCommandPublisher serializedCommandPublisher)
        {
            _serializedCommandPublisher = serializedCommandPublisher;
        
        }

        [HttpPost("{name}")]
        public async Task<IActionResult> PublishCommandAsync([FromBody]dynamic commandJson, string name)
        {
            const int version = 1;
           // if (!IsAuthorizedToPublish(name)) LoginManager.ReportSessionError("Invalid or expired login.");

          //  var json = JsonConvert.SerializeObject(commandJson);
            ISourceId sourceId =
                await
                    _serializedCommandPublisher.PublishSerilizedCommandAsync(name, version, commandJson,
                        CancellationToken.None).ConfigureAwait(false);
            return Ok(new { SourceId = sourceId.Value });
        }

        private bool IsAuthorizedToPublish(string name)
        {
            return true;
        }
    }
}