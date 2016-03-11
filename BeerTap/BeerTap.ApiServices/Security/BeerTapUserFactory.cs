using System;
using IQ.Auth.OAuth2.AccessTokenFormatting;
using IQ.Platform.Framework.Common;
using IQ.Platform.Framework.WebApi.AspNet;
using IQ.Platform.Framework.WebApi.Services.Security;

namespace BeerTap.ApiServices.Security
{
    public class BeerTapUserFactory : ApiUserFactory<BeerTapUser, UserAuthData>
    {
        const int DefaultUserId = -1;

        readonly IExtractDataFromAccessToken _extractUserIdFromAccessToken;

        public BeerTapUserFactory(IExtractDataFromAccessToken extractUserIdFromAccessToken)
            : base(new HttpRequestDataStore<UserAuthData>())
        {
            if (extractUserIdFromAccessToken == null) throw new ArgumentNullException(nameof(extractUserIdFromAccessToken));
            _extractUserIdFromAccessToken = extractUserIdFromAccessToken;
        }

        protected override BeerTapUser CreateUser(Option<UserAuthData> auth)
        {
            //var userId = auth.Select(x => _extractUserIdFromAccessToken.ExtractUserID(x.AccessToken))
            //                .ValueOrDefault(DefaultUserId);

            return new BeerTapUser(DefaultUserId, auth);
        }
    }
}
