namespace BeerTap.Model
{
    /// <summary>
    /// Enumeration listing the possible states of the Keg.
    /// </summary>
    public enum KegState
    {
        /// <summary>
        /// The Keg contains beer and is availble for consumption.
        /// links/actions:
        ///     PullBeer -> /Offices(123)/Taps(123)/PullBeer
        /// </summary>
        Full,

        /// <summary>
        /// The Keg contains beer and is availble for consumption.
        /// links/actions:
        ///     PullBeer -> /Offices(123)/Taps(123)/PullBeer
        /// 
        /// </summary>
        GoingDown,
        
        /// <summary>
        /// The Keg is almost empty and needs to be changed.
        ///     PullBeer -> /Offices(123)/Taps(123)/PullBeer
        ///     ReplaceKeg -> /Offices(123)/Taps(123)/ReplaceKeg
        /// </summary>
        AlmostEmpty,

        /// <summary>
        /// The Keg is empty and needs to be changed.
        /// links/actions:
        ///     PullBeer -> /Offices(123)/Taps(123)/PullBeer
        ///     ReplaceKeg -> /Offices(123)/Taps(123)/ReplaceKeg
        /// </summary>
        Empty
    }
}