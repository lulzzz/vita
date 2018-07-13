namespace Vita.Contracts
{
  public class Address
  {
    public string UnitNumber { get; set; }
    public string StreetNumber { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string AddressLine4 { get; set; }
    public string Suburb { get; set; }
    public AustralianState? State { get; set; }
    public string PostCode { get; set; }
    public string CountryCode { get; set; }
  }
}