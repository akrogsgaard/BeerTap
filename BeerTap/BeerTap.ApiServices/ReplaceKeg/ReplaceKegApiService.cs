using System;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using BeerTap.ApiServices.RequestContext;
using BeerTap.DomainServices.Keg.Commands;
using BeerTap.Model.Exceptions;
using IQ.Foundation.Logging;
using IQ.Platform.Framework.Common.CQS;
using IQ.Platform.Framework.WebApi;
using ApiModel = BeerTap.Model;

namespace BeerTap.ApiServices.ReplaceKeg
{
    public class ReplaceKegApiService : ICreateAResourceAsync<ApiModel.SupportResources.ReplaceKeg, int>
    {
        private readonly IExtractDataFromARequestContext _requestContextExtractor;
        private readonly IAsyncCommandHandler<CreateKegCommand> _createKeg;
        private readonly IAsyncCommandHandler<DeleteKegCommand> _deleteKeg;

        private Lazy<ILog> _lazyLogger;

        ILog Logger
        {
            get { return _lazyLogger.Value; }
        }

        public ReplaceKegApiService(
                IExtractDataFromARequestContext requestContextExtractor,
                IAsyncCommandHandler<CreateKegCommand> createKeg,
                IAsyncCommandHandler<DeleteKegCommand> deleteKeg
            )
        {
            if (requestContextExtractor == null) throw new ArgumentNullException("requestContextExtractor");
            if (createKeg == null) throw new ArgumentNullException(nameof(createKeg));
            if (deleteKeg == null) throw new ArgumentNullException(nameof(deleteKeg));

            _requestContextExtractor = requestContextExtractor;
            _createKeg = createKeg;
            _deleteKeg = deleteKeg;

            _lazyLogger = new Lazy<ILog>(LogManager.GetCurrentClassLogger);
        }

        public async Task<ResourceCreationResult<ApiModel.SupportResources.ReplaceKeg, int>> CreateAsync(ApiModel.SupportResources.ReplaceKeg resource, IRequestContext context, CancellationToken cancellation)
        {
            if (resource == null) throw new ArgumentNullException("resource");
            if (context == null) throw new ArgumentNullException("context");

            try
            {
                var userId = _requestContextExtractor.ExtractUserIdFromRequest(context);
                var tapId = _requestContextExtractor.ExtractTapId<ApiModel.SupportResources.ReplaceKeg>(context);

                await _deleteKeg.HandleAsync(new DeleteKegCommand(resource.Id, userId));

                var command = new CreateKegCommand(tapId, ApiModel.KegState.Full.ToString(), resource.BeerName, resource.Capacity, resource.Volume, userId);
                await _createKeg.HandleAsync(command).ConfigureAwait(false);

                return new ResourceCreationResult<ApiModel.SupportResources.ReplaceKeg, int>(resource);
            }
            catch (BeerTapServiceException ex)
            {
                throw context.CreateHttpResponseException<ApiModel.Office>(ex.Message, ex.StatusCode);
            }
            catch (DbUpdateException ex)
            {
                throw context.CreateHttpResponseException<ApiModel.Office>(ex.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                throw context.CreateHttpResponseException<ApiModel.Office>(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
