using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowTime.BusinessLogic.Dtos;

namespace ShowTime.BusinessLogic.Abstraction;

public interface IFestivalService
{
    Task<FestivalGetDto?> GetFestivalByIdAsync(int id);
    Task<IList<FestivalGetDto>> GetAllFestivalsAsync();
    Task AddFestivalAsync(FestivalCreateDto artist);
    Task UpdateFestivalAsync(FestivalUpdateDto artist, int id);
    Task DeleteFestivalAsync(int id);
}
