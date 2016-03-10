using IQ.Auth.OAuth2.AccessTokenFormatting;
using IQ.Platform.Framework.WebApi.AspNet;
using IQ.Platform.Framework.WebApi.AspNet.Handlers;
using IQ.Platform.Framework.WebApi.Services.Security;
using BeerTap.ApiServices.Security;

namespace BeerTap.WebApi.Handlers
{
    public class BeerTapSecurityContextProvidingHandler : ApiSecurityContextProvidingHandler<BeerTapUser, NullUserContext>
    {
        public BeerTapSecurityContextProvidingHandler(IStoreDataInHttpRequest<BeerTapUser> apiUserInRequestStore, IExtractDataFromAccessToken extractUserIDFromAccess)
            : base(new BeerTapUserFactory(extractUserIDFromAccess), CreateContextProvider(), apiUserInRequestStore)
        {
        }

        static ApiUserContextProvider<BeerTapUser, NullUserContext> CreateContextProvider()
        {
            return new ApiUserContextProvider<BeerTapUser, NullUserContext>(_ => new NullUserContext());
        }
    }
}