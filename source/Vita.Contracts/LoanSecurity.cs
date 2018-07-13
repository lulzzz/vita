namespace Vita.Contracts
{
    public class LoanSecurity : ValueObject
    {
        public static LoanSecurity None = new LoanSecurity(Vehicle.None);

        public Vehicle Vehicle { get; }

        public LoanSecurity(Vehicle vehicle)
        {
            Vehicle = vehicle;
        }
    }
}