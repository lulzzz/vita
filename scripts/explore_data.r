switch(Sys.info()[['sysname']],
       Windows = { suppressMessages(setwd("C:/dev/vita/scripts")) },
       Linux = { suppressMessages(setwd("~/dev/vita/scripts")) },
       Darwin = { print("I'm a Mac.") })

source(paste0(getwd(), '/setup.r'))
source(paste0(getwd(), '/include.r'))

data_path <- "C:/dev/vita/data/";

data.sample = read.csv(paste0(data_path, "data-sample.csv"))

# view original data sample
#file <- paste0(data_path, "data-sample.csv")
#csv <- read.csv.sql(file, sql = "select * from file", header = TRUE, sep = ",")

# load TSV
data.train = read_file_tsv(paste0(data_path, "train.tsv"))
data.test = read_file_tsv(paste0(data_path, "test.tsv"))

data.all <- merge(data.train, data.test)

# view the data
data.subcategories.amount = sqldf("select SubCategory, sum(Amount) as SubCategoryAmount from [data.all] where SubCategory not in ('SalaryWages', 'OtherTransferringMoney', 'CreditCard', 'Interest') group by SubCategory")
data.subcategories <- sqldf("select a.SubCategory, b.SubCategoRYAmount, count(a.SubCategory) as Total from [data.all] as a inner join [data.subcategories.amount] as b on a.SubCategory = b.SubCategory group by a.SubCategory order by count(a.SubCategory) desc")
#data.subcategories <- sqldf("select SubCategory from [data.all]")

#g <- ggplot(data.subcategories, aes(x = SubCategory, y = Total)) +
    #geom_bar(fill = "#0073C2FF", stat = "identity") +
    #geom_text(aes(label = Total), vjust = -0.3) +
    #theme(axis.text.x = element_text(angle = 90, hjust = 1))

#g


data.subcategories$SubCategoryAmount <- abs(data.subcategories$SubCategoryAmount)
top20 <- sqldf("select * from [data.subcategories] order by SubCategoryAmount desc limit 10")

pie3D(top20$SubCategoryAmount, labels = top20$SubCategory, main = "Top 10 spending categories", explode = 0.2, radius = .9, labelcex = 1.5, start = .6, theta = 1)