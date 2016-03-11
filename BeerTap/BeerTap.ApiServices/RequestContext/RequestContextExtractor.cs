using System;
using System.Net;
using System.Net.Http.Headers;
using BeerTap.ApiServices.Security;
using IQ.Platform.Framework.Common;
using IQ.Platform.Framework.WebApi;
using IQ.Platform.Framework.WebApi.AspNet;

namespace BeerTap.ApiServices.RequestContext
{
    public class RequestContextExtractor : IExtractDataFromARequestContext
    {
        private readonly IGetDataFromHttpRequest<BeerTapUser> _getApiUserFromHttpRequest;

        public RequestContextExtractor(IGetDataFromHttpRequest<BeerTapUser> getApiUserFromHttpRequest)
        {
            if (getApiUserFromHttpRequest == null) throw new ArgumentNullException(nameof(getApiUserFromHttpRequest));
            _getApiUserFromHttpRequest = getApiUserFromHttpRequest;
        }

        public int ExtractOfficeId<TResource>(IRequestContext context) where TResource : class
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var option = context.UriParameters.GetByName<int>("officeId");
            var officeId = option.EnsureValue(() => context.CreateHttpResponseException<TResource>("Cannot find office identifier in the uri", HttpStatusCode.BadRequest));
            context.LinkParameters.Set(new LinkParameters(officeId));

            return officeId;
        }

        public int ExtractTapId<TResource>(IRequestContext context) where TResource : class
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var option = context.UriParameters.GetByName<int>("tapId");
            var tapId = option.EnsureValue(() => context.CreateHttpResponseException<TResource>("Cannot find tap identifier in the uri", HttpStatusCode.BadRequest));
            context.LinkParameters.Set(new LinkParameters(tapId));

            return tapId;
        }

        public int ExtractETag<TResource>(IRequestContext context) where TResource : class
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var entityTagHeaderValue = GetEntityTagHeaderValue<TResource>(context);

            string trimmedETag = entityTagHeaderValue.ToString().Trim('"');

            int integerETag;

            if (!int.TryParse(trimmedETag, out integerETag))
                throw context.CreateHttpResponseException<TResource>("The ETag value must be an integer.", HttpStatusCode.BadRequest);

            return integerETag;
        }

        public int ExtractUserIdFromRequest(IRequestContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            // Try to get the UserId from the TaxApiUser. If we can't find it return -1
            var userId = _getApiUserFromHttpRequest.Get(context.Request).Select(x => x.UserId).ValueOrDefault(-1);
            return userId;
        }

        public string ExtractTokenFromRequest<TResource>(IRequestContext context) where TResource : class
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var tokenOption = _getApiUserFromHttpRequest.Get(context.Request).SelectMany(user => user.Authentication).Select(x => x.AccessToken);
            var token = tokenOption.EnsureValue(() => context.CreateHttpResponseException<TResource>("Could not extract authorization token from request.", HttpStatusCode.InternalServerError));

            return token;
        }

        EntityTagHeaderValue GetEntityTagHeaderValue<TResource>(IRequestContext context) where TResource : class
        {
            var ifMatch = context.Request.Headers.IfMatchAsOption();

            if (!ifMatch.HasValue || ifMatch.Value.Equals(EntityTagHeaderValue.Any))
                throw context.CreateHttpResponseException<TResource>("An ETag value must be provided in the If-Match header and it cannot be the wildcard character (*).", HttpStatusCode.BadRequest);

            var entityTagHeaderValue = ifMatch.Value;
            return entityTagHeaderValue;
        }
    }
}
