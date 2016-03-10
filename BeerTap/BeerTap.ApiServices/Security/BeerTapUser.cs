using IQ.Platform.Framework.Common;
using IQ.Platform.Framework.WebApi.Services.Security;

namespace BeerTap.ApiServices.Security
{

    public class BeerTapUser : ApiUser<UserAuthData>
    {
        readonly int _userId;

        public BeerTapUser(int userId, Option<UserAuthData> authData)
            : base(authData)
        {
            _userId = userId;
        }

        public int UserId { get { return _userId; } }
    }

}
