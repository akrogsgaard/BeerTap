using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace BeerTap.DataPersistance
{
    public static class DbContextUtils
    {
        public static string GetEntitySetName(this ObjectContext context, object entity)
        {
            var entityType = ObjectContext.GetObjectType(entity.GetType());
            if (entityType == null)
                throw new InvalidOperationException("not an entity");

            var container = context.MetadataWorkspace.GetEntityContainer(context.DefaultContainerName, DataSpace.CSpace);
            return (from entitySet in container.BaseEntitySets
                    where entitySet.ElementType.Name.Equals(entityType.Name)
                    select entitySet.Name).Single();

        }

        public static Exception ConvertDbUpdateException(DbUpdateException exception)
        {
            if (exception is DbUpdateConcurrencyException)
                return new DbUpdateConcurrencyException("The TaxPartner record has been updated since you last retrieved it.", exception);

            if (exception.InnerException != null && exception.InnerException.InnerException != null && exception.InnerException.InnerException is SqlException)
            {
                var innermost = (SqlException)exception.InnerException.InnerException;
                if (innermost.Number == 547 || innermost.Number == 2601)
                {
                    return new DbUpdateException("Error saving the record.", innermost);
                }
            }

            return exception;
        }
    }
}
