using System;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using BeerTap.ApiServices.RequestContext;
using BeerTap.DomainServices.Keg.Commands;
using BeerTap.DomainServices.Tap.Commands;
using BeerTap.DomainServices.Tap.Queries;
using BeerTap.Model.Exceptions;
using BeerTap.Transport;
using IQ.Foundation.Logging;
using IQ.Platform.Framework.Common;
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
        private readonly IAsyncQueryHandler<GetTapByIdQuery, Option<TapDto>> _getTapById;
        private readonly IAsyncCommandHandler<UpdateTapCommand> _updateTap;

        private Lazy<ILog> _lazyLogger;

        ILog Logger
        {
            get { return _lazyLogger.Value; }
        }

        public ReplaceKegApiService(
                IExtractDataFromARequestContext requestContextExtractor,
                IAsyncCommandHandler<CreateKegCommand> createKeg,
                IAsyncCommandHandler<DeleteKegCommand> deleteKeg,
                IAsyncQueryHandler<GetTapByIdQuery, Option<TapDto>> getTapById,
                IAsyncCommandHandler<UpdateTapCommand> updateTap
            )
        {
            if (requestContextExtractor == null) throw new ArgumentNullException("requestContextExtractor");
            if (createKeg == null) throw new ArgumentNullException(nameof(createKeg));
            if (deleteKeg == null) throw new ArgumentNullException(nameof(deleteKeg));
            if (getTapById == null) throw new ArgumentNullException(nameof(getTapById));
            if (updateTap == null) throw new ArgumentNullException(nameof(updateTap));

            _requestContextExtractor = requestContextExtractor;
            _createKeg = createKeg;
            _deleteKeg = deleteKeg;
            _getTapById = getTapById;
            _updateTap = updateTap;

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

                var command = new CreateKegCommand(tapId, resource.BeerName, resource.Capacity, resource.Volume, userId);
                await _createKeg.HandleAsync(command).ConfigureAwait(false);

                var tapOption = await _getTapById.HandleAsync(new GetTapByIdQuery(tapId)).ConfigureAwait(false);
                var tapDto = tapOption.EnsureValue();

                //update the tap with the new KegId.
                await _updateTap.HandleAsync(new UpdateTapCommand(tapDto.Id, tapDto.OfficeId, resource.Id, ApiModel.KegState.Full.ToString(), userId)).ConfigureAwait(false);

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
