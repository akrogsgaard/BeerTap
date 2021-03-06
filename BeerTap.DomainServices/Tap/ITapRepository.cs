﻿using System.Collections.Generic;
using System.Threading.Tasks;
using BeerTap.Transport;

namespace BeerTap.DomainServices.Tap
{
    public interface ITapRepository
    {
        Task<TapDto> GetByIdAsync(int id);
        Task<IEnumerable<TapDto>> GetAllTapsByOfficeIdAsync(int officeId);
        Task UpdateAsync(TapDto tapDto);
    }
}
