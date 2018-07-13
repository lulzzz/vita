using Vita.Contracts;

namespace Vita.Domain.Infrastructure
{
    public class Data
    {
      public IRepository<Classifier> Classifiers = new Repository<Classifier>();
      public IRepository<Company> Companies = new Repository<Company>();
      public IRepository<Locality> Localities = new Repository<Locality>();
  }
}
