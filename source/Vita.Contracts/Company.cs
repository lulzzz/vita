using System;
using Newtonsoft.Json;

namespace Vita.Contracts
{
  public class Company : Tracking
  {
    [JsonProperty(PropertyName = "Id")]
    public Guid Id { get; set; }
    public string CompanyName { get; set; }
    public string AustralianCompanyNumber { get; set; }
    public string CompanyType { get; set; }
    public string CompanyClass { get; set; }
    public string SubClass { get; set; }

      /*

DRGD - De-registered.
EXAD - External administration (in receivership/liquidation).
NOAC - Not active.
NRGD - Not registered.
PROV - Provisional (mentioned only under charges and refers
to those which have not been fully registered).
REGD – Registered.
SOFF - Strike-off action in progress.
DISS - Dissolved by Special Act of Parliament.
DIV3 - Organisation Transferred Registration via DIV3.
PEND - Pending - Schemes.

*/
    public string Status { get; set; }
    public string DateOfRegistration { get; set; }
    public string PreviousStateOfRegistration { get; set; }
    public string StateOfRegistrationNumber { get; set; }
    public string ModifiedSinceLastReport { get; set; }
    public string CurrentNameIndicator { get; set; }
    public string AustralianBusinessNumber { get; set; }
    public string CurrentName { get; set; }
    public string CurrentNameStartDate { get; set; }
    public string CompanyCurrentInd { get; set; }
    public string CompanyCurrentName { get; set; }
    public string CompanyCurrentNameStartDt { get; set; }
    public string CompanyModifiedSinceLast { get; set; }

    public override string ToString()
    {
      return
        $"{nameof(CompanyName)}: {CompanyName}, {nameof(AustralianCompanyNumber)}: {AustralianCompanyNumber}";
    }


  }
}