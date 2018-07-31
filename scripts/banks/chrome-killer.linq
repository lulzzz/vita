<Query Kind="FSharpProgram">
  <NuGetReference>canopy</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>canopy</Namespace>
  <Namespace>Microsoft.FSharp.Collections</Namespace>
  <Namespace>Microsoft.FSharp.Control</Namespace>
  <Namespace>Microsoft.FSharp.Core</Namespace>
  <Namespace>Microsoft.FSharp.Core.CompilerServices</Namespace>
  <Namespace>Microsoft.FSharp.Data.UnitSystems.SI.UnitNames</Namespace>
  <Namespace>Microsoft.FSharp.Linq</Namespace>
  <Namespace>Microsoft.FSharp.Linq.QueryRunExtensions</Namespace>
  <Namespace>Microsoft.FSharp.Linq.RuntimeHelpers</Namespace>
  <Namespace>Microsoft.FSharp.NativeInterop</Namespace>
  <Namespace>Microsoft.FSharp.Quotations</Namespace>
  <Namespace>Microsoft.FSharp.Reflection</Namespace>
</Query>

let killProcess name =      System.Diagnostics.Process.GetProcessesByName(name) 
                            |> Seq.iter(fun (x:System.Diagnostics.Process)-> 
                                                            try
                                                                x.Kill()
                                                                x.WaitForExit()
                                                            with
                                                                | err -> printfn "%A" err
                                                            )
killProcess "chrome"
killProcess "ChromeDriver"
killProcess "IEServerDriver"