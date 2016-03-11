using System;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using BeerTap.ApiServices.RequestContext;
using BeerTap.DomainServices.Keg.Commands;
using BeerTap.DomainServices.Keg.Queries;
using BeerTap.DomainServices.Tap.Commands;
using BeerTap.DomainServices.Tap.Queries;
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
        private readonly IAsyncQueryHandler<GetTapByIdQuery, Option<TapDto>> _getTapById;
        private readonly IAsyncCommandHandler<UpdateTapCommand> _updateTap;

        private Lazy<ILog> _lazyLogger;

        ILog Logger
        {
            get { return _lazyLogger.Value; }
        }

        public PullBeerApiService(
                IExtractDataFromARequestContext requestContextExtractor,
                IAsyncCommandHandler<UpdateKegCommand> updateKeg,
                IAsyncQueryHandler<GetKegByTapIdQuery, Option<KegDto>> getKegByTapId, 
                IAsyncQueryHandler<GetTapByIdQuery, Option<TapDto>> getTapById,
                IAsyncCommandHandler<UpdateTapCommand> updateTap
            )
        {
            if (requestContextExtractor == null) throw new ArgumentNullException(nameof(requestContextExtractor));
            if (updateKeg == null) throw new ArgumentNullException(nameof(updateKeg));
            if (getKegByTapId == null) throw new ArgumentNullException(nameof(getKegByTapId));
            if (getTapById == null) throw new ArgumentNullException(nameof(getTapById));
            if (updateTap == null) throw new ArgumentNullException(nameof(updateTap));

            _requestContextExtractor = requestContextExtractor;
            _updateKeg = updateKeg;
            _getKegByTapId = getKegByTapId;
            _getTapById = getTapById;
            _updateTap = updateTap;

            _lazyLogger = new Lazy<ILog>(LogManager.GetCurrentClassLogger);
        }

        public async Task<ResourceCreationResult<ApiModel.SupportResources.PullBeer, int>> CreateAsync(ApiModel.SupportResources.PullBeer resource, IRequestContext context, CancellationToken cancellation)
        {
            if (resource == null) throw new ArgumentNullException(nameof(resource));
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                var userId = _requestContextExtractor.ExtractUserIdFromRequest(context);

                var option = await _getKegByTapId.HandleAsync(new GetKegByTapIdQuery(resource.TapId)).ConfigureAwait(false);
                var kegDto = option.EnsureValue(() => context.CreateNotFoundHttpResponseException<ApiModel.Keg>());

                if (resource.Volume > kegDto.Volume)
                    resource.Volume = kegDto.Volume;

                kegDto.Volume = kegDto.Volume - resource.Volume;

                var command = new UpdateKegCommand(kegDto.Id, kegDto.TapId, kegDto.BeerName, kegDto.Capacity, kegDto.Volume, userId);
                await _updateKeg.HandleAsync(command).ConfigureAwait(false);

                var tapOption = await _getTapById.HandleAsync(new GetTapByIdQuery(kegDto.TapId)).ConfigureAwait(false);
                var tapDto = tapOption.EnsureValue();

                var kegState = GetKegState(tapDto.KegState, kegDto);

                //update the kegState in the tap.
                await _updateTap.HandleAsync(new UpdateTapCommand(tapDto.Id, tapDto.OfficeId, kegDto.Id, kegState, userId)).ConfigureAwait(false);

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

        private string GetKegState(string state, KegDto kegDto)
        {
            var lowVolumeLimit = kegDto.Capacity * .25;

            if (state == ApiModel.KegState.Full.ToString())
                return ApiModel.KegState.GoingDown.ToString();

            if(kegDto.Volume > lowVolumeLimit)
                return ApiModel.KegState.GoingDown.ToString();

            if (kegDto.Volume < lowVolumeLimit && kegDto.Volume > 0)
                return ApiModel.KegState.AlmostEmpty.ToString();

            return ApiModel.KegState.Empty.ToString();
        }
    }
}
