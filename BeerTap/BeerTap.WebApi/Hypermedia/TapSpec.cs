using System.Collections.Generic;
using BeerTap.ApiServices;
using IQ.Platform.Framework.WebApi.Hypermedia;
using IQ.Platform.Framework.WebApi.Hypermedia.Specs;
using IQ.Platform.Framework.WebApi.Model.Hypermedia;
using ApiModel = BeerTap.Model;

namespace BeerTap.WebApi.Hypermedia
{
    public class TapSpec : ResourceSpec<ApiModel.Tap, ApiModel.KegState, int>
    {
        public static ResourceUriTemplate Uri = ResourceUriTemplate.Create("Offices({officeId})/Taps({id})");

        // custom self link
        protected override IEnumerable<ResourceLinkTemplate<ApiModel.Tap>> Links()
        {
            yield return CreateLinkTemplate<LinkParameters>(CommonLinkRelations.Self, Uri, x => x.Parameters.OfficeId, x => x.Resource.Id);
        }

        protected override IEnumerable<IResourceStateSpec<ApiModel.Tap, ApiModel.KegState, int>> GetStateSpecs()
        {
            yield return new ResourceStateSpec<ApiModel.Tap, ApiModel.KegState, int>(ApiModel.KegState.Full)
            {
                Links =
                    {
                        CreateLinkTemplate<LinkParameters>(ApiModel.LinkRelations.Office, OfficeSpec.Uri, x => x.Parameters.OfficeId),
                        CreateLinkTemplate<LinkParameters>(ApiModel.LinkRelations.Keg, KegSpec.Uri.Many, x => x.Parameters.OfficeId, x => x.Resource.Id),
                        CreateLinkTemplate<LinkParameters>(ApiModel.LinkRelations.PullBeer, PullBeerSpec.Uri, x => x.Parameters.OfficeId, x => x.Resource.Id),
                    },

                Operations = new StateSpecOperationsSource<ApiModel.Tap, int>()
                    {
                        Get = ServiceOperations.Get,
                    }
            };

            yield return new ResourceStateSpec<ApiModel.Tap, ApiModel.KegState, int>(ApiModel.KegState.GoingDown)
            {
                Links =
                    {
                        CreateLinkTemplate<LinkParameters>(ApiModel.LinkRelations.Office, OfficeSpec.Uri, x => x.Parameters.OfficeId),
                        CreateLinkTemplate<LinkParameters>(ApiModel.LinkRelations.Keg, KegSpec.Uri.Many, x => x.Parameters.OfficeId, x => x.Resource.Id),
                        CreateLinkTemplate<LinkParameters>(ApiModel.LinkRelations.PullBeer, PullBeerSpec.Uri, x => x.Parameters.OfficeId, x => x.Resource.Id),
                    },

                Operations = new StateSpecOperationsSource<ApiModel.Tap, int>()
                    {
                        Get = ServiceOperations.Get,
                    }
            };

            yield return new ResourceStateSpec<ApiModel.Tap, ApiModel.KegState, int>(ApiModel.KegState.AlmostEmpty)
            {
                Links =
                    {
                        CreateLinkTemplate<LinkParameters>(ApiModel.LinkRelations.Office, OfficeSpec.Uri, x => x.Parameters.OfficeId),
                        CreateLinkTemplate<LinkParameters>(ApiModel.LinkRelations.Keg, KegSpec.Uri.Many, x => x.Parameters.OfficeId, x => x.Resource.Id),
                        CreateLinkTemplate<LinkParameters>(ApiModel.LinkRelations.PullBeer, PullBeerSpec.Uri, x => x.Parameters.OfficeId, x => x.Resource.Id),
                        CreateLinkTemplate<LinkParameters>(ApiModel.LinkRelations.ReplaceKeg, ReplaceKegSpec.Uri, x => x.Parameters.OfficeId, x => x.Resource.Id),
                    },

                Operations = new StateSpecOperationsSource<ApiModel.Tap, int>()
                {
                    Get = ServiceOperations.Get,
                }
            };

            yield return new ResourceStateSpec<ApiModel.Tap, ApiModel.KegState, int>(ApiModel.KegState.Empty)
            {
                Links =
                    {
                        CreateLinkTemplate<LinkParameters>(ApiModel.LinkRelations.Office, OfficeSpec.Uri, x => x.Parameters.OfficeId),
                        CreateLinkTemplate<LinkParameters>(ApiModel.LinkRelations.Keg, KegSpec.Uri.Many, x => x.Parameters.OfficeId, x => x.Resource.Id),
                        CreateLinkTemplate<LinkParameters>(ApiModel.LinkRelations.ReplaceKeg, ReplaceKegSpec.Uri, x => x.Parameters.OfficeId, x => x.Resource.Id),
                    },

                Operations = new StateSpecOperationsSource<ApiModel.Tap, int>()
                    {
                        Get = ServiceOperations.Get,
                        //InitialPost = ServiceOperations.Create,
                        //Delete = ServiceOperations.Delete,
                    }
            };
        }
    }
}