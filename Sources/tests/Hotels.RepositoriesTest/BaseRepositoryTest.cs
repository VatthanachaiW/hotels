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
    public class BaseRepositoryTest
    {
        private IApplicationDbContext _context;
        private IBaseRepository<RoomType> _repository;

        public BaseRepositoryTest()
        {
            _context = new ApplicationDbContextHelper().DbContext();
            _repository = new BaseRepository<RoomType>(_context);
        }

        [Fact]
        public void GetAllTest()
        {
            Assert.True(_repository.GetAll().Any());
        }

        [Fact]
        public void GetAllWithPredicateTest()
        {
            Assert.True(_repository.GetAll(w => w.RoomTypeName.Contains("e")).Any());
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
        public void GetWithPredicateTest()
        {
            var roomType = _repository.Get(s => s.RoomTypeName == "Room Type C");
            Assert.NotNull(roomType);
        }

        [Fact]
        public async Task GetAsyncWithPredicateTest()
        {
            var roomType = await _repository.GetAsync(s => s.RoomTypeName == "Room Type C");
            Assert.NotNull(roomType);
        }

        [Fact]
        public void GetByIdTest()
        {
            var roomType = _repository.GetAll().FirstOrDefault();
            Assert.NotNull(roomType);

            var nRoomType = _repository.GetById(roomType.Id);
            Assert.NotNull(nRoomType);
            Assert.Equal(roomType.Id, nRoomType.Id);
        }

        [Fact]
        public async Task GetByIdAsyncTest()
        {
            var roomType = _repository.GetAll().FirstOrDefault();
            Assert.NotNull(roomType);

            var nRoomType = await _repository.GetByIdAsync(roomType.Id);
            Assert.NotNull(nRoomType);
            Assert.Equal(roomType.Id, nRoomType.Id);
        }

        [Fact]
        public void AddNewTest()
        {
            var roomtype = new RoomType
            {
                RoomTypeName = "Demo Room Type"
            };

            roomtype.Created(Guid.NewGuid());

            _repository.Add(roomtype);
            Assert.True(_context.SaveChangesAsync().Result);
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
        public void AddNewRangeTest()
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

            _repository.AddRange(roomtype);
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
        public void UpdateTest()
        {
            var roomtype = _repository.Get(s => s.RoomTypeName == "Room Type D");

            Assert.NotNull(roomtype);
            roomtype.Modified(Guid.NewGuid());

            _repository.Update(roomtype);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public void UpdateRangeTest()
        {
            var roomtypes = _repository.GetAll(s => s.RoomTypeName.Contains("e"));
            Assert.NotNull(roomtypes);

            roomtypes.ForEach(s => s.Modified(Guid.NewGuid()));
            _repository.UpdateRange(roomtypes);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public void RemoveTest()
        {
            var roomtype = _repository.Get(s => s.RoomTypeName == "Room Type E");
            Assert.NotNull(roomtype);

            _repository.Remove(roomtype);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public void RemoveRangeTest()
        {
            var roomtypes = _repository.GetAll(s => s.RoomTypeName == "Room Type E");
            Assert.NotNull(roomtypes);

            _repository.RemoveRange(roomtypes);
            Assert.True(_context.SaveChangesAsync().Result);
        }
    }
}