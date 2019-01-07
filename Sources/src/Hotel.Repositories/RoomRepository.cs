using Hotels.Entities.Masters;
using Hotels.IDbConnections;
using Hotels.IRepositories;

namespace Hotels.Repositories
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        //todo: Implemenet later if require
        public RoomRepository(IApplicationDbContext context) : base(context)
        {
        }
    }
}