using IQ.Platform.Framework.Common;
using IQ.Platform.Framework.WebApi.Model.Hypermedia;

namespace BeerTap.Model
{
    /// <summary>
    /// A resource that describes a beer tap.
    /// </summary>
    public class Tap : IStatefulResource<KegState>, IIdentifiable<int>
    {
        /// <summary>
        /// Unique Identifier for the Tap.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Unique Identifier for the office associated with this tap.
        /// </summary>
        public int OfficeId { get; set; }

        /// <summary>
        /// Unique Identifier for the keg associated with this tap.
        /// </summary>
        public int KegId { get; set; }

        /// <summary>
        /// The state of the Keg.
        /// </summary>
        public KegState KegState { get; set; }
    }
}
