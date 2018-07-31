#r "packages/NETStandard.Library.2.0.3/build/netstandard2.0/ref/netstandard.dll"
#r "./packages/Selenium.WebDriver/lib/netstandard2.0/WebDriver.dll"
#r "./packages/canopy/lib/canopy.dll"
#r "packages/Newtonsoft.Json/lib/netstandard2.0/Newtonsoft.Json.dll"
#r "packages/vita/Vita.Contracts.dll"
#r "packages/vita/Vita.Domain.dll"

open System
open System.Diagnostics
open System.IO
open System.IO.Compression
open System.Net
open System.Text.RegularExpressions
open canopy
open canopy.classic
open Vita.Domain.Infrastructure

configuration.chromeDir <- "c:/"
configuration.ieDir <- "c:/"
//configuration. <- @"c:/dev/vita/scripts/banks/packages/phantomjs.exe/tools/phantomjs" 
