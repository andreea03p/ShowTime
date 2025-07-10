using ShowTime.BusinessLogic.Abstraction;
using ShowTime.BusinessLogic.Dtos;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Repositories.Abstraction;
using System.Runtime.InteropServices;

namespace ShowTime.BusinessLogic.Services;

public class ArtistService : IArtistService
{
    private readonly IGenericRepository<Artist> _artistRepository;
    private readonly IGenericRepository<Festival> _festivalRepository;
    private readonly IGenericRepository<Lineup> _lineupRepository;

    public ArtistService(
        IGenericRepository<Artist> artistRepository,
        IGenericRepository<Festival> festivalRepository,
        IGenericRepository<Lineup> lineupRepository)
    {
        _artistRepository = artistRepository;
        _festivalRepository = festivalRepository;
        _lineupRepository = lineupRepository;
    }

    public async Task AddArtistAsync(ArtistCreateDto artistCreateDto)
    {
        try
        {
            var artist = new Artist
            {
                Name = artistCreateDto.Name,
                Image = artistCreateDto.Image,
                Genre = artistCreateDto.Genre
            };
            await _artistRepository.AddAsync(artist);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while adding the artist.", ex);
        }
    }

    public async Task<IList<ArtistGetDto>> GetAllArtistsAsync()
    {
        try
        {
            var artists = await _artistRepository.GetAllAsync();
            return artists.Select(a => new ArtistGetDto
            {
                Id = a.Id,
                Name = a.Name,
                Image = a.Image,
                Genre = a.Genre
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving all artists.", ex);
        }
    }

    public async Task<ArtistGetDto?> GetArtistByIdAsync(int id)
    {
        try
        {
            var artist = await _artistRepository.GetByIdAsync(id);
            if (artist == null)
            {
                throw new KeyNotFoundException($"Artist with ID {id} not found.");
            }
            return new ArtistGetDto
            {
                Id = artist.Id,
                Name = artist.Name,
                Image = artist.Image,
                Genre = artist.Genre
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving the artist with ID {id}.", ex);
        }
    }

    public async Task UpdateArtistAsync(ArtistUpdateDto newArtist, int id)
    {
        try
        {
            var initialArtist = await _artistRepository.GetByIdAsync(id);
            if (initialArtist == null)
            {
                throw new KeyNotFoundException($"Artist with ID {id} not found.");
            }
            initialArtist.Name = newArtist.Name;
            initialArtist.Image = newArtist.Image;
            initialArtist.Genre = newArtist.Genre;
            await _artistRepository.UpdateAsync(initialArtist);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the artist.", ex);
        }
    }

    public async Task DeleteArtistAsync(int id)
    {
        try
        {
            var artist = await _artistRepository.GetByIdAsync(id);
            if (artist == null)
            {
                throw new KeyNotFoundException($"Artist with ID {id} not found.");
            }
            await _artistRepository.DeleteAsync(artist);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while deleting the artist with ID {id}.", ex);
        }
    }

    public async Task<List<LineupGetDto>> GetAllFestivalsForArtistAsync(int artistId)
    {
        try
        {
            var artist = await _artistRepository.GetByIdAsync(artistId);
            if (artist == null)
            {
                throw new KeyNotFoundException($"Artist with ID {artistId} not found.");
            }

            var lineups = await _lineupRepository.GetAllAsync();
            var artistLineups = lineups.Where(l => l.ArtistId == artistId).ToList();

            var lineupDtos = new List<LineupGetDto>();
            foreach (var lineup in artistLineups)
            {
                var festival = await _festivalRepository.GetByIdAsync(lineup.FestivalId);
                if (festival != null)
                {
                    lineupDtos.Add(new LineupGetDto
                    {
                        FestivalId = lineup.FestivalId,
                        ArtistId = lineup.ArtistId,
                        Stage = lineup.Stage,
                        StartTime = lineup.StartTime,
                        Festival = new FestivalGetDto
                        {
                            Id = festival.Id,
                            Name = festival.Name,
                            Address = festival.Location?.Address ?? "Location TBA",
                            Latitude = festival.Location?.Latitude ?? 0,
                            Longitude = festival.Location?.Longitude ?? 0,
                            StartDate = festival.StartDate,
                            EndDate = festival.EndDate,
                            SplashArt = festival.SplashArt,
                            Capacity = festival.Capacity
                        },
                        Artist = new ArtistGetDto
                        {
                            Id = artist.Id,
                            Name = artist.Name,
                            Genre = artist.Genre,
                            Image = artist.Image
                        }
                    });
                }
            }

            return lineupDtos.OrderBy(l => l.StartTime).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving festivals for artist {artistId}.", ex);
        }
    }
}