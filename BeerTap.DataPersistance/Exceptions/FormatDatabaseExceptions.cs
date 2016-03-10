using System.Data.Entity.Validation;
using System.Linq;
using IQ.Platform.EntityFrameworkEx.Exceptions;

namespace BeerTap.DataPersistance.Exceptions
{
    public class FormatDatabaseExceptions : IFormatDatabaseExceptions
    {
        public string AsErrorMessage(DbEntityValidationException exception)
        {
            // Retrieve the error messages as a list of strings.
            var errorMessages = exception.EntityValidationErrors
                                .SelectMany(x => x.ValidationErrors)
                                .Select(x => x.ErrorMessage);

            // Join the list to a single string.
            var fullErrorMessage = string.Join("; ", errorMessages);

            // Combine the original exception message with the new one.
            return string.Format("The following data validations failed: {0}", fullErrorMessage);
        }
    }
}
