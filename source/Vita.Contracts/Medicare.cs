namespace Vita.Contracts
{
    public class Medicare : ValueObject
    {
        public string Cardholder { get; }
        public string Number { get; }
        public string Reference { get; }
        public Colour Colour { get; }

        public Medicare(string cardholder, string number, string reference, Colour colour)
        {
            Cardholder = cardholder;
            Number = number;
            Reference = reference;
            Colour = colour;
        }
    }
}