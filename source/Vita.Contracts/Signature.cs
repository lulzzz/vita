using System;

namespace Vita.Contracts
{
    public class Signature : ValueObject
    {
        public Name Name { get; set; }
        public DateTime Dob { get; set; }
        public string Code { get; set; }

        public Signature(Name name, DateTime dob, string code)
        {
            Name = name;
            Dob = dob;
            Code = code;
        }
    }
}