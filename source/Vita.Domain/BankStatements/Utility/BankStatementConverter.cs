using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Vita.Domain.BankStatements.Models;

namespace Vita.Domain.BankStatements.Utility
{
    public class BankStatementConverter
    {
        public static Account Convert(string content)
        {
            var xml = new XmlDocument();
            xml.LoadXml(content);
            AddJsonNetRootAttribute(xml);
            AddJsonArrayAttributesForXPath("xml/accounts/account/statementData/details/detail/tags/tag", xml);
            AddJsonArrayAttributesForXPath("xml/accounts/account/statementData/analysis/*/*/transactions/transaction", xml);
            AddJsonArrayAttributesForXPath("xml/accounts/account/statementData/analysis/*/*/transactions/transaction/tags/tag", xml);
            var json = JsonConvert.SerializeXmlNode(xml.ChildNodes[1]);
            var jobj = JObject.Parse(json);
            return jobj["xml"].ToObject<Account>();
        }

        private static void AddJsonNetRootAttribute(XmlDocument xml)
        {
            var jsonNS = xml.CreateAttribute("xmlns", "json", "http://www.w3.org/2000/xmlns/");
            jsonNS.Value = "http://james.newtonking.com/projects/json";
            xml.DocumentElement.SetAttributeNode(jsonNS);
        }

        private static void AddJsonArrayAttributesForXPath(string xpath, XmlDocument doc)
        {
            var elements = doc.SelectNodes(xpath);
            foreach (var element in elements)
            {
                var el = element as XmlElement;
                if (el != null)
                {

                    var jsonArray = doc.CreateAttribute("json", "Array", "http://james.newtonking.com/projects/json");
                    jsonArray.Value = "true";
                    el.SetAttributeNode(jsonArray);
                }
            }
        }
    }
}
