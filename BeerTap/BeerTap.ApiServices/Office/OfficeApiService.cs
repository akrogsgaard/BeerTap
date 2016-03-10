using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using BeerTap.ApiServices.RequestContext;
using BeerTap.DomainServices.Office.Queries;
using BeerTap.Transport;
using IQ.Foundation.Logging;
using IQ.Platform.Framework.Common;
using IQ.Platform.Framework.Common.CQS;
using IQ.Platform.Framework.WebApi;
using ApiModel = BeerTap.Model;

namespace BeerTap.ApiServices.Office
{
    public class OfficeApiService : IGetAResourceAsync<ApiModel.Office, int>,
        IGetManyOfAResourceAsync<ApiModel.Office, int>
    {
        private readonly IExtractDataFromARequestContext _requestContextExtractor;
        private readonly ITransportMapper<OfficeDto, ApiModel.Office, int> _mapper;
        private readonly IAsyncQueryHandler<GetOfficeByIdQuery, Option<OfficeDto>> _getOfficeById;
        private readonly IAsyncQueryHandler<GetOfficeByNameQuery, IEnumerable<OfficeDto>> _getOfficeByName;
        private readonly IAsyncQueryHandler<GetAllOfficesQuery, IEnumerable<OfficeDto>> _getAllOffices;

        private Lazy<ILog> _lazyLogger;

        ILog Logger
        {
            get { return _lazyLogger.Value; }
        }

        public OfficeApiService(
                IExtractDataFromARequestContext requestContextExtractor,
                ITransportMapper<OfficeDto, ApiModel.Office, int> mapper,
                IAsyncQueryHandler<GetOfficeByIdQuery, Option<OfficeDto>> getOfficeById,
                IAsyncQueryHandler<GetOfficeByNameQuery, IEnumerable<OfficeDto>> getOfficeByName,
                IAsyncQueryHandler<GetAllOfficesQuery, IEnumerable<OfficeDto>> getAllOffices
            )
        {
            if (requestContextExtractor == null) throw new ArgumentNullException("requestContextExtractor");
            if (mapper == null) throw new ArgumentNullException("mapper");
            if (getAllOffices == null) throw new ArgumentNullException("getAllOffices");
            _requestContextExtractor = requestContextExtractor;
            _mapper = mapper;
            _getOfficeById = getOfficeById;
            _getOfficeByName = getOfficeByName;
            _getAllOffices = getAllOffices;

            _lazyLogger = new Lazy<ILog>(LogManager.GetCurrentClassLogger);
        }

        public async Task<ApiModel.Office> GetAsync(int id, IRequestContext context, CancellationToken cancellation)
        {
            var option = await _getOfficeById.HandleAsync(new GetOfficeByIdQuery(id)).ConfigureAwait(false);
            var officeDto = option.EnsureValue();
            var office = _mapper.MapToResource(officeDto);

            return office;
        }

        public async Task<IEnumerable<ApiModel.Office>> GetManyAsync(IRequestContext context, CancellationToken cancellation)
        {
            IEnumerable<OfficeDto> officeDtos = null;

            try
            {
                //FILTER BY NAME
                var nameFilter = context.GetFilter<ApiModel.Office>().GetSubstringValueFor(b => b.Name);
                if (!string.IsNullOrEmpty(nameFilter))
                {
                    var query = new GetOfficeByNameQuery(nameFilter);
                    officeDtos = await _getOfficeByName.HandleAsync(query).ConfigureAwait(false);
                }

                //DEFAULT RETURN ALL OFFICES
                if (officeDtos == null)
                {
                    officeDtos = await _getAllOffices.HandleAsync(new GetAllOfficesQuery());
                }
            }
            catch (Exception ex)
            {
                if (ex is HttpException)
                    throw context.CreateHttpResponseException<ApiModel.Office>(ex.Message, (HttpStatusCode)ex.HResult);
                throw;
            }

            return officeDtos.Select(office => _mapper.MapToResource(office));
        }
    }
}
