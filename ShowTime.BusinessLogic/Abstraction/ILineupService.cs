using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowTime.BusinessLogic.Dtos;

namespace ShowTime.BusinessLogic.Abstraction;

public interface ILineupService
{
    Task AddArtistToFestivalLineupAsync(int festivalId, int artistId, string stage, DateTime startTime);
    Task RemoveArtistFromLineupAsync(int festivalId, int artistId);
    Task<List<ArtistGetDto>> GetAllArtistsFromFestivalAsync(int festivalId);
    Task<List<FestivalGetDto>> GetAllFestivalsForArtistAsync(int artistId);
    Task UpdatePerformanceDetailsAsync(int festivalId, int artistId, string stage, DateTime startTime);

    Task<List<LineupGetDto>> GetCompleteLineupForFestivalAsync(int festivalId);
}
