namespace Vita.Contracts
{
    public class Name : ValueObject
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public Name(string title, string firstName, string middleName, string lastName)
        {
            Guard.AgainstEmpty(title, nameof(title));
            Guard.AgainstEmpty(firstName, nameof(firstName));
            Guard.AgainstEmpty(lastName, nameof(lastName));
            Title = title;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
        }
    }
}