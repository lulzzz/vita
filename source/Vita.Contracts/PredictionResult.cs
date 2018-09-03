namespace Vita.Contracts
{
    public class PredictionResult
    {
        public PredictionRequest Request = PredictionRequest.New();
        public string PredictedValue { get; set; }
        public PredictionMethod Method { get; set; }
    }
}