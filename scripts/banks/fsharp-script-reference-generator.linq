<Query Kind="Program" />

void Main()
{
	foreach (var x in Directory.GetFiles(@"C:\dev\vita\scripts\banks\packages", "*.dll",SearchOption.AllDirectories).Where(y => y.Contains("Microsoft.Rest.ClientRuntime")))
	{
		var a = x.Replace(@"C:\dev\vita\scripts\banks\", "'");
		a = a + "'";
		a = a.Replace("\\", "/");
		a = a.Replace(@"\","/");
		a= a.Replace("'","\"");
		Console.WriteLine($@"#r {a}");
	}
}

// Define other methods and classes here
