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
"#crn" << Vita.Domain.Infrastructure.SecretMan.Get("bankstatements-anz-cdm-username")
"#Password" << Vita.Domain.Infrastructure.SecretMan.Get("bankstatements-anz-cdm-password")
click "#SignonButton"
click "#proceedToIBButton"
sleep 1
click "div.backgroundView:nth-child(6) div.mainContentClass div.dispBlock:nth-child(9) div.normalLayoutAccounts:nth-child(2) div.listViewAccountWrapperYourAccounts.listViewFirstAccount:nth-child(1) a.accountNavLink div.accountNameTD > div.accountNameSection"
sleep 1
click "div.mainContentClass.ng-scope:nth-child(14) form.ng-pristine.ng-valid:nth-child(1) div.TranOutAutSec.TranOutAutSecLzyLdFx:nth-child(25) div.tabsContainer.tabsContainerMail.tabsContainerAcctTranAuth:nth-child(2) div.searchDwnldTransHistoryLinkSec.border-left-right:nth-child(1) div.dwnldTransHistoryDiv a:nth-child(1) > span.searchDwnldLinkSpan"
sleep 1
"#ANZSrchDtRng" << "5"
sleep 5
check "#extendedTxnDetailsChkBx"
click "#searchButton"
sleep 30
quit()