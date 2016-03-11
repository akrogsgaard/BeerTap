using System;
using BeerTap.Transport;
using IQ.Platform.Framework.Common;
using ApiModel = BeerTap.Model;

namespace BeerTap.ApiServices.Tap
{
    public class TapMapper : ITransportMapper<TapDto, ApiModel.Tap, int>
    {
        public ApiModel.Tap MapToResource(TapDto transport)
        {
            if (transport == null) throw new ArgumentNullException(nameof(transport));

            var stateOption = Option.None<ApiModel.KegState>();

            if (transport.KegId > 0)
                stateOption = transport.KegState.ConvertTo<ApiModel.KegState>();

            return new ApiModel.Tap
                {
                    Id = transport.Id,
                    OfficeId = transport.OfficeId,
                    KegId = transport.KegId,
                    KegState = stateOption.HasValue ? stateOption.Value : ApiModel.KegState.Empty,
                };
        }

        public TapDto MapToTransport(ApiModel.Tap resource, Func<TapDto> transportCreator = null)
        {
            var transport = transportCreator != null
                              ? transportCreator()
                              : new TapDto();

            transport.Id = resource.Id;
            transport.OfficeId = resource.OfficeId;
            transport.KegId = resource.KegId;

            return transport;
        }
    }
}
