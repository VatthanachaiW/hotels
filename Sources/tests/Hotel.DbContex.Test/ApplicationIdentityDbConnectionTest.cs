using System;
using System.Linq;
using Hotels.Entities.Audits;
using Hotels.Entities.Masters;
using Hotels.IDbConnections;
using Hotels.TestUtilities;
using Xunit;

namespace Hotels.DbContextTest
{
    public class ApplicationIdentityDbConnectionTest
    {
        private readonly IApplicationDbContext _context;

        public ApplicationIdentityDbConnectionTest()
        {
            _context = new ApplicationDbContextHelper().DbContext();
        }

        [Fact(DisplayName = "Test get first item should be true")]
        public void GetItemTest()
        {
            Assert.Equal("Room Type A", _context.Set<RoomType>().FirstOrDefault().RoomTypeName);
        }

        [Fact(DisplayName = "Test add new item should be true")]
        public void AddItemTest()
        {
            var roomType = new RoomType
            {
                RoomTypeName = "Test room"
            };

            roomType.Created(Guid.NewGuid());

            _context.Set<RoomType>().Add(roomType);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact(DisplayName = "Test update first item should be true")]
        public void UpdateItemTest()
        {
            var roomtype = _context.Set<RoomType>().FirstOrDefault();

            Assert.NotNull(roomtype);

            roomtype.RoomTypeName = "Test Room Type";

            roomtype.Modified(Guid.NewGuid());

            _context.Set<RoomType>().Update(roomtype);

            Assert.True(_context.SaveChangesAsync().Result);

            Assert.Equal("Test Room Type", _context.Set<RoomType>().FirstOrDefault().RoomTypeName);
        }

        [Fact(DisplayName = "Test remove item should be true")]
        public void DeleteItemTest()
        {
            var roomtype = _context.Set<RoomType>().FirstOrDefault(s => s.RoomTypeName.Contains("E"));

            Assert.NotNull(roomtype);

            _context.Set<RoomType>().Remove(roomtype);
            Assert.True(_context.SaveChangesAsync().Result);
            Assert.Null(_context.Set<RoomType>().FirstOrDefault(s => s.RoomTypeName.Contains("E")));
        }

        [Fact(DisplayName = "Test audit tail create after insert item should be true")]
        public void AuditTailTest()
        {
            var roomtype = new RoomType
            {
                RoomTypeName = "Room Test Item"
            };

            _context.Set<RoomType>().Add(roomtype);
            Assert.True(_context.SaveChangesAsync().Result);

            var audittail = _context.Set<AuditTrail>().FirstOrDefault(s => s.NewValues.Contains("Room Test Item"));
            Assert.NotNull(audittail);
        }
    }
}