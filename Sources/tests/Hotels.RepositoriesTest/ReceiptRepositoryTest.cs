using Hotels.DbConnections;
using Hotels.IDbConnections;
using Hotels.IRepositories;
using Hotels.Repositories;
using Hotels.TestUtilities;

namespace Hotels.RepositoriesTest
{
    public class ReceiptRepositoryTest
    {
        private IApplicationDbContext _context;
        private IRoomRepository _roomRepository;
        private IRoomPriceRepository _roomPriceRepository;

        public ReceiptRepositoryTest()
        {
            _context = new ApplicationDbContextHelper().DbContext();
            _roomRepository = new RoomRepository(_context);
            _roomPriceRepository = new RoomPriceRepository(_context);
        }
    }
}