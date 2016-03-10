using System;
using System.Threading;
using System.Threading.Tasks;
using IQ.Platform.Framework.Common.CQS;

namespace BeerTap.DataPersistance.Version
{
    public class GetDbVersionQueryHandler : IAsyncQueryHandler<GetDbVersion, string>
    {
        readonly IDbContextFactory<BeerTapContext> _contextFactory;

        public GetDbVersionQueryHandler(IDbContextFactory<BeerTapContext> contextFactory)
        {
            if (contextFactory == null) throw new ArgumentNullException("contextFactory");

            _contextFactory = contextFactory;
        }

        public async Task<string> HandleAsync(GetDbVersion query, CancellationToken cancellationToken = new CancellationToken())
        {
            using (var context = _contextFactory.CreateContext())
            {
                var result = await context.Database
                                        .SqlQuery<string>("select max(MigrationId) MigrationId from __MigrationHistory")
                                        .FirstOrDefaultAsync().ConfigureAwait(false);

                return ExtractMigrationDate(result);
            }
        }

        string ExtractMigrationDate(string result)
        {
            if (string.IsNullOrWhiteSpace(result) || result.Length < 15)
                return string.Empty;

            // The migrationId value is in the format: 201503112005476_InitializeAndSeed
            // The first 15 chars should give us the date and time of the migration
            return result.Substring(0, 15);
        }
    }
}
