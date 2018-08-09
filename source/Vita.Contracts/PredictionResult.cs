namespace Vita.Contracts
{
    public class PredictionResult
    {
        public PredictionRequest Request = new PredictionRequest();
        public string PredictedValue { get; set; }
    }
}