using System;
using BeerTap.Transport;
using ApiModel = BeerTap.Model;

namespace BeerTap.ApiServices.Keg
{
    public class KegMapper : ITransportMapper<KegDto, ApiModel.Keg, int>
    {
        public ApiModel.Keg MapToResource(KegDto transport)
        {
            if (transport == null) throw new ArgumentNullException(nameof(transport));

            return new ApiModel.Keg()
                {
                    Id = transport.Id,
                    TapId = transport.TapId,
                    BeerName = transport.BeerName,
                    Capacity = transport.Capacity,
                    Volume = transport.Volume,
                };
        }

        public KegDto MapToTransport(ApiModel.Keg resource, Func<KegDto> transportCreator = null)
        {
            var transport = transportCreator != null
                              ? transportCreator()
                              : new KegDto();

            transport.Id = resource.Id;
            transport.TapId = resource.TapId;
            transport.BeerName = resource.BeerName;
            transport.Capacity = resource.Capacity;
            transport.Volume = resource.Volume;

            return transport;
        }
    }
}
