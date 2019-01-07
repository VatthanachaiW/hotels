using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotels.Entities.Masters;
using Hotels.IDbConnections;
using Hotels.IRepositories;
using Hotels.Repositories;
using Hotels.TestUtilities;
using Xunit;

namespace Hotels.RepositoriesTest
{
    public class RoomTypeRepositoryTest
    {
        private IApplicationDbContext _context;
        private IRoomTypeRepository _repository;

        public RoomTypeRepositoryTest()
        {
            _context = new ApplicationDbContextHelper().DbContext();
            _repository = new RoomTypeRepository(_context);
        }

        [Fact]
        public async Task GetAllAsyncTest()
        {
            var roomTypes = await _repository.GetAllAsync();
            Assert.True(roomTypes.Any());
        }

        [Fact]
        public async Task GetAllAsyncWithPredicateTest()
        {
            var roomTypes = await _repository.GetAllAsync(w => w.RoomTypeName.Contains("A"));
            Assert.True(roomTypes.Any());
        }

        [Fact]
        public async Task GetAsyncWithPredicateTest()
        {
            var roomType = await _repository.GetAsync(s => s.RoomTypeName == "Room Type C");
            Assert.NotNull(roomType);
        }


        [Fact]
        public async Task GetByIdAsyncTest()
        {
            var roomType = await _repository.GetAllAsync();
            Assert.NotNull(roomType);
            var roomTypeId = roomType.FirstOrDefault().Id;


            var nRoomType = await _repository.GetByIdAsync(roomTypeId);
            Assert.NotNull(nRoomType);
            Assert.Equal(roomTypeId, nRoomType.Id);
        }


        [Fact]
        public async Task AddNewAsyncTest()
        {
            var roomtype = new RoomType
            {
                RoomTypeName = "Demo Room Type"
            };

            roomtype.Created(Guid.NewGuid());

            await _repository.AddAsync(roomtype);
            Assert.True(_context.SaveChangesAsync().Result);
        }


        [Fact]
        public async Task AddNewRangeAsyncTest()
        {
            var roomtype = new List<RoomType>
            {
                new RoomType
                {
                    RoomTypeName = "Demo Room Type"
                },
                new RoomType
                {
                    RoomTypeName = "Demo Room Type 2"
                },
            };

            roomtype.ForEach(s => s.Created(Guid.NewGuid()));

            await _repository.AddRangeAsync(roomtype);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public async Task UpdateTest()
        {
            var roomType = await _repository.GetAsync(s => s.RoomTypeName == "Room Type D");

            Assert.NotNull(roomType);
            roomType.Modified(Guid.NewGuid());

            _repository.Update(roomType);
            Assert.True(_context.SaveChangesAsync().Result);
        }


        [Fact]
        public async Task RemoveTest()
        {
            var roomType = await _repository.GetAsync(s => s.RoomTypeName == "Room Type E");
            Assert.NotNull(roomType);

            _repository.Remove(roomType);
            Assert.True(_context.SaveChangesAsync().Result);
        }
    }
}