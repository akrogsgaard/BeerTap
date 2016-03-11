using System;
using System.Net.Http.Headers;
using IQ.Platform.Framework.Common;

namespace BeerTap.ApiServices.RequestContext
{
    public static class HttpRequestHeadersExtensions
    {
        public static Option<EntityTagHeaderValue> IfMatchAsOption(this HttpRequestHeaders requestHeaders)
        {
            if (requestHeaders == null) throw new ArgumentNullException(nameof(requestHeaders));

            return requestHeaders.IfMatch == null ?
                       Option.None<EntityTagHeaderValue>() :
                       requestHeaders.IfMatch.FirstAsOption();
        }
    }
}
