using System;
using BeerTap.Transport;
using ApiModel = BeerTap.Model;

namespace BeerTap.ApiServices.Office
{
    public class OfficeMapper : ITransportMapper<OfficeDto, ApiModel.Office, int>
    {
        public ApiModel.Office MapToResource(OfficeDto transport)
        {
            if (transport == null) throw new ArgumentNullException("transport");

            return new ApiModel.Office
                {
                    Id = transport.Id,
                    Name = transport.Name
                };
        }

        public OfficeDto MapToTransport(ApiModel.Office resource, Func<OfficeDto> transportCreator = null)
        {
            var transport = transportCreator != null
                              ? transportCreator()
                              : new OfficeDto();

            transport.Id = resource.Id;
            transport.Name = resource.Name;

            return transport;
        }
    }
}
