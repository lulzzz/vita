<Query Kind="Program" />

/*

Vita local setup

*/

void Main()
{
	string current = Path.GetDirectoryName(Util.CurrentQueryPath);
	string root = current.Replace(@"scripts\setup", string.Empty);

	// create model for build to work
	string model = $@"{root}data\vita-model-1.zip";
	if (!File.Exists(model)) File.Create(model);

	// build
	string command = $@"msbuild {root}source\vita.sln";
	command.Dump();
	ProcessStartInfo start = new ProcessStartInfo();
	start.FileName = "cmd.exe";
	start.Arguments = command;
	start.WorkingDirectory = $@"{root}\source";
	start.UseShellExecute = false;
	start.RedirectStandardOutput = true;
	start.RedirectStandardError = true;
	
	
	 try
	{
		// Start the process with the info we specified.
		// Call WaitForExit and then the using-statement will close.
		using (Process p = Process.Start(start))
		{
	
			string output = p.StandardOutput.ReadToEnd();
			string error = p.StandardError.ReadToEnd();
			
			output.Dump("output");
			error.Dump("error");
			p.WaitForExit();
		}
	}
	catch(Exception ex)
	{
		ex.Dump("exception");
		
	}

}