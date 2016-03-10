using IQ.Platform.Framework.Common;
using IQ.Platform.Framework.WebApi.Model.Hypermedia;

namespace BeerTap.Model
{
    /// <summary>
    /// A resource that describes a keg of beer.
    /// </summary>
    public class Keg : IStatelessResource, IIdentifiable<int>
    {
        /// <summary>
        /// Unique Identifier for the Keg.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Unique Identifier for the Keg.
        /// </summary>
        public int TapId { get; set; }

        /// <summary>
        /// The name of the beer in the Keg.
        /// </summary>
        public string BeerName { get; set; }

        /// <summary>
        /// The volume of beer that the keg can hold.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// The volume of beer in the Keg.
        /// </summary>
        public int Volume { get; set; }
    }
}