<Query Kind="Program">
  <Output>DataGrids</Output>
  <Reference Relative="..\..\FinPad\lib\FinPower\evohtmltopdf.dll">C:\dev\FinPad\lib\FinPower\evohtmltopdf.dll</Reference>
  <Reference Relative="..\..\FinPad\lib\Fasti\Fasti.Core.dll">C:\dev\FinPad\lib\Fasti\Fasti.Core.dll</Reference>
  <Reference Relative="..\..\FinPad\lib\Fasti\Fasti.FinPowerService.dll">C:\dev\FinPad\lib\Fasti\Fasti.FinPowerService.dll</Reference>
  <Reference Relative="..\..\FinPad\lib\Fasti\Fasti.Infrastructure.dll">C:\dev\FinPad\lib\Fasti\Fasti.Infrastructure.dll</Reference>
  <Reference Relative="..\..\FinPad\lib\Fasti\Fasti.WebApi.dll">C:\dev\FinPad\lib\Fasti\Fasti.WebApi.dll</Reference>
  <Reference Relative="..\..\FinPad\lib\Fasti\Fasti.WebCommon.dll">C:\dev\FinPad\lib\Fasti\Fasti.WebCommon.dll</Reference>
  <Reference Relative="..\..\FinPad\lib\FinPower\finReports2.dll">C:\dev\FinPad\lib\FinPower\finReports2.dll</Reference>
  <Reference Relative="..\..\FinPad\lib\FinPower\finSupport2.dll">C:\dev\FinPad\lib\FinPower\finSupport2.dll</Reference>
  <Reference Relative="..\..\FinPad\lib\FinPower\GemBox.Document.dll">C:\dev\FinPad\lib\FinPower\GemBox.Document.dll</Reference>
  <Reference Relative="..\..\FinPad\lib\FinPower\ISAccountingInterface2.dll">C:\dev\FinPad\lib\FinPower\ISAccountingInterface2.dll</Reference>
  <Reference Relative="..\..\FinPad\lib\FinPower\ISAddress2.dll">C:\dev\FinPad\lib\FinPower\ISAddress2.dll</Reference>
  <Reference Relative="..\..\FinPad\lib\FinPower\ISBankInterface2.dll">C:\dev\FinPad\lib\FinPower\ISBankInterface2.dll</Reference>
  <Reference Relative="..\..\FinPad\lib\FinPower\ISControls2.dll">C:\dev\FinPad\lib\FinPower\ISControls2.dll</Reference>
  <Reference Relative="..\..\FinPad\lib\FinPower\ISCreditBureau2.dll">C:\dev\FinPad\lib\FinPower\ISCreditBureau2.dll</Reference>
  <Reference Relative="..\..\FinPad\lib\FinPower\ISDocumentManager2.dll">C:\dev\FinPad\lib\FinPower\ISDocumentManager2.dll</Reference>
  <Reference Relative="..\..\FinPad\lib\FinPower\ISElectronicSignature2.dll">C:\dev\FinPad\lib\FinPower\ISElectronicSignature2.dll</Reference>
  <Reference Relative="..\..\FinPad\lib\FinPower\ISReports2.dll">C:\dev\FinPad\lib\FinPower\ISReports2.dll</Reference>
  <Reference Relative="..\..\FinPad\lib\FinPower\ISRuntime2.dll">C:\dev\FinPad\lib\FinPower\ISRuntime2.dll</Reference>
  <Reference Relative="..\..\FinPad\lib\FinPower\ISSecurityEnquiry2.dll">C:\dev\FinPad\lib\FinPower\ISSecurityEnquiry2.dll</Reference>
  <Reference Relative="..\..\FinPad\lib\FinPower\ISSecurityRegister2.dll">C:\dev\FinPad\lib\FinPower\ISSecurityRegister2.dll</Reference>
  <Reference Relative="..\..\FinPad\lib\FinPower\ISSupport2.dll">C:\dev\FinPad\lib\FinPower\ISSupport2.dll</Reference>
  <Reference Relative="..\..\FinPad\lib\FinPower\ISUserInterface2.dll">C:\dev\FinPad\lib\FinPower\ISUserInterface2.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Linq.dll</Reference>
  <Reference Relative="..\..\FinPad\lib\Fasti\Z.ExtensionMethods.dll">C:\dev\FinPad\lib\Fasti\Z.ExtensionMethods.dll</Reference>
  <GACReference>Microsoft.VisualBasic, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</GACReference>
  <NuGetReference>SlackClient</NuGetReference>
  <Namespace>Intersoft.finSupport2</Namespace>
  <Namespace>Intersoft.ISRuntime2</Namespace>
  <Namespace>Intersoft.ISSupport2</Namespace>
  <Namespace>Microsoft.VisualBasic</Namespace>
  <Namespace>Microsoft.VisualBasic.ApplicationServices</Namespace>
  <Namespace>Microsoft.VisualBasic.CompilerServices</Namespace>
  <Namespace>Microsoft.VisualBasic.Devices</Namespace>
  <Namespace>Microsoft.VisualBasic.FileIO</Namespace>
  <Namespace>Microsoft.VisualBasic.Logging</Namespace>
  <Namespace>Microsoft.VisualBasic.MyServices</Namespace>
  <Namespace>Microsoft.VisualBasic.MyServices.Internal</Namespace>
  <Namespace>Slack.Client</Namespace>
  <Namespace>Slack.Client.Converters</Namespace>
  <Namespace>Slack.Client.entity</Namespace>
  <Namespace>Slack.Client.Resolvers</Namespace>
</Query>

/*

Angular component creator

*/

void Main()
{
	NormalComponent();
	//ControlComponent();	
 
 
}

void NormalComponent(){
	string component = "incomedetails";    // ----->  rename me
	const string folder = @"C:\dev\vita\source\Vita.Spa\ClientApp\app\components\";
	string templateDir = Path.Combine(folder,"template");
	string componentDir = Path.Combine(folder,component);
	
	Directory.CreateDirectory(componentDir);
	
	// copy from template dir
	foreach (var file in Directory.GetFiles(templateDir))
	{
		string xxx = Path.GetFileName(file).Replace("template", component);
		string yyy =  Path.Combine(componentDir, xxx);
		File.Copy(file,yyy);
		
		string contents = File.ReadAllText(yyy)
					.Replace("template", component)
					.Replace("Template", component.ToTitleCase())
					.Replace($"{component}Url","templateUrl")
					;
		File.WriteAllText(yyy,contents);
		
	}
}


void ControlComponent(){
	string component = "lz-select-state";    // ----->  rename me
	const string folder = @"C:\dev\vita\source\Vita.Spa\ClientApp\app\components\controls";
	string templateDir = Path.Combine(@"C:\dev\vita\source\Vita.Spa\ClientApp\app\components\controls\","lz-select-country");
	string componentDir = Path.Combine(folder,component);
	
	Directory.CreateDirectory(componentDir);
	
	// copy from template dir
	foreach (var file in Directory.GetFiles(templateDir))
	{
		string xxx = Path.GetFileName(file).Replace("lz-select-country", component);
		string yyy =  Path.Combine(componentDir, xxx);
		File.Copy(file,yyy);
		
		var parts = component.Split("-");
		
		string contents = File.ReadAllText(yyy)
					.Replace("lz-select-country", $"{parts[0].ToTitleCase()}{parts[1].ToTitleCase()}")
					.Replace("lz-select-country", component.ToTitleCase())
					.Replace($"{component}Url","templateUrl")
					;
		File.WriteAllText(yyy,contents);
		
	}
}