switch(Sys.info()[['sysname']],
       Windows = { suppressMessages(setwd("C:/dev/vita/scripts")) },
       Linux = { suppressMessages(setwd("~/dev/vita/scripts")) },
       Darwin = { print("I'm a Mac.") })

source(paste0(getwd(), '/setup.r'))
source(paste0(getwd(), '/include.r'))

data_path <- "C:/dev/vita/data/";

data.cats = read.csv(paste0(data_path, "cats.csv"), header = FALSE, sep = ",")
colnames(data.cats) <- c("category", "total")

data.subs = read.csv(paste0(data_path, "subs.csv"), header = FALSE, sep = ",")
colnames(data.subs) <- c("subcategory", "total")

# load CSV
#see linqpad cross-validtion.linq to create these files from data.sample.csv
data.train = read_file_csv(paste0(data_path, "train.csv"))
data.test = read_file_csv(paste0(data_path, "test.csv"))

data.all <- merge(data.train, data.test)

# view the data
data.subcategories.amount <- sqldf("select SubCategory, sum(Amount) as SubCategoryAmount from [data.all] where SubCategory not in ('Wages', 'OtherTransferringMoney', 'CreditCard', 'Interest') group by SubCategory")
data.subcategories <- sqldf("select a.SubCategory, b.SubCategoRYAmount, count(a.SubCategory) as Total from [data.all] as a inner join [data.subcategories.amount] as b on a.SubCategory = b.SubCategory group by a.SubCategory order by count(a.SubCategory) desc")

data.cats = sqldf("select * from [data.cats] where total > 50 and category <> 'Uncategorised'")
g <- ggplot(data.cats, aes(x = category, y = total)) +
    geom_bar(fill = "#0073C2FF", stat = "identity") +
    geom_text(aes(label = total), vjust = -0.3) +
    theme(axis.text.x = element_text(angle = 90, hjust = 1))
g


data.subcategories$SubCategoryAmount <- abs(data.subcategories$SubCategoryAmount)
top20 <- sqldf("select * from [data.subcategories] where subcategory <> 'Uncategorised' order by SubCategoryAmount desc limit 20")
pie3D(top20$SubCategoryAmount, labels = top20$SubCategory, main = "Amount per category", explode = 0., radius = .9, labelcex = .7, start = .6, theta = 1)