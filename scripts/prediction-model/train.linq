<Query Kind="Program">
  <Connection>
    <ID>c5e63f91-715c-4cdc-b87c-cc24ace9a884</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Vita</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Output>DataGrids</Output>
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.Tasks.dll</Reference>
  <Reference Relative="..\..\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Api.dll">C:\dev\vita\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Api.dll</Reference>
  <Reference Relative="..\..\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Contracts.dll">C:\dev\vita\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Contracts.dll</Reference>
  <Reference Relative="..\..\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Domain.dll">C:\dev\vita\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Domain.dll</Reference>
  <Reference Relative="..\..\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Predictor.dll">C:\dev\vita\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Predictor.dll</Reference>
  <NuGetReference>ExtensionMinder</NuGetReference>
  <NuGetReference>Microsoft.ML</NuGetReference>
  <NuGetReference>Microsoft.ML.CpuMath</NuGetReference>
  <NuGetReference>Microsoft.ML.HalLearners</NuGetReference>
  <NuGetReference>Microsoft.ML.ImageAnalytics</NuGetReference>
  <NuGetReference>Microsoft.ML.LightGBM</NuGetReference>
  <NuGetReference>Microsoft.ML.Onnx</NuGetReference>
  <NuGetReference>Microsoft.ML.Scoring</NuGetReference>
  <NuGetReference>Microsoft.ML.TensorFlow</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <NuGetReference>Scikit.ML.DataFrame</NuGetReference>
  <Namespace>ExtensionMinder</Namespace>
  <Namespace>Google.Protobuf</Namespace>
  <Namespace>Google.Protobuf.Collections</Namespace>
  <Namespace>Google.Protobuf.Reflection</Namespace>
  <Namespace>Google.Protobuf.WellKnownTypes</Namespace>
  <Namespace>Microsoft.ML</Namespace>
  <Namespace>Microsoft.ML.Core.Data</Namespace>
  <Namespace>Microsoft.ML.Data</Namespace>
  <Namespace>Microsoft.ML.Models</Namespace>
  <Namespace>Microsoft.ML.Runtime</Namespace>
  <Namespace>Microsoft.ML.Runtime.Api</Namespace>
  <Namespace>Microsoft.ML.Runtime.Command</Namespace>
  <Namespace>Microsoft.ML.Runtime.CommandLine</Namespace>
  <Namespace>Microsoft.ML.Runtime.Data</Namespace>
  <Namespace>Microsoft.ML.Runtime.Data.Conversion</Namespace>
  <Namespace>Microsoft.ML.Runtime.Data.IO</Namespace>
  <Namespace>Microsoft.ML.Runtime.Data.IO.Zlib</Namespace>
  <Namespace>Microsoft.ML.Runtime.DataPipe</Namespace>
  <Namespace>Microsoft.ML.Runtime.EntryPoints</Namespace>
  <Namespace>Microsoft.ML.Runtime.EntryPoints.CodeGen</Namespace>
  <Namespace>Microsoft.ML.Runtime.EntryPoints.JsonUtils</Namespace>
  <Namespace>Microsoft.ML.Runtime.FactorizationMachine</Namespace>
  <Namespace>Microsoft.ML.Runtime.FastTree</Namespace>
  <Namespace>Microsoft.ML.Runtime.FastTree.Internal</Namespace>
  <Namespace>Microsoft.ML.Runtime.Internal.Calibration</Namespace>
  <Namespace>Microsoft.ML.Runtime.Internal.CpuMath</Namespace>
  <Namespace>Microsoft.ML.Runtime.Internal.Internallearn</Namespace>
  <Namespace>Microsoft.ML.Runtime.Internal.Internallearn.ResultProcessor</Namespace>
  <Namespace>Microsoft.ML.Runtime.Internal.Tools</Namespace>
  <Namespace>Microsoft.ML.Runtime.Internal.Utilities</Namespace>
  <Namespace>Microsoft.ML.Runtime.KMeans</Namespace>
  <Namespace>Microsoft.ML.Runtime.Learners</Namespace>
  <Namespace>Microsoft.ML.Runtime.LightGBM</Namespace>
  <Namespace>Microsoft.ML.Runtime.Model</Namespace>
  <Namespace>Microsoft.ML.Runtime.Model.Onnx</Namespace>
  <Namespace>Microsoft.ML.Runtime.Model.Pfa</Namespace>
  <Namespace>Microsoft.ML.Runtime.Numeric</Namespace>
  <Namespace>Microsoft.ML.Runtime.PCA</Namespace>
  <Namespace>Microsoft.ML.Runtime.PipelineInference</Namespace>
  <Namespace>Microsoft.ML.Runtime.Sweeper</Namespace>
  <Namespace>Microsoft.ML.Runtime.Sweeper.Algorithms</Namespace>
  <Namespace>Microsoft.ML.Runtime.TextAnalytics</Namespace>
  <Namespace>Microsoft.ML.Runtime.Tools</Namespace>
  <Namespace>Microsoft.ML.Runtime.Training</Namespace>
  <Namespace>Microsoft.ML.Runtime.Transforms</Namespace>
  <Namespace>Microsoft.ML.Runtime.TreePredictor</Namespace>
  <Namespace>Microsoft.ML.Runtime.UniversalModelFormat.Onnx</Namespace>
  <Namespace>Microsoft.ML.Trainers</Namespace>
  <Namespace>Microsoft.ML.Transforms</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Bson</Namespace>
  <Namespace>Newtonsoft.Json.Converters</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Newtonsoft.Json.Schema</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
  <Namespace>System.Threading.Tasks.Dataflow</Namespace>
  <Namespace>Vita.Contracts</Namespace>
  <Namespace>Vita.Contracts.ChargeId</Namespace>
  <Namespace>Vita.Contracts.SubCategories</Namespace>
  <Namespace>Vita.Predictor</Namespace>
</Query>

/*

train model

*/

public static string Data = Vita.Predictor.PredictionModelWrapper.GetFilePath("data-sample.csv");
public static string Train = Vita.Predictor.PredictionModelWrapper.GetFilePath("train.csv");
public static string Test = Vita.Predictor.PredictionModelWrapper.GetFilePath("test.csv");
 
async System.Threading.Tasks.Task Main()
{
	if (!File.Exists(Data)) throw new FileNotFoundException(Data);
	
	Console.WriteLine("start...");
	
	var predict = new Predict();
	var modelpath = await predict.TrainAsync(Train);
	
	Console.WriteLine("done...");
 	
}