import os # import OS dependant functionality
import sys
import pandas as pd #import data analysis library required
import pandasql
from pandasql import sqldf
import pymssql


conn = pymssql.connect(server='earth', database='Vita',user='SqlAdmin', password='Mandurah4B!@#') 
cursor = conn.cursor()
cursor.execute("SELECT @@VERSION")
print(cursor.fetchone()[0])
stmt = "SELECT * FROM dbo.BankStatementReadModel"
#print(stmt)
df = pd.read_sql(stmt,conn)
df.head()
