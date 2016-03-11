namespace BeerTap.ApiServices
{
    /// <summary>
    /// The class is used to pass additional parameters to hypermedia links definitions in resource specifications.
    /// </summary>
    public class LinkParameters
    {
        public LinkParameters(int officeId, int tapId)
        {
            OfficeId = officeId;
            TapId = tapId;
        }

        public int OfficeId { get; private set; }
        public int TapId { get; private set; }
    }
}