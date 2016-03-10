﻿using System.Collections.Generic;
using BeerTap.ApiServices;
using BeerTap.Model;
using IQ.Platform.Framework.WebApi.Hypermedia;
using IQ.Platform.Framework.WebApi.Hypermedia.Specs;
using IQ.Platform.Framework.WebApi.Model.Hypermedia;
using ApiModel = BeerTap.Model.SupportResources;

namespace BeerTap.WebApi.Hypermedia
{
    public class PullBeerSpec : SingleStateResourceSpec<ApiModel.PullBeer, int>
    {
        public static ResourceUriTemplate Uri = ResourceUriTemplate.Create("Offices({officeId})/Taps({tapId})/PullBeer");

        // custom self link
        protected override IEnumerable<ResourceLinkTemplate<ApiModel.PullBeer>> Links()
        {
            yield return CreateLinkTemplate<LinkParameters>(CommonLinkRelations.Self, Uri, x => x.Parameters.OfficeId,  x => x.Resource.TapId);
        }

        public override IResourceStateSpec<ApiModel.PullBeer, NullState, int> StateSpec
        {
            get
            {
                return
                    new SingleStateSpec<ApiModel.PullBeer, int>
                    {
                        Links =
                            {
                                CreateLinkTemplate<LinkParameters>(LinkRelations.Tap, TapSpec.Uri, i => i.Parameters.OfficeId, i => i.Resource.TapId),
                            },

                        Operations =
                            {
                                InitialPost = ServiceOperations.Create,
                            },
                    };
            }
        }
    }
}