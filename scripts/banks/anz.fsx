#load "refs.fsx"
#r "packages/NETStandard.Library.2.0.3/build/netstandard2.0/ref/netstandard.dll"
#r "packages/Microsoft.Azure.KeyVault.3.0.0/lib/netstandard1.4/Microsoft.Azure.KeyVault.dll"
#r "packages/Serilog.2.7.1/lib/netstandard1.3/serilog.dll"
#r "packages/Microsoft.Azure.Services.AppAuthentication.1.0.3/lib/netstandard1.4/Microsoft.Azure.Services.AppAuthentication.dll"
#r "packages/Microsoft.Rest.ClientRuntime.2.3.11/lib/netstandard1.4/Microsoft.Rest.ClientRuntime.dll"
#r "packages/Microsoft.Rest.ClientRuntime.Azure.3.3.12/lib/netstandard1.4/Microsoft.Rest.ClientRuntime.Azure.dll"

open canopy
open canopy.classic
open Vita.Domain.Infrastructure

start chrome

describe "ANZ Login"
url "https://www.anz.com/INETBANK/login.asp"
//click "#skip_logon"
sleep 1 
"#crn" << Vita.Domain.Infrastructure.SecretMan.Get("bankstatements-anz-test-username")
"#Password" << Vita.Domain.Infrastructure.SecretMan.Get("bankstatements-anz-test-password")
click "#SignonButton"
sleep 10
quit()