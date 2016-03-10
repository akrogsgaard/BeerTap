using IQ.Platform.Framework.WebApi;

namespace BeerTap.ApiServices.RequestContext
{
    public interface IExtractDataFromARequestContext
    {
        int ExtractOfficeId<TResource>(IRequestContext context) where TResource : class;
        int ExtractTapId<TResource>(IRequestContext context) where TResource : class;
        int ExtractETag<TResource>(IRequestContext context) where TResource : class;
        int ExtractUserIdFromRequest(IRequestContext context);
        string ExtractTokenFromRequest<TResource>(IRequestContext context) where TResource : class;
    }
}
