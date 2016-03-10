using System;
using System.Web.Http;
using BeerTap.WebApi.Infrastructure;
using IQ.Foundation.Logging;

namespace BeerTap.WebApi
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private Lazy<ILog> _lazyLogger;

        ILog Logger
        {
            get { return _lazyLogger.Value; }
        }

        protected void Application_Start()
        {
            _lazyLogger = new Lazy<ILog>(LogManager.GetCurrentClassLogger);

            Logger.Info("BeerTap app is starting up.");
            BootStrapper.Initialize(GlobalConfiguration.Configuration);

            WarmUpDatabase();
            InitializeFilters();
        }

        void WarmUpDatabase()
        {
            var pingFactory = BootStrapper.ResolvePingFactory();

            Logger.Info("Warming up database.");

            var result = pingFactory.Create();

            Logger.Info(string.Format("ApplicationVersion: {0}, DbVersion: {1}, CompileTime: {2}", result.ApplicationVersion, result.DbVersion, result.CompileTime));
        }

        void InitializeFilters()
        {
            //GlobalConfiguration.Configuration.Filters.Add(new ValidateModelStateAttribute());
        }

        protected void Application_End(object sender, EventArgs e)
        {
            if (Logger != null)
                Logger.Info("BeerTap app is shutting down.");
        }
    }
}