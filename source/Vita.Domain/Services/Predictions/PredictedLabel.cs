using Microsoft.ML.Runtime.Api;

namespace Vita.Domain.Services.Predictions
{
    public class PredictedLabel
    {
      [ColumnName("PredictedLabel")]
      public string SubCategory;
    }
}
