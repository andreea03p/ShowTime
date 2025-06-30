using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.DataAccess.Repositories;
public class UserRepository : GenericRepository<Models.User>, Abstraction.IUserRepository
{
    public UserRepository(ShowTimeDbContext context) : base(context)
    {
    }
}
