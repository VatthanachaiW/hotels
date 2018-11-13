using Hotels.Entities.Masters;
using Hotels.IDbConnections;
using Hotels.IRepositories;

namespace Hotels.Repositories
{
    public class ReceiptRepository : BaseRepository<Receipt>, IReceiptRepository
    {
        public ReceiptRepository(IApplicationDbContext context) : base(context)
        {
        }
    }
}