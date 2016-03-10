using System;
using System.Collections.Generic;
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
using Newtonsoft.Json;
using ApiModel = BeerTap.Model;

namespace BeerTap.ApiServices.Keg
{
    public class KegApiService : IGetAResourceAsync<ApiModel.Keg, int>,
        IGetManyOfAResourceAsync<ApiModel.Keg, int>,
        ICreateAResourceAsync<ApiModel.Keg, int>,
        IDeleteResourceAsync<ApiModel.Keg, int>

    {
        private readonly IExtractDataFromARequestContext _requestContextExtractor;
        private readonly ITransportMapper<KegDto, ApiModel.Keg, int> _mapper;
        private readonly ICreateKegCommandHandler _createKeg;
        private readonly IAsyncCommandHandler<DeleteKegCommand> _deleteKeg;
        private readonly IAsyncQueryHandler<GetKegByIdQuery, Option<KegDto>> _getKegById;
        private readonly IAsyncQueryHandler<GetKegByTapIdQuery, Option<KegDto>> _getKegByTapId;
        private readonly IAsyncQueryHandler<GetTapByIdQuery, Option<TapDto>> _getTapById;
        private readonly IAsyncCommandHandler<UpdateTapCommand> _updateTap;

        private Lazy<ILog> _lazyLogger;

        ILog Logger
        {
            get { return _lazyLogger.Value; }
        }

        public KegApiService(
                IExtractDataFromARequestContext requestContextExtractor,
                ITransportMapper<KegDto, ApiModel.Keg, int> mapper,
                ICreateKegCommandHandler createKeg,
                IAsyncCommandHandler<DeleteKegCommand> deleteKeg,
                IAsyncQueryHandler<GetKegByIdQuery, Option<KegDto>> getKegById,
                IAsyncQueryHandler<GetKegByTapIdQuery, Option<KegDto>> getKegByTapId,
                IAsyncQueryHandler<GetTapByIdQuery, Option<TapDto>> getTapById,
                IAsyncCommandHandler<UpdateTapCommand> updateTap
            )
        {
            if (requestContextExtractor == null) throw new ArgumentNullException("requestContextExtractor");
            if (mapper == null) throw new ArgumentNullException("mapper");
            if (createKeg == null) throw new ArgumentNullException("createKeg");
            if (deleteKeg == null) throw new ArgumentNullException("deleteKeg");
            if (getKegById == null) throw new ArgumentNullException("getKegById");
            if (getKegByTapId == null) throw new ArgumentNullException("getAllKegsByOfficeId");
            if (getTapById == null) throw new ArgumentNullException(nameof(getTapById));
            if (updateTap == null) throw new ArgumentNullException(nameof(updateTap));
            _requestContextExtractor = requestContextExtractor;
            _mapper = mapper;
            _createKeg = createKeg;
            _deleteKeg = deleteKeg;
            _getKegById = getKegById;
            _getKegByTapId = getKegByTapId;
            _getTapById = getTapById;
            _updateTap = updateTap;

            _lazyLogger = new Lazy<ILog>(LogManager.GetCurrentClassLogger);
        }

        public async Task<ResourceCreationResult<ApiModel.Keg, int>> CreateAsync(ApiModel.Keg resource, IRequestContext context, CancellationToken cancellation)
        {
            if (resource == null) throw new ArgumentNullException("resource");
            if (context == null) throw new ArgumentNullException("context");

            try
            {
                var userId = _requestContextExtractor.ExtractUserIdFromRequest(context);
                var tapId = _requestContextExtractor.ExtractTapId<ApiModel.Keg>(context);

                var command = new CreateKegCommand(tapId, resource.BeerName, resource.Capacity, resource.Volume, userId);

                Logger.Debug(string.Format("Creating Keg record: {0}", JsonConvert.SerializeObject(resource)));

                resource.Id = await _createKeg.HandleCustomAsync(command);

                var keg = await GetAsync(resource.Id, context, cancellation).ConfigureAwait(false);

                var tapOption = await _getTapById.HandleAsync(new GetTapByIdQuery(tapId)).ConfigureAwait(false);
                var tapDto = tapOption.EnsureValue();

                //update the tap with the new KegId.
                await _updateTap.HandleAsync(new UpdateTapCommand(tapDto.Id, tapDto.OfficeId, keg.Id, ApiModel.KegState.Full.ToString(), userId)).ConfigureAwait(false);
                
                return new ResourceCreationResult<ApiModel.Keg, int>(keg);
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

        public async Task<ApiModel.Keg> GetAsync(int id, IRequestContext context, CancellationToken cancellation)
        {
            var option = await _getKegById.HandleAsync(new GetKegByIdQuery(id)).ConfigureAwait(false);
            var kegDto = option.EnsureValue(() => context.CreateNotFoundHttpResponseException<ApiModel.Keg>());
            var keg = _mapper.MapToResource(kegDto);

            return keg;
        }

        public async Task<IEnumerable<ApiModel.Keg>> GetManyAsync(IRequestContext context, CancellationToken cancellation)
        {
            var kegs = new List<ApiModel.Keg>();

            var tapId = _requestContextExtractor.ExtractTapId<ApiModel.Keg>(context);
            var option = await _getKegByTapId.HandleAsync(new GetKegByTapIdQuery(tapId)).ConfigureAwait(false);
            var kegDto = option.EnsureValue(() => context.CreateNotFoundHttpResponseException<ApiModel.Keg>());

            kegs.Add(_mapper.MapToResource(kegDto));

            return kegs;
        }

        public async Task DeleteAsync(ResourceOrIdentifier<ApiModel.Keg, int> input, IRequestContext context, CancellationToken cancellation)
        {
            if (context == null) throw new ArgumentNullException("context");

            try
            {
                var userId = _requestContextExtractor.ExtractUserIdFromRequest(context);

                await _deleteKeg.HandleAsync(new DeleteKegCommand(input.Id, userId));
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
