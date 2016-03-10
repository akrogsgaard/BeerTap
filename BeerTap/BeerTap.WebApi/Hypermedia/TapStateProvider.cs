using System.Collections.Generic;
using IQ.Platform.Framework.Common;
using IQ.Platform.Framework.WebApi.Hypermedia;
using ApiModel = BeerTap.Model;

namespace BeerTap.WebApi.Hypermedia
{
    public class TapStateProvider : ResourceStateProviderBase<ApiModel.Tap, ApiModel.KegState>
    {
        public override ApiModel.KegState GetFor(ApiModel.Tap resource)
        {
            return resource.KegState;
        }

        protected override IDictionary<ApiModel.KegState, IEnumerable<ApiModel.KegState>> GetTransitions()
        {
            return new Dictionary<ApiModel.KegState, IEnumerable<ApiModel.KegState>>
                {
                    // from --> to
                    {ApiModel.KegState.Full, new []
                    {
                        ApiModel.KegState.GoingDown
                    }},

                    {ApiModel.KegState.GoingDown, new []
                    {
                        ApiModel.KegState.AlmostEmpty
                    }},

                    {ApiModel.KegState.AlmostEmpty, new []
                    {
                        ApiModel.KegState.Empty
                    }},
                };
        }

        public override IEnumerable<ApiModel.KegState> All
        {
            get { return EnumEx.GetValuesFor<ApiModel.KegState>(); }
        }
    }
}