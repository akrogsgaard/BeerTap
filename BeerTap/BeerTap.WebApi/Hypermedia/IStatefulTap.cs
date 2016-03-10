using ApiModel = BeerTap.Model;

namespace BeerTap.WebApi.Hypermedia
{
    public interface IStatefulTap
    {
        ApiModel.KegState State { get; }
    }
}