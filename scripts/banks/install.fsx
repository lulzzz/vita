#r "System.IO.Compression.FileSystem.dll"
#r "System.IO.Compression.dll"

open System
open System.Diagnostics
open System.IO
open System.IO.Compression
open System.Net
open System.Text.RegularExpressions

let currentDir = Environment.CurrentDirectory
let packages = currentDir + "/packages"

let paket = currentDir + "/paket.exe"
let refs = currentDir + "/refs.fsx"
let demo = currentDir + "/demo.fsx"
let zipFile = currentDir + "/zipfile.zip"
let isNix = Environment.OSVersion.Platform = PlatformID.MacOSX || Environment.OSVersion.Platform = PlatformID.Unix

let webClient() = 
  let webClient = new WebClient(UseDefaultCredentials = true, Proxy = WebRequest.DefaultWebProxy)
  webClient.Proxy.Credentials <- CredentialCache.DefaultCredentials
  webClient

let writeFile file (line : string) = 
  use fileWriter = File.AppendText file
  fileWriter.WriteLine line

let startProcess fileName arguments = 
  let fileName, arguments = 
    if isNix then "mono", sprintf "%s %s" fileName arguments
    else fileName, arguments
  
  let processStartInfo = 
    ProcessStartInfo(FileName = fileName, Arguments = arguments, UseShellExecute = false, CreateNoWindow = true)
  let pr = Process.Start processStartInfo
  pr.WaitForExit()

let downloadPackage package = 
  printfn "Installing '%s'" package
  startProcess paket <| sprintf "add nuget %s" package

let deleteIfExists path = 
  printfn "Deleting '%s'" path
  if (Directory.Exists path) then Directory.Delete(path, true)
  else File.Delete path

let download source destination = 
  printfn "Downloading '%s'" source
  webClient().DownloadFile(source, destination)

let downloadPaket() = 
  let paketSource = webClient().DownloadString "http://fsprojects.github.io/Paket/stable"
  download paketSource paket
  startProcess paket "init"

let downloadAndUnzip source = 
  download source zipFile
  ZipFile.ExtractToDirectory(zipFile, packages)
  deleteIfExists zipFile

let downloadChromeDriver() = 
  webClient().DownloadString "http://chromedriver.storage.googleapis.com/LATEST_RELEASE"
  |> sprintf "http://chromedriver.storage.googleapis.com/%s/chromedriver_win32.zip"
  |> downloadAndUnzip

let downloadIeDriver() = 
  let html = webClient().DownloadString "http://selenium-release.storage.googleapis.com/?delimiter=/"
  
  let version = 
    [ for m in Regex.Matches(html, "(?<=\>)[\d\.]+(?=/\<)") do
        yield m.Value ]
    |> List.sortBy id
    |> List.rev
    |> List.head
  sprintf "http://selenium-release.storage.googleapis.com/%s/IEDriverServer_x64_%s.0.zip" version version 
  |> downloadAndUnzip

let deletePaket() = 
  deleteIfExists paket
  deleteIfExists "paket.lock"
  deleteIfExists "paket.dependencies"

deleteIfExists refs
deleteIfExists demo
deleteIfExists packages
deletePaket()
downloadPaket()
downloadPackage "canopy"
downloadPackage "phantomjs.exe"
deletePaket()
printfn "Installing 'ChromeDriver'"
downloadChromeDriver()
//printfn "Installing 'IEDriverServer'"
//downloadIeDriver()
printfn "Creating 'refs.fsx'"

let refsText = sprintf """
#I "packages/Selenium.WebDriver/lib/net40"
#I "packages/Selenium.Support/lib/net40"
#r "WebDriver.dll"
#r "WebDriver.Support.dll"
#r "packages/Newtonsoft.Json/lib/net40/Newtonsoft.Json.dll"
#r "packages/SizSelCsZzz/lib/SizSelCsZzz.dll"
#r "packages/canopy/lib/canopy.dll"
open canopy
configuration.chromeDir <- "%s"
configuration.ieDir <- "%s"
configuration.phantomJSDir <- @"%s/phantomjs.exe/tools/phantomjs" """ packages packages packages

writeFile refs refsText
printfn "Creating 'demo.fsx'"

let firefox = 
  if isNix then """FirefoxWithPath "/usr/lib/firefox/firefox" """
  else "firefox"

let demoText = sprintf """
#load "refs.fsx"
open canopy
open canopy.types
start %s
url "http://google.com"
"input" << "canopy fsharp"
press enter
sleep 5000
quit()
"""

demoText firefox |> writeFile demo
printfn "Done"
