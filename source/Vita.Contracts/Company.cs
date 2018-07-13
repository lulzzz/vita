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