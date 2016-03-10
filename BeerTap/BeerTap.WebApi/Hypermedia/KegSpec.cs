using System.Collections.Generic;
using BeerTap.ApiServices;
using IQ.Platform.Framework.WebApi.Hypermedia;
using IQ.Platform.Framework.WebApi.Hypermedia.Specs;
using IQ.Platform.Framework.WebApi.Model.Hypermedia;
using ApiModel = BeerTap.Model;

namespace BeerTap.WebApi.Hypermedia
{
    public class KegSpec : SingleStateResourceSpec<ApiModel.Keg, int>
    {
        public static ResourceUriTemplate Uri = ResourceUriTemplate.Create("Offices({officeId})/Taps({tapId})/Kegs({id})");

        // custom self link
        protected override IEnumerable<ResourceLinkTemplate<ApiModel.Keg>> Links()
        {
            yield return CreateLinkTemplate<LinkParameters>(CommonLinkRelations.Self, Uri, x => x.Parameters.OfficeId, x => x.Resource.TapId, x => x.Resource.Id);
        }

        public override IResourceStateSpec<ApiModel.Keg, NullState, int> StateSpec
        {
            get
            {
                return
                    new SingleStateSpec<ApiModel.Keg, int>
                    {
                        Links =
                            {
                                CreateLinkTemplate<LinkParameters>(ApiModel.LinkRelations.Office, OfficeSpec.Uri, x => x.Parameters.OfficeId),
                                CreateLinkTemplate<LinkParameters>(ApiModel.LinkRelations.Tap, TapSpec.Uri, x => x.Parameters.OfficeId, x => x.Resource.Id),
                            },

                        Operations =
                            {
                                Get = ServiceOperations.Get,
                                InitialPost = ServiceOperations.Create,
                                Post = ServiceOperations.Update,
                                Delete = ServiceOperations.Delete,
                            },
                    };
            }
        }
    }
}