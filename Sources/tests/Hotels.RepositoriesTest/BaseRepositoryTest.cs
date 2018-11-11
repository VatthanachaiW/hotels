using System;
using System.Linq;
using Hotel.Test.Utilities;
using Hotels.Entities.Masters;
using Hotels.IDbConnections;
using Xunit;

namespace Hotels.RepositoriesTest
{
    public class BaseRepositoryTest
    {
        private IApplicationDbContext _context;

        public BaseRepositoryTest()
        {
            _context = new ApplicationDbContextHelper().DbContext();
        }

        [Fact]
        public void GetAllTest()
        {
            Assert.True(_context.Set<RoomType>().Any());
        }
    }
}