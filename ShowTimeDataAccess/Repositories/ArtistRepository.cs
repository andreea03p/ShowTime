using ShowTime.DataAccess.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ShowTime.DataAccess.Repositories;

public class ArtistRepository : GenericRepository<Models.Artist>, IArtistRepository
{
    public ArtistRepository(ShowTimeDbContext context) : base(context)
    {
    }
}