import pandas as pd
import pyodbc

conn = (
    r'DRIVER={SQL Server};'
    r'SERVER=(local);'
    r'DATABASE=Vita;'
    r'Trusted_Connection=yes;')

conn = pyodbc.connect(conn)
df= pd.read_sql('select * from dbo.BankStatementReadModel',conn)

if conn:
    print("Yes, we are connected ")
