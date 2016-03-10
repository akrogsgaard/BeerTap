using System.Collections.Generic;
using System.Threading.Tasks;
using BeerTap.Transport;

namespace BeerTap.DomainServices.Office
{
    public interface IOfficeRepository
    {
        Task<OfficeDto> GetByIdAsync(int id);
        Task<IEnumerable<OfficeDto>> GetByNameAsync(string name);
        Task<IEnumerable<OfficeDto>> GetAllAsync();
        Task<int> SaveNewAsync(OfficeDto officeDto);
        Task UpdateAsync(OfficeDto officeDto);
        Task DeleteAsync(int id, int userId);
    }
}
