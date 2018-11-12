using Hotels.Entities.Masters;
using Hotels.IDbConnections;
using Hotels.IRepositories;

namespace Hotels.Repositories
{
    public class RoomTypeRepository : BaseRepository<RoomType>, IRoomTypeRepository
    {
        public RoomTypeRepository(IApplicationDbContext context) : base(context)
        {
        }
    }
}