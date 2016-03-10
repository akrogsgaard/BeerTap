using System.Data.Entity;

namespace BeerTap.DataPersistance
{
    public interface IDbContextFactory<T> where T : DbContext
    {
        T CreateContext();
    }
}