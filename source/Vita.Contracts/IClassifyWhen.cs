using System;

namespace Vita.Contracts
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