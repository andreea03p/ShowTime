using ShowTime.BusinessLogic.Abstraction;
using ShowTime.BusinessLogic.Dtos;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Models.Extras;
using ShowTime.DataAccess.Repositories.Abstraction;
using System.Collections.Generic;

namespace ShowTime.BusinessLogic.Services;

public class FestivalService : IFestivalService
{
    private readonly IGenericRepository<Festival> _festivalRepository;
    private readonly IGenericRepository<Artist> _artistRepository;
    private readonly IGenericRepository<Lineup> _lineupRepository;

    public FestivalService(
        IGenericRepository<Festival> festivalRepository,
        IGenericRepository<Artist> artistRepository,
        IGenericRepository<Lineup> lineupRepository)
    {
        _festivalRepository = festivalRepository;
        _artistRepository = artistRepository;
        _lineupRepository = lineupRepository;
    }

    public async Task AddFestivalAsync(FestivalCreateDto festivalCreateDto)
    {
        try
        {
            var festival = new Festival
            {
                Name = festivalCreateDto.Name,
                Location = new Location
                {
                    Address = festivalCreateDto.Address,
                    Latitude = festivalCreateDto.Latitude,
                    Longitude = festivalCreateDto.Longitude
                },
                StartDate = festivalCreateDto.StartDate,
                EndDate = festivalCreateDto.EndDate,
                SplashArt = festivalCreateDto.SplashArt,
                Capacity = festivalCreateDto.Capacity
            };

            await _festivalRepository.AddAsync(festival);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while adding the festival.", ex);
        }
    }

    public async Task<IList<FestivalGetDto>> GetAllFestivalsAsync()
    {
        try
        {
            var festivals = await _festivalRepository.GetAllAsync();

            return festivals.Select(f => new FestivalGetDto
            {
                Id = f.Id,
                Name = f.Name,
                Address = f.Location.Address,
                Latitude = f.Location.Latitude,
                Longitude = f.Location.Longitude,
                StartDate = f.StartDate,
                EndDate = f.EndDate,
                SplashArt = f.SplashArt,
                Capacity = f.Capacity
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving all festivals.", ex);
        }
    }

    public async Task<FestivalGetDto?> GetFestivalByIdAsync(int id)
    {
        try
        {
            var festival = await _festivalRepository.GetByIdAsync(id);

            if (festival == null)
            {
                throw new KeyNotFoundException($"Festival with ID {id} not found.");
            }

            return new FestivalGetDto
            {
                Id = festival.Id,
                Name = festival.Name,
                Address = festival.Location.Address,
                Latitude = festival.Location.Latitude,
                Longitude = festival.Location.Longitude,
                StartDate = festival.StartDate,
                EndDate = festival.EndDate,
                SplashArt = festival.SplashArt,
                Capacity = festival.Capacity
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving the festival with ID {id}.", ex);
        }
    }

    public async Task UpdateFestivalAsync(FestivalUpdateDto updateDto, int id)
    {
        try
        {
            var existingFestival = await _festivalRepository.GetByIdAsync(id);

            if (existingFestival == null)
            {
                throw new KeyNotFoundException($"Festival with ID {id} not found.");
            }

            existingFestival.Name = updateDto.Name;
            existingFestival.Location.Address = updateDto.Address;
            existingFestival.Location.Latitude = updateDto.Latitude;
            existingFestival.Location.Longitude = updateDto.Longitude;
            existingFestival.StartDate = updateDto.StartDate;
            existingFestival.EndDate = updateDto.EndDate;
            existingFestival.SplashArt = updateDto.SplashArt;
            existingFestival.Capacity = updateDto.Capacity;

            await _festivalRepository.UpdateAsync(existingFestival);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the festival.", ex);
        }
    }

    public async Task DeleteFestivalAsync(int id)
    {
        try
        {
            var festival = await _festivalRepository.GetByIdAsync(id);
            if (festival == null)
            {
                throw new KeyNotFoundException($"Festival with ID {id} not found.");
            }
            await _festivalRepository.DeleteAsync(festival);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while deleting the festival with ID {id}.", ex);
        }
    }



    public async Task<List<LineupGetDto>> GetCompleteLineupForFestivalAsync(int festivalId)
    {
        try
        {
            var lineups = await _lineupRepository.GetAllAsync();
            var festivalLineups = lineups.Where(l => l.FestivalId == festivalId).ToList();

            var result = new List<LineupGetDto>();
            foreach (var lineup in festivalLineups)
            {
                var festival = await _festivalRepository.GetByIdAsync(lineup.FestivalId);
                var artist = await _artistRepository.GetByIdAsync(lineup.ArtistId);

                if (festival != null && artist != null)
                {
                    result.Add(new LineupGetDto
                    {
                        FestivalId = lineup.FestivalId,
                        ArtistId = lineup.ArtistId,
                        Stage = lineup.Stage,
                        StartTime = DateTimeOffset.Now,
                        Festival = new FestivalGetDto
                        {
                            Id = festival.Id,
                            Name = festival.Name,
                            Address = festival.Location.Address,
                            Latitude = festival.Location.Latitude,
                            Longitude = festival.Location.Longitude,
                            StartDate = festival.StartDate,
                            EndDate = festival.EndDate,
                            SplashArt = festival.SplashArt,
                            Capacity = festival.Capacity
                        },
                        Artist = new ArtistGetDto
                        {
                            Id = artist.Id,
                            Name = artist.Name,
                            Image = artist.Image,
                            Genre = artist.Genre
                        }
                    });
                }
            }

            return result.OrderBy(l => l.StartTime).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving lineup for festival {festivalId}.", ex);
        }
    }

    public async Task AddArtistToFestivalLineupAsync(int festivalId, int artistId, string stage, DateTimeOffset startTime)
    {
        try
        {
            Console.WriteLine($"FestivalId={festivalId}, ArtistId={artistId}, Stage='{stage}', StartTime={startTime}");

            if (festivalId <= 0)
                throw new ArgumentException("Invalid festival ID", nameof(festivalId));

            if (artistId <= 0)
                throw new ArgumentException("Invalid artist ID", nameof(artistId));

            if (string.IsNullOrWhiteSpace(stage))
                throw new ArgumentException("Stage cannot be empty", nameof(stage));


            var existingLineups = await _lineupRepository.GetAllAsync();
            var existingEntry = existingLineups.FirstOrDefault(l =>
                l.FestivalId == festivalId && l.ArtistId == artistId);

            if (existingEntry != null)
            {
                var festival = await _festivalRepository.GetByIdAsync(festivalId);
                var artist = await _artistRepository.GetByIdAsync(artistId);

                throw new InvalidOperationException(
                    $"Artist '{artist?.Name ?? "Unknown"}' is already performing at festival '{festival?.Name ?? "Unknown"}' " +
                    $"on stage '{existingEntry.Stage}' at {existingEntry.StartTime:yyyy-MM-dd HH:mm}. " +
                    $"Each artist can only perform once per festival.");
            }

            var targetFestival = await _festivalRepository.GetByIdAsync(festivalId);
            if (targetFestival == null)
            {
                throw new ArgumentException($"Festival with ID {festivalId} not found");
            }

            var targetArtist = await _artistRepository.GetByIdAsync(artistId);
            if (targetArtist == null)
            {
                throw new ArgumentException($"Artist with ID {artistId} not found");
            }

            Console.WriteLine($"Adding {targetArtist.Name} to {targetFestival.Name}");

            var lineup = new Lineup
            {
                FestivalId = festivalId,
                Festival = targetFestival,
                ArtistId = artistId,
                Artist = targetArtist,
                Stage = stage.Trim(),
                StartTime = DateTime.Today
            };

            await _lineupRepository.AddAsync(lineup);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }

            throw new Exception($"Failed to add artist to festival lineup: {ex.Message}", ex);
        }
    }

    public async Task RemoveArtistFromLineupAsync(int festivalId, int artistId)
    {
        try
        {
            var lineups = await _lineupRepository.GetAllAsync();
            var lineup = lineups.FirstOrDefault(l => l.FestivalId == festivalId && l.ArtistId == artistId);

            if (lineup == null)
                throw new KeyNotFoundException($"Lineup not found for festival {festivalId} and artist {artistId}.");

            await _lineupRepository.DeleteAsync(lineup);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while removing artist from lineup.", ex);
        }
    }

    public async Task UpdateLineupEntryAsync(int festivalId, int artistId, string stage, DateTimeOffset startTime)
    {
        try
        {
            var lineups = await _lineupRepository.GetAllAsync();
            var lineup = lineups.FirstOrDefault(l => l.FestivalId == festivalId && l.ArtistId == artistId);

            if (lineup == null)
                throw new KeyNotFoundException($"Lineup not found for festival {festivalId} and artist {artistId}.");

            lineup.Stage = stage;
            lineup.StartTime = startTime;

            await _lineupRepository.UpdateAsync(lineup);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating lineup entry.", ex);
        }
    }

    public async Task<List<LineupGetDto>> GetLineupByStageAsync(int festivalId, string stage)
    {
        try
        {
            var completeLineup = await GetCompleteLineupForFestivalAsync(festivalId);
            return completeLineup.Where(l => l.Stage.Equals(stage, StringComparison.OrdinalIgnoreCase))
                                .OrderBy(l => l.StartTime)
                                .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving lineup for stage {stage}.", ex);
        }
    }

    public async Task<List<LineupGetDto>> GetLineupByDayAsync(int festivalId, DateTime day)
    {
        try
        {
            var completeLineup = await GetCompleteLineupForFestivalAsync(festivalId);
            return completeLineup.Where(l => l.StartTime.Date == day.Date)
                                .OrderBy(l => l.StartTime)
                                .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving lineup for day {day:yyyy-MM-dd}.", ex);
        }
    }

    public async Task<List<string>> GetStagesForFestivalAsync(int festivalId)
    {
        try
        {
            var lineups = await _lineupRepository.GetAllAsync();
            return lineups.Where(l => l.FestivalId == festivalId)
                         .Select(l => l.Stage)
                         .Distinct()
                         .OrderBy(s => s)
                         .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving stages for festival {festivalId}.", ex);
        }
    }
}