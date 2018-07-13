namespace Vita.Contracts
{
    public class EmailAddress : ValueObject
    {
        public string Address { get; set; }

        public EmailAddress(string address)
        {
            Address = address;
        }
    }
}