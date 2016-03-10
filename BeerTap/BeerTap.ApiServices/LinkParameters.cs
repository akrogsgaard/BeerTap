namespace BeerTap.ApiServices
{
    /// <summary>
    /// The class is used to pass additional parameters to hypermedia links definitions in resource specifications.
    /// </summary>
    public class LinkParameters
    {
        public LinkParameters(int officeId)
        {
            OfficeId = officeId;
        }

        public int OfficeId { get; private set; }
    }
}