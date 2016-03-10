using System.Threading.Tasks;
using BeerTap.Transport;

namespace BeerTap.DomainServices.Keg
{
    public interface IKegRepository
    {
        Task<KegDto> GetByIdAsync(int id);
        Task<KegDto> GetByTapIdAsync(int tapId);
        Task<int> SaveNewAsync(KegDto kegDto);
        Task UpdateAsync(KegDto kegDto);
        Task DeleteAsync(int id, int userId);
    }
}
