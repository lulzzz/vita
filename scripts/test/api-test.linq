<Query Kind="Program">
  <Connection>
    <ID>c5e63f91-715c-4cdc-b87c-cc24ace9a884</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Vita</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Output>DataGrids</Output>
  <Reference>&lt;RuntimeDirectory&gt;\System.ComponentModel.DataAnnotations.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.Serialization.dll</Reference>
  <Reference Relative="..\..\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Api.dll">C:\dev\vita\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Api.dll</Reference>
  <Reference Relative="..\..\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Contracts.dll">C:\dev\vita\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Contracts.dll</Reference>
  <Reference Relative="..\..\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Domain.dll">C:\dev\vita\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Domain.dll</Reference>
  <Reference Relative="..\..\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Predictor.dll">C:\dev\vita\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Predictor.dll</Reference>
  <NuGetReference>EventFlow</NuGetReference>
  <NuGetReference>ExtensionMinder</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>EventFlow</Namespace>
  <Namespace>EventFlow.Aggregates</Namespace>
  <Namespace>EventFlow.Aggregates.ExecutionResults</Namespace>
  <Namespace>EventFlow.Commands</Namespace>
  <Namespace>EventFlow.Configuration</Namespace>
  <Namespace>EventFlow.Configuration.Bootstraps</Namespace>
  <Namespace>EventFlow.Configuration.Decorators</Namespace>
  <Namespace>EventFlow.Core</Namespace>
  <Namespace>EventFlow.Core.Caching</Namespace>
  <Namespace>EventFlow.Core.RetryStrategies</Namespace>
  <Namespace>EventFlow.Core.VersionedTypes</Namespace>
  <Namespace>EventFlow.Entities</Namespace>
  <Namespace>EventFlow.EventStores</Namespace>
  <Namespace>EventFlow.EventStores.Files</Namespace>
  <Namespace>EventFlow.EventStores.InMemory</Namespace>
  <Namespace>EventFlow.Exceptions</Namespace>
  <Namespace>EventFlow.Extensions</Namespace>
  <Namespace>EventFlow.Jobs</Namespace>
  <Namespace>EventFlow.Logs</Namespace>
  <Namespace>EventFlow.Logs.Internals.Logging</Namespace>
  <Namespace>EventFlow.MetadataProviders</Namespace>
  <Namespace>EventFlow.Provided</Namespace>
  <Namespace>EventFlow.Provided.Jobs</Namespace>
  <Namespace>EventFlow.Provided.Specifications</Namespace>
  <Namespace>EventFlow.Queries</Namespace>
  <Namespace>EventFlow.ReadStores</Namespace>
  <Namespace>EventFlow.ReadStores.InMemory</Namespace>
  <Namespace>EventFlow.ReadStores.InMemory.Queries</Namespace>
  <Namespace>EventFlow.Sagas</Namespace>
  <Namespace>EventFlow.Sagas.AggregateSagas</Namespace>
  <Namespace>EventFlow.Snapshots</Namespace>
  <Namespace>EventFlow.Snapshots.Stores</Namespace>
  <Namespace>EventFlow.Snapshots.Stores.InMemory</Namespace>
  <Namespace>EventFlow.Snapshots.Stores.Null</Namespace>
  <Namespace>EventFlow.Snapshots.Strategies</Namespace>
  <Namespace>EventFlow.Specifications</Namespace>
  <Namespace>EventFlow.Subscribers</Namespace>
  <Namespace>EventFlow.ValueObjects</Namespace>
  <Namespace>ExtensionMinder</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Bson</Namespace>
  <Namespace>Newtonsoft.Json.Converters</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Newtonsoft.Json.Schema</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
  <Namespace>System.Runtime.Caching</Namespace>
  <Namespace>System.Runtime.Caching.Configuration</Namespace>
  <Namespace>System.Runtime.Caching.Hosting</Namespace>
  <Namespace>Vita.Contracts</Namespace>
  <Namespace>Vita.Domain.Infrastructure</Namespace>
</Query>

/*

CommandController test

*/
async System.Threading.Tasks.Task Main()
{
	Vita.Domain.Charges.Commands.ImportChargesCommand cmd = new Vita.Domain.Charges.Commands.ImportChargesCommand();
	cmd.AggregateId = Vita.Domain.Charges.ChargeId.New;
	string json = JsonConvert.SerializeObject(cmd);
	VitaClient client = new VitaClient();
	await client.PublishCommandAsync(json, typeof(Vita.Domain.Charges.Commands.ImportChargesCommand).FullName);
 
}


[System.CodeDom.Compiler.GeneratedCode("NSwag", "11.18.5.0 (NJsonSchema v9.10.67.0 (Newtonsoft.Json v9.0.0.0))")]
public partial class VitaClient
{
	private string _baseUrl = "http://localhost:5001";
	private System.Lazy<Newtonsoft.Json.JsonSerializerSettings> _settings;

	public VitaClient()
	{
		_settings = new System.Lazy<Newtonsoft.Json.JsonSerializerSettings>(() =>
		{
			var settings = new Newtonsoft.Json.JsonSerializerSettings();
			UpdateJsonSerializerSettings(settings);
			return settings;
		});
	}

	public string BaseUrl
	{
		get { return _baseUrl; }
		set { _baseUrl = value; }
	}

	protected Newtonsoft.Json.JsonSerializerSettings JsonSerializerSettings { get { return _settings.Value; } }

	partial void UpdateJsonSerializerSettings(Newtonsoft.Json.JsonSerializerSettings settings);
	partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url);
	partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, System.Text.StringBuilder urlBuilder);
	partial void ProcessResponse(System.Net.Http.HttpClient client, System.Net.Http.HttpResponseMessage response);

	/// <exception cref="SwaggerException">A server side error occurred.</exception>
	public System.Threading.Tasks.Task<FileResponse> PublishCommandAsync(object commandJson, string name)
	{
		return PublishCommandAsync(commandJson, name, System.Threading.CancellationToken.None);
	}

	/// <exception cref="SwaggerException">A server side error occurred.</exception>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	public async System.Threading.Tasks.Task<FileResponse> PublishCommandAsync(object commandJson, string name, System.Threading.CancellationToken cancellationToken)
	{
		if (name == null)
			throw new System.ArgumentNullException("name");

		var urlBuilder_ = new System.Text.StringBuilder();
		urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/Command/{name}");
		urlBuilder_.Replace("{name}", System.Uri.EscapeDataString(ConvertToString(name, System.Globalization.CultureInfo.InvariantCulture)));

		var client_ = new System.Net.Http.HttpClient();
		try
		{
			using (var request_ = new System.Net.Http.HttpRequestMessage())
			{
				var content_ = new System.Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(commandJson, _settings.Value));
				content_.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");
				request_.Content = content_;
				request_.Method = new System.Net.Http.HttpMethod("POST");
				request_.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

				PrepareRequest(client_, request_, urlBuilder_);
				var url_ = urlBuilder_.ToString();
				request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);
				PrepareRequest(client_, request_, url_);

				var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
				try
				{
					var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
					if (response_.Content != null && response_.Content.Headers != null)
					{
						foreach (var item_ in response_.Content.Headers)
							headers_[item_.Key] = item_.Value;
					}

					ProcessResponse(client_, response_);

					var status_ = ((int)response_.StatusCode).ToString();
					if (status_ == "200" || status_ == "206")
					{
						var responseStream_ = response_.Content == null ? System.IO.Stream.Null : await response_.Content.ReadAsStreamAsync().ConfigureAwait(false);
						var fileResponse_ = new FileResponse((int)response_.StatusCode, headers_, responseStream_, client_, response_);
						client_ = null; response_ = null; // response and client are disposed by FileResponse
						return fileResponse_;
					}
					else
					if (status_ != "200" && status_ != "204")
					{
						var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
						throw new SwaggerException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
					}

					return default(FileResponse);
				}
				finally
				{
					if (response_ != null)
						response_.Dispose();
				}
			}
		}
		finally
		{
			if (client_ != null)
				client_.Dispose();
		}
	}

	private string ConvertToString(object value, System.Globalization.CultureInfo cultureInfo)
	{
		if (value is System.Enum)
		{
			string name = System.Enum.GetName(value.GetType(), value);
			if (name != null)
			{
				var field = System.Reflection.IntrospectionExtensions.GetTypeInfo(value.GetType()).GetDeclaredField(name);
				if (field != null)
				{
					var attribute = System.Reflection.CustomAttributeExtensions.GetCustomAttribute(field, typeof(System.Runtime.Serialization.EnumMemberAttribute))
						as System.Runtime.Serialization.EnumMemberAttribute;
					if (attribute != null)
					{
						return attribute.Value;
					}
				}
			}
		}
		else if (value is byte[])
		{
			return System.Convert.ToBase64String((byte[])value);
		}
		else if (value != null && value.GetType().IsArray)
		{
			var array = System.Linq.Enumerable.OfType<object>((System.Array)value);
			return string.Join(",", System.Linq.Enumerable.Select(array, o => ConvertToString(o, cultureInfo)));
		}

		return System.Convert.ToString(value, cultureInfo);
	}
}


[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.10.67.0 (Newtonsoft.Json v9.0.0.0)")]
public partial class SearchResponse : System.ComponentModel.INotifyPropertyChanged
{
	private System.Guid _id;
	private Company _who;
	private TransactionType? _what;
	private Locality _where;
	private System.DateTime? _when;
	private Classifier _why;
	private PaymentMethodType? _paymentMethodType;
	private System.DateTime _createdUtcDate;
	private bool _isChargeId;
	private System.Guid? _chargeId;

	[Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public System.Guid Id
	{
		get { return _id; }
		set
		{
			if (_id != value)
			{
				_id = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("who", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public Company Who
	{
		get { return _who; }
		set
		{
			if (_who != value)
			{
				_who = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("what", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	[Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
	public TransactionType? What
	{
		get { return _what; }
		set
		{
			if (_what != value)
			{
				_what = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("where", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public Locality Where
	{
		get { return _where; }
		set
		{
			if (_where != value)
			{
				_where = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("when", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public System.DateTime? When
	{
		get { return _when; }
		set
		{
			if (_when != value)
			{
				_when = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("why", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public Classifier Why
	{
		get { return _why; }
		set
		{
			if (_why != value)
			{
				_why = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("paymentMethodType", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	[Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
	public PaymentMethodType? PaymentMethodType
	{
		get { return _paymentMethodType; }
		set
		{
			if (_paymentMethodType != value)
			{
				_paymentMethodType = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("createdUtcDate", Required = Newtonsoft.Json.Required.Always)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public System.DateTime CreatedUtcDate
	{
		get { return _createdUtcDate; }
		set
		{
			if (_createdUtcDate != value)
			{
				_createdUtcDate = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("isChargeId", Required = Newtonsoft.Json.Required.Always)]
	public bool IsChargeId
	{
		get { return _isChargeId; }
		set
		{
			if (_isChargeId != value)
			{
				_isChargeId = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("chargeId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public System.Guid? ChargeId
	{
		get { return _chargeId; }
		set
		{
			if (_chargeId != value)
			{
				_chargeId = value;
				RaisePropertyChanged();
			}
		}
	}

	public string ToJson()
	{
		return Newtonsoft.Json.JsonConvert.SerializeObject(this);
	}

	public static SearchResponse FromJson(string data)
	{
		return Newtonsoft.Json.JsonConvert.DeserializeObject<SearchResponse>(data);
	}

	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

	protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
	{
		var handler = PropertyChanged;
		if (handler != null)
			handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
	}

}

[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.10.67.0 (Newtonsoft.Json v9.0.0.0)")]
public abstract partial class Tracking : System.ComponentModel.INotifyPropertyChanged
{
	private System.DateTime _createdUtc;
	private System.DateTime? _modifiedUtc;

	[Newtonsoft.Json.JsonProperty("createdUtc", Required = Newtonsoft.Json.Required.Always)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public System.DateTime CreatedUtc
	{
		get { return _createdUtc; }
		set
		{
			if (_createdUtc != value)
			{
				_createdUtc = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("modifiedUtc", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public System.DateTime? ModifiedUtc
	{
		get { return _modifiedUtc; }
		set
		{
			if (_modifiedUtc != value)
			{
				_modifiedUtc = value;
				RaisePropertyChanged();
			}
		}
	}

	public string ToJson()
	{
		return Newtonsoft.Json.JsonConvert.SerializeObject(this);
	}

	public static Tracking FromJson(string data)
	{
		return Newtonsoft.Json.JsonConvert.DeserializeObject<Tracking>(data);
	}

	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

	protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
	{
		var handler = PropertyChanged;
		if (handler != null)
			handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
	}

}

[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.10.67.0 (Newtonsoft.Json v9.0.0.0)")]
public partial class Company : Tracking, System.ComponentModel.INotifyPropertyChanged
{
	private System.Guid _id;
	private string _companyName;
	private string _australianCompanyNumber;
	private string _companyType;
	private string _companyClass;
	private string _subClass;
	private string _status;
	private string _dateOfRegistration;
	private string _previousStateOfRegistration;
	private string _stateOfRegistrationNumber;
	private string _modifiedSinceLastReport;
	private string _currentNameIndicator;
	private string _australianBusinessNumber;
	private string _currentName;
	private string _currentNameStartDate;
	private string _companyCurrentInd;
	private string _companyCurrentName;
	private string _companyCurrentNameStartDt;
	private string _companyModifiedSinceLast;

	[Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public System.Guid Id
	{
		get { return _id; }
		set
		{
			if (_id != value)
			{
				_id = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("companyName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string CompanyName
	{
		get { return _companyName; }
		set
		{
			if (_companyName != value)
			{
				_companyName = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("australianCompanyNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string AustralianCompanyNumber
	{
		get { return _australianCompanyNumber; }
		set
		{
			if (_australianCompanyNumber != value)
			{
				_australianCompanyNumber = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("companyType", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string CompanyType
	{
		get { return _companyType; }
		set
		{
			if (_companyType != value)
			{
				_companyType = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("companyClass", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string CompanyClass
	{
		get { return _companyClass; }
		set
		{
			if (_companyClass != value)
			{
				_companyClass = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("subClass", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string SubClass
	{
		get { return _subClass; }
		set
		{
			if (_subClass != value)
			{
				_subClass = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("status", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string Status
	{
		get { return _status; }
		set
		{
			if (_status != value)
			{
				_status = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("dateOfRegistration", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string DateOfRegistration
	{
		get { return _dateOfRegistration; }
		set
		{
			if (_dateOfRegistration != value)
			{
				_dateOfRegistration = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("previousStateOfRegistration", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string PreviousStateOfRegistration
	{
		get { return _previousStateOfRegistration; }
		set
		{
			if (_previousStateOfRegistration != value)
			{
				_previousStateOfRegistration = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("stateOfRegistrationNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string StateOfRegistrationNumber
	{
		get { return _stateOfRegistrationNumber; }
		set
		{
			if (_stateOfRegistrationNumber != value)
			{
				_stateOfRegistrationNumber = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("modifiedSinceLastReport", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string ModifiedSinceLastReport
	{
		get { return _modifiedSinceLastReport; }
		set
		{
			if (_modifiedSinceLastReport != value)
			{
				_modifiedSinceLastReport = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("currentNameIndicator", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string CurrentNameIndicator
	{
		get { return _currentNameIndicator; }
		set
		{
			if (_currentNameIndicator != value)
			{
				_currentNameIndicator = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("australianBusinessNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string AustralianBusinessNumber
	{
		get { return _australianBusinessNumber; }
		set
		{
			if (_australianBusinessNumber != value)
			{
				_australianBusinessNumber = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("currentName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string CurrentName
	{
		get { return _currentName; }
		set
		{
			if (_currentName != value)
			{
				_currentName = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("currentNameStartDate", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string CurrentNameStartDate
	{
		get { return _currentNameStartDate; }
		set
		{
			if (_currentNameStartDate != value)
			{
				_currentNameStartDate = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("companyCurrentInd", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string CompanyCurrentInd
	{
		get { return _companyCurrentInd; }
		set
		{
			if (_companyCurrentInd != value)
			{
				_companyCurrentInd = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("companyCurrentName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string CompanyCurrentName
	{
		get { return _companyCurrentName; }
		set
		{
			if (_companyCurrentName != value)
			{
				_companyCurrentName = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("companyCurrentNameStartDt", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string CompanyCurrentNameStartDt
	{
		get { return _companyCurrentNameStartDt; }
		set
		{
			if (_companyCurrentNameStartDt != value)
			{
				_companyCurrentNameStartDt = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("companyModifiedSinceLast", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string CompanyModifiedSinceLast
	{
		get { return _companyModifiedSinceLast; }
		set
		{
			if (_companyModifiedSinceLast != value)
			{
				_companyModifiedSinceLast = value;
				RaisePropertyChanged();
			}
		}
	}

	public string ToJson()
	{
		return Newtonsoft.Json.JsonConvert.SerializeObject(this);
	}

	public static Company FromJson(string data)
	{
		return Newtonsoft.Json.JsonConvert.DeserializeObject<Company>(data);
	}

	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

	protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
	{
		var handler = PropertyChanged;
		if (handler != null)
			handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
	}

}

[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.10.67.0 (Newtonsoft.Json v9.0.0.0)")]
public enum TransactionType
{
	[System.Runtime.Serialization.EnumMember(Value = "Credit")]
	Credit = 0,

	[System.Runtime.Serialization.EnumMember(Value = "Debit")]
	Debit = 1,

	[System.Runtime.Serialization.EnumMember(Value = "Transfer")]
	Transfer = 2,

	[System.Runtime.Serialization.EnumMember(Value = "Reversal")]
	Reversal = 3,

	[System.Runtime.Serialization.EnumMember(Value = "Dishonour")]
	Dishonour = 4,

	[System.Runtime.Serialization.EnumMember(Value = "Fees")]
	Fees = 5,

	[System.Runtime.Serialization.EnumMember(Value = "Overdrawn")]
	Overdrawn = 6,

	[System.Runtime.Serialization.EnumMember(Value = "Interest")]
	Interest = 7,

	[System.Runtime.Serialization.EnumMember(Value = "Repayments")]
	Repayments = 8,

	[System.Runtime.Serialization.EnumMember(Value = "Unknown")]
	Unknown = 9,

}

[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.10.67.0 (Newtonsoft.Json v9.0.0.0)")]
public partial class Locality : Tracking, System.ComponentModel.INotifyPropertyChanged
{
	private System.Guid _id;
	private string _postcode;
	private string _suburb;
	private AustralianState? _australianState;
	private double _latitude;
	private double _longitude;
	private string _placeId;

	[Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public System.Guid Id
	{
		get { return _id; }
		set
		{
			if (_id != value)
			{
				_id = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("postcode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string Postcode
	{
		get { return _postcode; }
		set
		{
			if (_postcode != value)
			{
				_postcode = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("suburb", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string Suburb
	{
		get { return _suburb; }
		set
		{
			if (_suburb != value)
			{
				_suburb = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("australianState", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public AustralianState? AustralianState
	{
		get { return _australianState; }
		set
		{
			if (_australianState != value)
			{
				_australianState = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("latitude", Required = Newtonsoft.Json.Required.Always)]
	public double Latitude
	{
		get { return _latitude; }
		set
		{
			if (_latitude != value)
			{
				_latitude = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("longitude", Required = Newtonsoft.Json.Required.Always)]
	public double Longitude
	{
		get { return _longitude; }
		set
		{
			if (_longitude != value)
			{
				_longitude = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("placeId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string PlaceId
	{
		get { return _placeId; }
		set
		{
			if (_placeId != value)
			{
				_placeId = value;
				RaisePropertyChanged();
			}
		}
	}

	public string ToJson()
	{
		return Newtonsoft.Json.JsonConvert.SerializeObject(this);
	}

	public static Locality FromJson(string data)
	{
		return Newtonsoft.Json.JsonConvert.DeserializeObject<Locality>(data);
	}

	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

	protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
	{
		var handler = PropertyChanged;
		if (handler != null)
			handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
	}

}

[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.10.67.0 (Newtonsoft.Json v9.0.0.0)")]
public enum AustralianState
{
	ACT = 0,

	NSW = 1,

	NT = 2,

	QLD = 3,

	SA = 4,

	TAS = 5,

	VIC = 6,

	WA = 7,

}

[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.10.67.0 (Newtonsoft.Json v9.0.0.0)")]
public partial class Classifier : System.ComponentModel.INotifyPropertyChanged
{
	private System.Guid _id;
	private CategoryType _categoryType;
	private string _subCategory;
	private System.Collections.ObjectModel.ObservableCollection<string> _keywords;

	[Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public System.Guid Id
	{
		get { return _id; }
		set
		{
			if (_id != value)
			{
				_id = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("categoryType", Required = Newtonsoft.Json.Required.Always)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	[Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
	public CategoryType CategoryType
	{
		get { return _categoryType; }
		set
		{
			if (_categoryType != value)
			{
				_categoryType = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("subCategory", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string SubCategory
	{
		get { return _subCategory; }
		set
		{
			if (_subCategory != value)
			{
				_subCategory = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("keywords", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public System.Collections.ObjectModel.ObservableCollection<string> Keywords
	{
		get { return _keywords; }
		set
		{
			if (_keywords != value)
			{
				_keywords = value;
				RaisePropertyChanged();
			}
		}
	}

	public string ToJson()
	{
		return Newtonsoft.Json.JsonConvert.SerializeObject(this);
	}

	public static Classifier FromJson(string data)
	{
		return Newtonsoft.Json.JsonConvert.DeserializeObject<Classifier>(data);
	}

	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

	protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
	{
		var handler = PropertyChanged;
		if (handler != null)
			handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
	}

}

[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.10.67.0 (Newtonsoft.Json v9.0.0.0)")]
public enum CategoryType
{
	[System.Runtime.Serialization.EnumMember(Value = "BankingFinance")]
	BankingFinance = 0,

	[System.Runtime.Serialization.EnumMember(Value = "Entertainment")]
	Entertainment = 1,

	[System.Runtime.Serialization.EnumMember(Value = "FoodDrinks")]
	FoodDrinks = 2,

	[System.Runtime.Serialization.EnumMember(Value = "Groceries")]
	Groceries = 3,

	[System.Runtime.Serialization.EnumMember(Value = "HealthBeauty")]
	HealthBeauty = 4,

	[System.Runtime.Serialization.EnumMember(Value = "HolidayTravel")]
	HolidayTravel = 5,

	[System.Runtime.Serialization.EnumMember(Value = "Home")]
	Home = 6,

	[System.Runtime.Serialization.EnumMember(Value = "HouseholdUtilities")]
	HouseholdUtilities = 7,

	[System.Runtime.Serialization.EnumMember(Value = "Income")]
	Income = 8,

	[System.Runtime.Serialization.EnumMember(Value = "Insurance")]
	Insurance = 9,

	[System.Runtime.Serialization.EnumMember(Value = "Kids")]
	Kids = 10,

	[System.Runtime.Serialization.EnumMember(Value = "Miscellaneous")]
	Miscellaneous = 11,

	[System.Runtime.Serialization.EnumMember(Value = "Shopping")]
	Shopping = 12,

	[System.Runtime.Serialization.EnumMember(Value = "TransferringMoney")]
	TransferringMoney = 13,

	[System.Runtime.Serialization.EnumMember(Value = "Transport")]
	Transport = 14,

	[System.Runtime.Serialization.EnumMember(Value = "Uncategorised")]
	Uncategorised = 15,

	[System.Runtime.Serialization.EnumMember(Value = "WorkStudy")]
	WorkStudy = 16,

}

[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.10.67.0 (Newtonsoft.Json v9.0.0.0)")]
public enum PaymentMethodType
{
	[System.Runtime.Serialization.EnumMember(Value = "CashWithdrawl")]
	CashWithdrawl = 0,

	[System.Runtime.Serialization.EnumMember(Value = "Eftpos")]
	Eftpos = 1,

	[System.Runtime.Serialization.EnumMember(Value = "DirectDebit")]
	DirectDebit = 2,

	[System.Runtime.Serialization.EnumMember(Value = "CreditCard")]
	CreditCard = 3,

	[System.Runtime.Serialization.EnumMember(Value = "Unknown")]
	Unknown = 4,

}

[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.10.67.0 (Newtonsoft.Json v9.0.0.0)")]
public partial class Charge : Tracking, System.ComponentModel.INotifyPropertyChanged
{
	private System.Guid _id;
	private string _searchPhrase;
	private string _keywords;
	private string _notes;
	private string _tags;
	private System.DateTime? _transactionUtcDate;
	private System.DateTime? _postedDate;
	private System.Guid? _companyId;
	private string _bankCode;
	private string _bankName;
	private string _bsb;
	private string _accountNumber;
	private string _accountName;
	private double? _amount;
	private double? _balance;
	private double? _balanceAvailable;
	private TransactionType? _transactionType;
	private ExpenseType _expenseType;
	private System.Guid? _localityId;
	private string _placeId;
	private CategoryType _category;
	private string _subCategory;
	private System.Collections.ObjectModel.ObservableCollection<PlaceLocationType> _placeLocationTypes;
	private PaymentMethodType _paymentMethod;
	private System.Collections.Generic.Dictionary<string, string> _jsonData;

	[Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public System.Guid Id
	{
		get { return _id; }
		set
		{
			if (_id != value)
			{
				_id = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("searchPhrase", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string SearchPhrase
	{
		get { return _searchPhrase; }
		set
		{
			if (_searchPhrase != value)
			{
				_searchPhrase = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("keywords", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string Keywords
	{
		get { return _keywords; }
		set
		{
			if (_keywords != value)
			{
				_keywords = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("notes", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string Notes
	{
		get { return _notes; }
		set
		{
			if (_notes != value)
			{
				_notes = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("tags", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string Tags
	{
		get { return _tags; }
		set
		{
			if (_tags != value)
			{
				_tags = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("transactionUtcDate", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public System.DateTime? TransactionUtcDate
	{
		get { return _transactionUtcDate; }
		set
		{
			if (_transactionUtcDate != value)
			{
				_transactionUtcDate = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("postedDate", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public System.DateTime? PostedDate
	{
		get { return _postedDate; }
		set
		{
			if (_postedDate != value)
			{
				_postedDate = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("companyId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public System.Guid? CompanyId
	{
		get { return _companyId; }
		set
		{
			if (_companyId != value)
			{
				_companyId = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("bankCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string BankCode
	{
		get { return _bankCode; }
		set
		{
			if (_bankCode != value)
			{
				_bankCode = value;
				RaisePropertyChanged();

			}
		}
	}

	[Newtonsoft.Json.JsonProperty("bankName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string BankName
	{
		get { return _bankName; }
		set
		{
			if (_bankName != value)
			{
				_bankName = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("bsb", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string Bsb
	{
		get { return _bsb; }
		set
		{
			if (_bsb != value)
			{
				_bsb = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("accountNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string AccountNumber
	{
		get { return _accountNumber; }
		set
		{
			if (_accountNumber != value)
			{
				_accountNumber = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("accountName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string AccountName
	{
		get { return _accountName; }
		set
		{
			if (_accountName != value)
			{
				_accountName = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("amount", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public double? Amount
	{
		get { return _amount; }
		set
		{
			if (_amount != value)
			{
				_amount = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("balance", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public double? Balance
	{
		get { return _balance; }
		set
		{
			if (_balance != value)
			{
				_balance = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("balanceAvailable", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public double? BalanceAvailable
	{
		get { return _balanceAvailable; }
		set
		{
			if (_balanceAvailable != value)
			{
				_balanceAvailable = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("transactionType", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	[Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
	public TransactionType? TransactionType
	{
		get { return _transactionType; }
		set
		{
			if (_transactionType != value)
			{
				_transactionType = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("expenseType", Required = Newtonsoft.Json.Required.Always)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	[Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
	public ExpenseType ExpenseType
	{
		get { return _expenseType; }
		set
		{
			if (_expenseType != value)
			{
				_expenseType = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("localityId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public System.Guid? LocalityId
	{
		get { return _localityId; }
		set
		{
			if (_localityId != value)
			{
				_localityId = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("placeId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string PlaceId
	{
		get { return _placeId; }
		set
		{
			if (_placeId != value)
			{
				_placeId = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("category", Required = Newtonsoft.Json.Required.Always)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	[Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
	public CategoryType Category
	{
		get { return _category; }
		set
		{
			if (_category != value)
			{
				_category = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("subCategory", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public string SubCategory
	{
		get { return _subCategory; }
		set
		{
			if (_subCategory != value)
			{
				_subCategory = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("placeLocationTypes", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public System.Collections.ObjectModel.ObservableCollection<PlaceLocationType> PlaceLocationTypes
	{
		get { return _placeLocationTypes; }
		set
		{
			if (_placeLocationTypes != value)
			{
				_placeLocationTypes = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("paymentMethod", Required = Newtonsoft.Json.Required.Always)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	[Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
	public PaymentMethodType PaymentMethod
	{
		get { return _paymentMethod; }
		set
		{
			if (_paymentMethod != value)
			{
				_paymentMethod = value;
				RaisePropertyChanged();
			}
		}
	}

	[Newtonsoft.Json.JsonProperty("jsonData", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
	public System.Collections.Generic.Dictionary<string, string> JsonData
	{
		get { return _jsonData; }
		set
		{
			if (_jsonData != value)
			{
				_jsonData = value;
				RaisePropertyChanged();
			}
		}
	}

	public string ToJson()
	{
		return Newtonsoft.Json.JsonConvert.SerializeObject(this);
	}

	public static Charge FromJson(string data)
	{
		return Newtonsoft.Json.JsonConvert.DeserializeObject<Charge>(data);
	}

	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

	protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
	{
		var handler = PropertyChanged;
		if (handler != null)
			handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
	}

}

[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.10.67.0 (Newtonsoft.Json v9.0.0.0)")]
public enum ExpenseType
{
	[System.Runtime.Serialization.EnumMember(Value = "None")]
	None = 0,

	[System.Runtime.Serialization.EnumMember(Value = "PersonalLoan")]
	PersonalLoan = 1,

	[System.Runtime.Serialization.EnumMember(Value = "CarLoan")]
	CarLoan = 2,

	[System.Runtime.Serialization.EnumMember(Value = "PaydayLoan")]
	PaydayLoan = 3,

	[System.Runtime.Serialization.EnumMember(Value = "ChildCare")]
	ChildCare = 4,

	[System.Runtime.Serialization.EnumMember(Value = "ChildSupport")]
	ChildSupport = 5,

	[System.Runtime.Serialization.EnumMember(Value = "DebtAgreement")]
	DebtAgreement = 6,

	[System.Runtime.Serialization.EnumMember(Value = "Foxtel")]
	Foxtel = 7,

	[System.Runtime.Serialization.EnumMember(Value = "GymMembership")]
	GymMembership = 8,

	[System.Runtime.Serialization.EnumMember(Value = "Insurances")]
	Insurances = 9,

	[System.Runtime.Serialization.EnumMember(Value = "PenaltyOrFinePayments")]
	PenaltyOrFinePayments = 10,

	[System.Runtime.Serialization.EnumMember(Value = "OtherExpenses")]
	OtherExpenses = 11,

	[System.Runtime.Serialization.EnumMember(Value = "Living")]
	Living = 12,

}

/// <summary>Place Location Types
/// https://developers.google.com/places/supported_types#table1
/// https://developers.google.com/places/supported_types#table2</summary>
[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.10.67.0 (Newtonsoft.Json v9.0.0.0)")]
public enum PlaceLocationType
{
	Uknown = 0,

	Geocode = 1,

	Street_Address = 2,

	Route = 3,

	Intersection = 4,

	Political = 5,

	Country = 6,

	Administrative_Area_Level_1 = 7,

	Administrative_Area_Level_2 = 8,

	Administrative_Area_Level_3 = 9,

	Administrative_Area_Level_4 = 10,

	Administrative_Area_Level_5 = 11,

	Colloquial_Area = 12,

	Locality = 13,

	Sublocality = 14,

	Sublocality_Level_1 = 15,

	Sublocality_Level_2 = 16,

	Sublocality_Level_3 = 17,

	Sublocality_Level_4 = 18,

	Sublocality_Level_5 = 19,

	Neighborhood = 20,

	Premise = 21,

	Subpremise = 22,

	Postal_Code = 23,

	Postal_Code_Prefix = 24,

	Postal_Code_Suffix = 25,

	Natural_Feature = 26,

	Point_Of_Interest = 27,

	Floor = 28,

	Post_Box = 29,

	Postal_Town = 30,

	Room = 31,

	Street_Number = 32,

	Transit_Station = 33,

	Accounting = 34,

	Airport = 35,

	Amusement_Park = 36,

	Aquarium = 37,

	Art_Gallery = 38,

	Atm = 39,

	Bakery = 40,

	Bank = 41,

	Bar = 42,

	Beauty_Salon = 43,

	Bicycle_Store = 44,

	Book_Store = 45,

	Bowling_Alley = 46,

	Bus_Station = 47,

	Cafe = 48,

	Campground = 49,

	Car_Dealer = 50,

	Car_Rental = 51,

	Car_Repair = 52,

	Car_Wash = 53,

	Casino = 54,

	Cemetery = 55,

	Church = 56,

	City_Hall = 57,

	Clothing_Store = 58,

	Convenience_Store = 59,

	Courthouse = 60,

	Dentist = 61,

	Department_Store = 62,

	Doctor = 63,

	Electrician = 64,

	Electronics_Store = 65,

	Embassy = 66,

	Establishment = 67,

	Finance = 68,

	Fire_Station = 69,

	Florist = 70,

	Food = 71,

	Funeral_Home = 72,

	Furniture_Store = 73,

	Gas_Station = 74,

	General_Contractor = 75,

	Supermarket = 76,

	Grocery_Or_Supermarket = 77,

	Gym = 78,

	Hair_Care = 79,

	Hardware_Store = 80,

	Health = 81,

	Hindu_Temple = 82,

	Home_Goods_Store = 83,

	Hospital = 84,

	Insurance_Agency = 85,

	Jewelry_Store = 86,

	Laundry = 87,

	Lawyer = 88,

	Library = 89,

	Liquor_Store = 90,

	Local_Government_Office = 91,

	Locksmith = 92,

	Lodging = 93,

	Meal_Delivery = 94,

	Meal_Takeaway = 95,

	Mosque = 96,

	Movie_Rental = 97,

	Movie_Theater = 98,

	Moving_Company = 99,

	Museum = 100,

	Night_Club = 101,

	Painter = 102,

	Park = 103,

	Parking = 104,

	Pet_Store = 105,

	Pharmacy = 106,

	Physiotherapist = 107,

	Place_Of_Worship = 108,

	Plumber = 109,

	Police = 110,

	PostOffice = 111,

	Real_Estate_Agency = 112,

	Restaurant = 113,

	Roofing_Contractor = 114,

	Rv_Park = 115,

	School = 116,

	Shoe_Store = 117,

	Shopping_Mall = 118,

	Spa = 119,

	Stadium = 120,

	Storage = 121,

	Store = 122,

	Subway_Station = 123,

	Synagogue = 124,

	Taxi_Stand = 125,

	Train_Station = 126,

	Travel_Agency = 127,

	University = 128,

	Veterinary_Care = 129,

	Zoo = 130,

}

[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.10.67.0 (Newtonsoft.Json v9.0.0.0)")]
public enum AccountType
{
	[System.Runtime.Serialization.EnumMember(Value = "Credit")]
	Credit = 0,

	[System.Runtime.Serialization.EnumMember(Value = "Debit")]
	Debit = 1,

	[System.Runtime.Serialization.EnumMember(Value = "Offset")]
	Offset = 2,

	[System.Runtime.Serialization.EnumMember(Value = "Facility")]
	Facility = 3,

}

public partial class FileResponse : System.IDisposable
{
	private System.IDisposable _client;
	private System.IDisposable _response;

	public int StatusCode { get; private set; }

	public System.Collections.Generic.Dictionary<string, System.Collections.Generic.IEnumerable<string>> Headers { get; private set; }

	public System.IO.Stream Stream { get; private set; }

	public bool IsPartial
	{
		get { return StatusCode == 206; }
	}

	public FileResponse(int statusCode, System.Collections.Generic.Dictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.IO.Stream stream, System.IDisposable client, System.IDisposable response)
	{
		StatusCode = statusCode;
		Headers = headers;
		Stream = stream;
		_client = client;
		_response = response;
	}

	public void Dispose()
	{
		if (Stream != null)
			Stream.Dispose();
		if (_response != null)
			_response.Dispose();
		if (_client != null)
			_client.Dispose();
	}
}

[System.CodeDom.Compiler.GeneratedCode("NSwag", "11.18.5.0 (NJsonSchema v9.10.67.0 (Newtonsoft.Json v9.0.0.0))")]
public partial class SwaggerException : System.Exception
{
	public int StatusCode { get; private set; }

	public string Response { get; private set; }

	public System.Collections.Generic.Dictionary<string, System.Collections.Generic.IEnumerable<string>> Headers { get; private set; }

	public SwaggerException(string message, int statusCode, string response, System.Collections.Generic.Dictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.Exception innerException)
		: base(message, innerException)
	{
		StatusCode = statusCode;
		Response = response;
		Headers = headers;
	}

	public override string ToString()
	{
		return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
	}
}

[System.CodeDom.Compiler.GeneratedCode("NSwag", "11.18.5.0 (NJsonSchema v9.10.67.0 (Newtonsoft.Json v9.0.0.0))")]
public partial class SwaggerException<TResult> : SwaggerException
{
	public TResult Result { get; private set; }

	public SwaggerException(string message, int statusCode, string response, System.Collections.Generic.Dictionary<string, System.Collections.Generic.IEnumerable<string>> headers, TResult result, System.Exception innerException)
		: base(message, statusCode, response, headers, innerException)
	{
		Result = result;
	}
}



