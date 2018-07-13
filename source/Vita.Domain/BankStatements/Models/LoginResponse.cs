using System.Xml.Serialization;

namespace Vita.Domain.BankStatements.Models
{
    [XmlRoot(ElementName = "xml")]
    public class LoginResponse
    {
        [XmlElement(ElementName = "accounts")]
        public Account[] Accounts { get; set; }
        [XmlElement(ElementName = "user_token")]
        public string UserToken { get; set; }
        [XmlElement(ElementName = "referral_code")]
        public string ReferralCode { get; set; }
    }
}
