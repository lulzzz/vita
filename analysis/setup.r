
#' using
#'
#' @param packageName
#'
#' @return
#' @export
#'
#' @examples
using <- function(packageName) {

    #p_load(packageName)

    if (!require(packageName, character.only = TRUE) && !(packageName %in% installed.packages())) {
        install.packages(dput(packageName), dependencies = TRUE, quiet = FALSE)
    }

    library(packageName, character.only = TRUE)
}



# using('pacman')
using('data.table')
using('downloader')
using('dplyr')
using('DT')
using('foreach')
using('ggplot2')
using('knitr')
using('lubridate')
using('magrittr')
using('markdown')
using('sqldf')
using('stringi')
using('stringr')
using('tidyverse')
using('plotrix')

#install_github("ggrothendieck/sqldf")
#install_github("trinker/qdapRegex")
#install_github("trinker/qdapDictionaries")
#install_github("trinker/qdapRegex")
#install_github("trinker/qdapTools")
#install_github("trinker/qdap")

options(mc.cores = 1)
options(encoding = "UTF-8")
options(stringsAsFactors = FALSE)
options(save.defaults = list(ascii = TRUE, safe = FALSE))


 