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


    Task<List<LineupGetDto>> GetCompleteLineupForFestivalAsync(int festivalId);
    Task AddArtistToFestivalLineupAsync(int festivalId, int artistId, string stage, DateTime startTime);
    Task RemoveArtistFromLineupAsync(int festivalId, int artistId);
    Task UpdateLineupEntryAsync(int festivalId, int artistId, string stage, DateTime startTime);

    Task<List<LineupGetDto>> GetLineupByStageAsync(int festivalId, string stage);
    Task<List<LineupGetDto>> GetLineupByDayAsync(int festivalId, DateTime day);
    Task<List<string>> GetStagesForFestivalAsync(int festivalId);
}
