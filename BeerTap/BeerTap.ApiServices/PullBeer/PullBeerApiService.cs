using System;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using BeerTap.ApiServices.RequestContext;
using BeerTap.DomainServices.Keg.Commands;
using BeerTap.DomainServices.Keg.Queries;
using BeerTap.Model.Exceptions;
using BeerTap.Transport;
using IQ.Foundation.Logging;
using IQ.Platform.Framework.Common;
using IQ.Platform.Framework.Common.CQS;
using IQ.Platform.Framework.WebApi;
using ApiModel = BeerTap.Model;

namespace BeerTap.ApiServices.PullBeer
{
    public class PullBeerApiService : ICreateAResourceAsync<ApiModel.SupportResources.PullBeer, int>
    {
        private readonly IExtractDataFromARequestContext _requestContextExtractor;
        private readonly IAsyncCommandHandler<UpdateKegCommand> _updateKeg;
        private readonly IAsyncQueryHandler<GetKegByTapIdQuery, Option<KegDto>> _getKegByTapId;

        private Lazy<ILog> _lazyLogger;

        ILog Logger
        {
            get { return _lazyLogger.Value; }
        }

        public PullBeerApiService(
                IExtractDataFromARequestContext requestContextExtractor,
                IAsyncCommandHandler<UpdateKegCommand> updateKeg,
                IAsyncQueryHandler<GetKegByTapIdQuery, Option<KegDto>> getKegByTapId
            )
        {
            if (requestContextExtractor == null) throw new ArgumentNullException("requestContextExtractor");
            if (updateKeg == null) throw new ArgumentNullException("updateKeg");
            if (getKegByTapId == null) throw new ArgumentNullException("getKegByTapId");

            _requestContextExtractor = requestContextExtractor;
            _updateKeg = updateKeg;
            _getKegByTapId = getKegByTapId;

            _lazyLogger = new Lazy<ILog>(LogManager.GetCurrentClassLogger);
        }

        public async Task<ResourceCreationResult<ApiModel.SupportResources.PullBeer, int>> CreateAsync(ApiModel.SupportResources.PullBeer resource, IRequestContext context, CancellationToken cancellation)
        {
            if (resource == null) throw new ArgumentNullException("resource");
            if (context == null) throw new ArgumentNullException("context");

            try
            {
                var userId = _requestContextExtractor.ExtractUserIdFromRequest(context);

                var option = await _getKegByTapId.HandleAsync(new GetKegByTapIdQuery(resource.TapId)).ConfigureAwait(false);
                var kegDto = option.EnsureValue(() => context.CreateNotFoundHttpResponseException<ApiModel.Keg>());

                if (resource.Volume > kegDto.Volume)
                    resource.Volume = kegDto.Volume;

                kegDto.Volume = kegDto.Volume - resource.Volume;
                kegDto.State = GetKegState(kegDto);

                var command = new UpdateKegCommand(kegDto.Id, kegDto.TapId, kegDto.State, kegDto.BeerName, kegDto.Capacity, kegDto.Volume, userId);
                await _updateKeg.HandleAsync(command).ConfigureAwait(false);

                return new ResourceCreationResult<ApiModel.SupportResources.PullBeer, int>(resource);
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

        private string GetKegState(KegDto kegDto)
        {
            var lowVolumeLimit = kegDto.Capacity * .25;

            if (kegDto.State == ApiModel.KegState.Full.ToString())
                return ApiModel.KegState.GoingDown.ToString();

            if(kegDto.Volume > lowVolumeLimit)
                return ApiModel.KegState.GoingDown.ToString();

            if (kegDto.Volume < lowVolumeLimit && kegDto.Volume > 0)
                return ApiModel.KegState.AlmostEmpty.ToString();

            return ApiModel.KegState.Empty.ToString();
        }
    }
}
