using ShowTime.DataAccess.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.DataAccess.Repositories;

public class FestivalRepository : GenericRepository<Models.Festival>, IFestivalRepository
{
    public FestivalRepository(ShowTimeDbContext context) : base(context)
    {
    }
}

