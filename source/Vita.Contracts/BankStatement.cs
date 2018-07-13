namespace Vita.Contracts
{
    public class BankStatement : ValueObject
    {
        public string Token { get; set; }
        public string Xml { get; set; }
        public byte[] Binary { get; set; }

        public BankStatement(string token, string xml, byte[] binary)
        {
            Token = token;
            Xml = xml;
            Binary = binary;
        }
    }
}