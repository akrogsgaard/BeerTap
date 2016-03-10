using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BeerTap.ApiServices.RequestContext;
using BeerTap.DomainServices.Tap.Queries;
using BeerTap.Transport;
using IQ.Foundation.Logging;
using IQ.Platform.Framework.Common;
using IQ.Platform.Framework.Common.CQS;
using IQ.Platform.Framework.WebApi;
using ApiModel = BeerTap.Model;

namespace BeerTap.ApiServices.Tap
{
    public class TapApiService : IGetAResourceAsync<ApiModel.Tap, int>,
        IGetManyOfAResourceAsync<ApiModel.Tap, int>
    {
        private readonly IExtractDataFromARequestContext _requestContextExtractor;
        private readonly ITransportMapper<TapDto, ApiModel.Tap, int> _mapper;
        private readonly IAsyncQueryHandler<GetTapByIdQuery, Option<TapDto>> _getTapById;
        private readonly IAsyncQueryHandler<GetAllTapsByOfficeIdQuery, IEnumerable<TapDto>> _getAllTapsByOfficeId;
        
        private Lazy<ILog> _lazyLogger;

        ILog Logger
        {
            get { return _lazyLogger.Value; }
        }
        
        public TapApiService(
                IExtractDataFromARequestContext requestContextExtractor,
                ITransportMapper<TapDto, ApiModel.Tap, int> mapper,
                IAsyncQueryHandler<GetTapByIdQuery, Option<TapDto>> getTapById,
                IAsyncQueryHandler<GetAllTapsByOfficeIdQuery, IEnumerable<TapDto>> getAllTapsByOfficeId
            )
        {
            if (requestContextExtractor == null) throw new ArgumentNullException("requestContextExtractor");
            if (mapper == null) throw new ArgumentNullException("mapper");
            if (getTapById == null) throw new ArgumentNullException("getTapById");
            if (getAllTapsByOfficeId == null) throw new ArgumentNullException("getAllTapsByOfficeId");
            _requestContextExtractor = requestContextExtractor;
            _mapper = mapper;
            _getTapById = getTapById;
            _getAllTapsByOfficeId = getAllTapsByOfficeId;

            _lazyLogger = new Lazy<ILog>(LogManager.GetCurrentClassLogger);
        }

        public async Task<ApiModel.Tap> GetAsync(int id, IRequestContext context, CancellationToken cancellation)
        {
            _requestContextExtractor.ExtractOfficeId<ApiModel.Tap>(context);
            
            var option = await _getTapById.HandleAsync(new GetTapByIdQuery(id)).ConfigureAwait(false);
            var tapDto = option.EnsureValue();
            var tap = _mapper.MapToResource(tapDto);

            return tap;
        }

        public async Task<IEnumerable<ApiModel.Tap>> GetManyAsync(IRequestContext context, CancellationToken cancellation)
        {
            var officeId = _requestContextExtractor.ExtractOfficeId<ApiModel.Tap>(context);
            var tapDtos = await _getAllTapsByOfficeId.HandleAsync(new GetAllTapsByOfficeIdQuery(officeId));

            return tapDtos.Select(office => _mapper.MapToResource(office));
        }
    }
}