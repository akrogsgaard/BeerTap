using System;
using IQ.Foundation.Logging;

namespace BeerTap.DataPersistance
{
    public class BeerTapContextFactory : IDbContextFactory<BeerTapContext>
    {
        private readonly Lazy<ILog> _lazyLogger;

        ILog Logger
        {
            get { return _lazyLogger.Value; }
        }

        public BeerTapContextFactory()
		{
            _lazyLogger = new Lazy<ILog>(LogManager.GetCurrentClassLogger);
        }

		public BeerTapContext CreateContext()
		{
			var context = new BeerTapContext();
            //context.Database.Log = x => Logger.Debug(x);
			return context;
		}
    }
}
