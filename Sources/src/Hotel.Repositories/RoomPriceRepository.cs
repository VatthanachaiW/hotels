using Hotels.Entities.Masters;
using Hotels.IDbConnections;
using Hotels.IRepositories;

namespace Hotels.Repositories
{
    public class RoomPriceRepository : BaseRepository<RoomPrice>, IRoomPriceRepository
    {
        //todo: Implemenet later if require
        public RoomPriceRepository(IApplicationDbContext context) : base(context)
        {
        }
    }
}