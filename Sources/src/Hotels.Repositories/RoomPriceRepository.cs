using Hotels.Entities.Masters;
using Hotels.IDbConnections;
using Hotels.IRepositories;

namespace Hotels.Repositories
{
    public class RoomPriceRepository : BaseRepository<RoomPrice>, IRoomPriceRepository
    {
        public RoomPriceRepository(IApplicationDbContext context) : base(context)
        {
        }
    }
}