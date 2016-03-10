using IQ.Platform.Framework.Common;
using IQ.Platform.Framework.WebApi.Model.Hypermedia;

namespace BeerTap.Model
{
    /// <summary>
    /// A resource that describes an office. ie - Regina, Vancouver, Winnipeg, Davidson, Manila.
    /// </summary>
    public class Office : IStatelessResource, IIdentifiable<int>
    {
        /// <summary>
        /// Unique Identifier for the Office
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name for the Office
        /// </summary>
        public string Name { get; set; }
    }
}
