namespace Vita.Contracts
{
    public class PredictionResult
    {
        public PredictionRequest Request { get; set; }
        public string PredictedValue { get; set; }
        public PredictionMethod Method { get; set; }
    }
}