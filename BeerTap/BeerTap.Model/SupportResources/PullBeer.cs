using IQ.Platform.Framework.Common;
using IQ.Platform.Framework.WebApi.Model.Hypermedia;

namespace BeerTap.Model.SupportResources
{
    /// <summary>
    /// An action resource that describes the action of pulling beer from a tap.
    /// </summary>
    public class PullBeer : IStatelessResource, IIdentifiable<int>
    {
        /// <summary>
        /// Unique Identifier for the PullBeer resource.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Idenfifier for the Tap being pulled from.
        /// </summary>
        public int TapId { get; set; }

        /// <summary>
        /// The amount of beer being pulled from the Tap.
        /// </summary>
        public int Volume { get; set; }
    }
}