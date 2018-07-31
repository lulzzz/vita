
#load "refs.fsx"
open canopy
open canopy.types
start firefox
url "http://google.com"
"input" << "canopy fsharp"
press enter
sleep 5000
quit()

