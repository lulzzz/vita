using System;
using Vita.Contracts;
using Microsoft.ML.Runtime.Api;

#pragma warning disable 649 // We don't care about unsused fields here, because they are mapped with the input file.

namespace Vita.Domain.BankStatements
{
  /// <summary>
  /// SubCategory	Description	Bank	Amount	AccountName	Notes	Tags
  /// </summary>
  public class BankStatementLineItem
  {
    [Column(ordinal: "0", name: "SubCategory")] public string SubCategory;

    [Column(ordinal: "1")] public string Description;

    [Column(ordinal: "2")] public string Bank;

    [Column(ordinal: "3")] public double Amount;

    [Column(ordinal: "4")] public string AccountName;

    [Column(ordinal: "5")] public string Notes;

    [Column(ordinal: "6")] public string Tags;










    public AccountType? AccountType { get; set; }
    public string AccountNumber { get; set; }
    public DateTime TransactionUtcDate  { get; set; }

    public static BankStatementLineItem ToBankStatementLineItem(PredictionRequest request)
    {
      var item = new BankStatementLineItem
      {
        Description = request.Description,
        Amount = request.Amount,
        Bank = request.Bank,
        //Tags = request.Tags,
        //Notes = request.Notes,
        AccountType = request.AccountType,
        //AccountName = request.AccountName,
        //AccountNumber = request.AccountName,
        TransactionUtcDate = request.TransactionUtcDate
      };
      return item;

    }
  }

 
}