namespace Vita.Contracts
{
    public class ForseableChange : ValueObject
    {
        public static ForseableChange Yes = new ForseableChange(true);

        public static ForseableChange No = new ForseableChange(false);

        public bool IsForseable { get; }

        public ForseableChange(bool isForseable)
        {
            IsForseable = isForseable;
        }
    }
}