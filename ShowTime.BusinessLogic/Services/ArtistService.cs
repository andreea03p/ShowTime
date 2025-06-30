using ShowTime.BusinessLogic.Abstraction;
using ShowTime.BusinessLogic.Dtos;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Repositories.Abstraction;
using System.Runtime.InteropServices;

namespace ShowTime.BusinessLogic.Services;

public class ArtistService : IArtistService
{
    public readonly IGenericRepository<Artist> _artistRepository;

    public ArtistService(IGenericRepository<Artist> artistRepository)
    {
        _artistRepository = artistRepository;
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
}