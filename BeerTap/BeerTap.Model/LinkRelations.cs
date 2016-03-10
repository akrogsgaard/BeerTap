namespace BeerTap.Model
{
    /// <summary>
    /// iQmetrix link relation names
    /// </summary>
    public static class LinkRelations
    {
        /// <summary>
        /// link relation to describe the Office resource.
        /// </summary>
        public const string Office = "iq:Office";

        /// <summary>
        /// link relation to describe the Tap resource.
        /// </summary>
        public const string Tap = "iq:Tap";

        /// <summary>
        /// link relation to describe the Keg resource.
        /// </summary>
        public const string Keg = "iq:Keg";

        /// <summary>
        /// link relation to pull an amount of beer.
        /// </summary>
        public const string PullBeer = "iq:PullBeer";
        
        /// <summary>
        /// link relation to replace the keg.
        /// </summary>
        public const string ReplaceKeg = "iq:ReplaceKeg";
    }
}
