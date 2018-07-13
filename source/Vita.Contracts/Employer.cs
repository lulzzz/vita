namespace Vita.Contracts
{
    public class Employer : ValueObject
    {
        public string Contact { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; }

        public Employer(string contact, string name, string contactNumber)
        {
            Contact = contact;
            Name = name;
            ContactNumber = contactNumber;
        }
    }
}