using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowTime.BusinessLogic.Dtos;

namespace ShowTime.BusinessLogic.Abstraction;

public interface IArtistService
{
    Task<ArtistGetDto?> GetArtistByIdAsync(int id);
    Task<IList<ArtistGetDto>> GetAllArtistsAsync();
    Task AddArtistAsync(ArtistCreateDto artist);   
    Task UpdateArtistAsync(ArtistUpdateDto artist, int id);
}
