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
    public class RoomRepositoryTest
    {
        private IApplicationDbContext _context;
        private IRoomTypeRepository _romTypeRepository;
        private IRoomRepository _roomRepository;

        public RoomRepositoryTest()
        {
            _context = new ApplicationDbContextHelper().DbContext();
            _romTypeRepository = new RoomTypeRepository(_context);
            _roomRepository = new RoomRepository(_context);
        }

        [Fact]
        public void GetAllTest()
        {
            Assert.True(_roomRepository.GetAll().Any());
        }

        [Fact]
        public void GetAllWithPredicateTest()
        {
            Assert.True(_roomRepository.GetAll(w => w.RoomCode.Contains("R0010")).Any());
        }

        [Fact]
        public async Task GetAllAsyncTest()
        {
            var room = await _roomRepository.GetAllAsync();
            Assert.True(room.Any());
        }

        [Fact]
        public async Task GetAllAsyncWithPredicateTest()
        {
            var room = await _roomRepository.GetAllAsync(w => w.RoomCode.Contains("R0010"));
            Assert.True(room.Any());
        }

        [Fact]
        public void GetWithPredicateTest()
        {
            var room = _roomRepository.Get(s => s.RoomCode == "R0010");
            Assert.NotNull(room);
        }

        [Fact]
        public async Task GetAsyncWithPredicateTest()
        {
            var room = await _roomRepository.GetAsync(s => s.RoomCode == "R0010");
            Assert.NotNull(room);
        }

        [Fact]
        public void GetByIdTest()
        {
            var roomCode = "R0010";
            var room = _roomRepository.GetAll().FirstOrDefault(s => s.RoomCode == roomCode);
            Assert.NotNull(room);

            var nRoom = _roomRepository.GetById(room.Id);
            Assert.NotNull(nRoom);
            Assert.Equal(roomCode, nRoom.RoomCode);
        }

        [Fact]
        public async Task GetByIdAsyncTest()
        {
            var roomCode = "R0010";
            var room = _roomRepository.GetAll().FirstOrDefault(s => s.RoomCode == roomCode);
            Assert.NotNull(room);

            var nRoom = await _roomRepository.GetByIdAsync(room.Id);
            Assert.NotNull(nRoom);
            Assert.Equal(roomCode, nRoom.RoomCode);
        }

        [Fact]
        public void AddNewTest()
        {
            var roomA = _romTypeRepository.Get(s => s.RoomTypeName == "Room Type A");
            var room = new Room
            {
                RoomCode = "R0011",
                Description = "Room demo",
                RoomTypeId = roomA.Id
            };

            room.Created(Guid.NewGuid());

            _roomRepository.Add(room);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public async Task AddNewAsyncTest()
        {
            var roomA = _romTypeRepository.Get(s => s.RoomTypeName == "Room Type A");
            var room = new Room
            {
                RoomCode = "R0011",
                Description = "Room demo",
                RoomTypeId = roomA.Id
            };

            room.Created(Guid.NewGuid());

            await _roomRepository.AddAsync(room);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public void AddNewRangeTest()
        {
            var roomA = _romTypeRepository.Get(s => s.RoomTypeName == "Room Type A");
            var rooms = new List<Room>
            {
                new Room
                {
                    RoomCode = "R0011",
                    Description = "Room demo",
                    RoomTypeId = roomA.Id
                },
                new Room
                {
                    RoomCode = "R0012",
                    Description = "Room demo",
                    RoomTypeId = roomA.Id
                },
            };

            rooms.ForEach(s => s.Created(Guid.NewGuid()));

            _roomRepository.AddRange(rooms);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public async Task AddNewRangeAsyncTest()
        {
            var roomA = _romTypeRepository.Get(s => s.RoomTypeName == "Room Type A");
            var rooms = new List<Room>
            {
                new Room
                {
                    RoomCode = "R0011",
                    Description = "Room demo",
                    RoomTypeId = roomA.Id
                },
                new Room
                {
                    RoomCode = "R0012",
                    Description = "Room demo",
                    RoomTypeId = roomA.Id
                },
            };

            rooms.ForEach(s => s.Created(Guid.NewGuid()));

            await _roomRepository.AddRangeAsync(rooms);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public void UpdateTest()
        {
            var room = _roomRepository.Get(s => s.RoomCode == "R0010");

            Assert.NotNull(room);
            room.Modified(Guid.NewGuid());

            _roomRepository.Update(room);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public void UpdateRangeTest()
        {
            var room = _roomRepository.GetAll(s => s.RoomCode == "R0010");
            Assert.NotNull(room);

            room.ForEach(s => s.Modified(Guid.NewGuid()));
            _roomRepository.UpdateRange(room);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public void RemoveTest()
        {
            var room = _roomRepository.Get(s => s.RoomCode == "R0010");
            Assert.NotNull(room);

            _roomRepository.Remove(room);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public void RemoveRangeTest()
        {
            var room = _roomRepository.GetAll(s => s.RoomCode == "R0010");
            Assert.NotNull(room);

            _roomRepository.RemoveRange(room);
            Assert.True(_context.SaveChangesAsync().Result);
        }
    }
}