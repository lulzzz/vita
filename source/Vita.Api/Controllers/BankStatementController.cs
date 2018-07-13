using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Vita.Contracts.ChargeId;
using Vita.Domain.BankStatements;
using Vita.Domain.BankStatements.Login;

namespace Vita.Api.Controllers
{
  [Produces("application/json")]
  [Route("[controller]")]
  public class BankStatementController : Controller
  {
    public IBankStatementService BankStatementService { get; }

    public BankStatementController(IBankStatementService bankStatementService)
    {
      BankStatementService = bankStatementService;
 
    }

    [HttpPost("login/{userIdentifier}")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(IEnumerable<SearchResponse>))]
    public async Task<IActionResult> Login(string userIdentifier, [FromBody]  BankLogin bankLogin)
    {
      var result = await BankStatementService.LoginAsync(userIdentifier, bankLogin);
      return Ok(result);
    }

    [HttpPost("loginfetchall/{userIdentifier}")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(IEnumerable<SearchResponse>))]
    public async Task<IActionResult> LoginFetchAllAsync(string userIdentifier, [FromBody]  BankLogin bankLogin)
    {
      var result = await BankStatementService.LoginFetchAllAsync(userIdentifier, bankLogin);
      return Ok(result);
    }
  }
}