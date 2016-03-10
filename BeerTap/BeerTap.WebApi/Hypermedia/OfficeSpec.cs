using IQ.Platform.Framework.WebApi.Hypermedia;
using IQ.Platform.Framework.WebApi.Hypermedia.Specs;
using IQ.Platform.Framework.WebApi.Model.Hypermedia;
using ApiModel = BeerTap.Model;

namespace BeerTap.WebApi.Hypermedia
{
    public class OfficeSpec : SingleStateResourceSpec<ApiModel.Office, int>
    {
        public static ResourceUriTemplate Uri = ResourceUriTemplate.Create("Offices({id})");

        public override string EntrypointRelation
        {
            get { return ApiModel.LinkRelations.Office; }
        }

        public override IResourceStateSpec<ApiModel.Office, NullState, int> StateSpec
        {
            get
            {
                return
                    new SingleStateSpec<ApiModel.Office, int>
                    {
                        Links =
                            {
                                CreateLinkTemplate(ApiModel.LinkRelations.Tap, TapSpec.Uri.Many, r => r.Id),
                            },

                        Operations =
                            {
                                Get = ServiceOperations.Get,
                                //InitialPost = ServiceOperations.Create,
                                Post = ServiceOperations.Update,
                                Delete = ServiceOperations.Delete,
                            },
                    };
            }
        }
    }
}