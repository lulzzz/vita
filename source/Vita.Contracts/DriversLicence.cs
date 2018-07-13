namespace Vita.Contracts
{
    public class DriversLicence : ValueObject
    {
        public string State { get; set; }
        public string Number { get; set; }
        public string Card { get; set; }

        public DriversLicence(string state, string number, string card)
        {
            State = state;
            Number = number;
            Card = card;
        }
    }
}