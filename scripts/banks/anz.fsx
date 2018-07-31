#load "refs.fsx"
open canopy
open canopy.classic

start chrome

describe "Go to Google.com and search for Attack of the Mutant Camels"
url "http://google.com"
"input[type=text]" << "Attack of the Mutant Camels"
press enter

describe "Check that the first result is the Wikipedia page"
"li h3" == "Attack of the Mutant Camels - Wikipedia, the free encyclopedia"

describe "Follow link to Wikipedia"
click "li h3"

describe "Check that the first paragraph mentions Jeff Minter"
"p" =~ "Jeff Minter"

quit()