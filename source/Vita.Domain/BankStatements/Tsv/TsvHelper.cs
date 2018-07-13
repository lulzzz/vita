using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Vita.Contracts;
using Vita.Domain.BankStatements.Models;
using Vita.Domain.Services.TextClassifiers;

namespace Vita.Domain.BankStatements.Tsv
{
  public static class TsvHelper
  {
    public static string Create(IEnumerable<BankStatementLineItem> data)
    {
      if (data == null) return null;
      var sb = new StringBuilder();
      const string delimiter = "\t";

      var tmp = new BankStatementLineItem();
      sb.AppendLine(nameof(tmp.SubCategory) + delimiter + nameof(tmp.Description) + delimiter + nameof(tmp.Bank) +
                    delimiter + nameof(tmp.Amount) + delimiter + nameof(tmp.AccountName) + delimiter + nameof(tmp.Notes) +
                    delimiter + nameof(tmp.Tags));

      foreach (var item in data)
      {
        sb.AppendLine((item.SubCategory) + delimiter + (item.Description) + delimiter + (item.Bank) + delimiter +
                      (item.Amount) + delimiter + (item.AccountName) + delimiter + (item.Notes) + delimiter + (item.Tags));
      }

      return sb.ToString();
    }

    public static string Create(FetchAllResponse response)
    {
      if (response == null) return null;

      var list = new List<BankStatementLineItem>();

      foreach (var account in response.Accounts)
      {
        var item = new BankStatementLineItem {AccountType = AccountTypeConverter.Convert(account.AccountType)};

        foreach (var detail in account.StatementData.Details)
        {
          item.TransactionUtcDate = detail.DateObj.Date.DateTime;
          item.Description = detail.Text;
          item.Amount = detail.Amount;
          item.Notes = Convert.ToString(detail.Notes);
          item.AccountNumber = account.AccountNumber;
          item.Bank = account.Institution;
          item.Tags = string.Join(",",detail.Tags);

          list.Add(item);
        }
      }

      var sb = new StringBuilder();
      const string delimiter = "\t";

      var tmp = new BankStatementLineItem();
      sb.AppendLine(nameof(tmp.SubCategory) + delimiter + nameof(tmp.Description) + delimiter + nameof(tmp.Bank) +
                    delimiter + nameof(tmp.Amount) + delimiter + nameof(tmp.AccountName) + delimiter + nameof(tmp.Notes) +
                    delimiter + nameof(tmp.Tags));

      foreach (var item in list)
      {
        sb.AppendLine((item.SubCategory) + delimiter + (item.Description) + delimiter + (item.Bank) + delimiter +
                      (item.Amount) + delimiter + (item.AccountName) + delimiter + (item.Notes) + delimiter + (item.Tags));
      }

      return sb.ToString();
    }

    public static string Create(IEnumerable<PocketBook> pbs)
    {
      
      if (pbs == null) return null;

      var list = new List<BankStatementLineItem>();

      foreach (var pb in pbs)
      {
        var item = new BankStatementLineItem();
        item.SubCategory = CategoryTypeConverter.ExtractSubCategory(pb.Category);
        item.AccountName = pb.AccountName;
        item.AccountNumber = pb.AccountNumber;
        item.Description = pb.Description;
        item.Amount = Convert.ToDouble(pb.Amount);
        item.Tags = pb.Tags;
        item.Notes = pb.Tags;
        item.TransactionUtcDate =  Convert.ToDateTime(pb.Date);
        item.Bank = pb.Bank;

        list.Add(item);
      }

      var sb = new StringBuilder();
      const string delimiter = "\t";

      var tmp = new BankStatementLineItem();
      sb.AppendLine(nameof(tmp.SubCategory) + delimiter + nameof(tmp.Description) + delimiter + nameof(tmp.Bank) +
                    delimiter + nameof(tmp.Amount) + delimiter + nameof(tmp.AccountName) + delimiter + nameof(tmp.Notes) +
                    delimiter + nameof(tmp.Tags));

      foreach (var item in list)
      {
        sb.AppendLine((item.SubCategory) + delimiter + (item.Description) + delimiter + (item.Bank) + delimiter +
                      (item.Amount) + delimiter + (item.AccountName) + delimiter + (item.Notes) + delimiter + (item.Tags));
      }

      return sb.ToString();
    }

    public static IEnumerable<BankStatementLineItem> Read(string contents)
    {
      using (var reader = new StringReader(contents))
      using (var csv = new CsvReader(reader))
      {
        csv.Configuration.Delimiter = "\t";
        csv.Configuration.HeaderValidated = null;
        csv.Configuration.HasHeaderRecord = true;
        csv.Configuration.IgnoreBlankLines = true;
        csv.Configuration.RegisterClassMap<BankStatementLineItemMap>();
        var records = csv.GetRecords<BankStatementLineItem>();
        return records.ToList();
      }

    } 
  }

  public sealed class BankStatementLineItemMap : ClassMap<BankStatementLineItem>
  {
    public BankStatementLineItemMap()
    {
      /// SubCategory	Description	Bank	Amount	AccountName	Notes	Tags
      Map(m => m.SubCategory).Index(0);
      Map(m => m.Description).Index(1);
      Map(m => m.Bank).Index(2);
      Map(m => m.Amount).Index(3);
      Map(m => m.AccountName).Index(4);
      Map(m => m.Notes).Index(5);
      Map(m => m.Tags).Index(6);
     
    }
  }

  public class SubCategoryTypeConverter :  DefaultTypeConverter
  {
    public SubCategoryTypeConverter()
    {
  
    }

    public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
      return CategoryTypeConverter.ExtractSubCategory(Convert.ToString(value));
    }
  }
}