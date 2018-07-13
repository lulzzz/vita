using System;
using Vita.Contracts;

namespace Vita.Domain.Services.TextClassifiers
{
  public interface IClassifyWho
  {
    Company Who(string sentence);
  }

  public interface IClassifyWhat
  {
    TransactionType? What(TextClassificationResult result, string sentence);
  }

  public interface IClassifyWhere
  {
    Locality Where(string sentence);
  }

  public interface IClassifyWhen
  {
    DateTime? When(string sentence);
  }

  public interface IClassifyWhy
  {
    Classifier Why(string sentence);
  }

  public interface IClassifyHow
  {
    PaymentMethodType How(string sentence);
  }
}