using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hotels.Entities.Masters;
using Hotels.IDbConnections;
using Hotels.IRepositories;
using Hotels.Repositories;
using Hotels.TestUtilities;
using System.Linq;
using Xunit;

namespace Hotels.RepositoriesTest
{
    public class RoomPriceRepositoryTest
    {
        private IApplicationDbContext _context;
        private IRoomRepository _roomRepository;
        private IRoomPriceRepository _priceRepository;

        public RoomPriceRepositoryTest()
        {
            _context = new ApplicationDbContextHelper().DbContext();
            _roomRepository = new RoomRepository(_context);
            _priceRepository = new RoomPriceRepository(_context);
        }


        [Fact]
        public async Task GetAllAsyncTest()
        {
            var prices = await _priceRepository.GetAllAsync();
            Assert.True(prices.Any());
        }

        [Fact]
        public async Task GetAllAsyncWithPredicateTest()
        {
            var room = await _roomRepository.GetAsync(s => s.RoomCode == "R0010");
            Assert.NotNull(room);

            var prices = await _priceRepository.GetAllAsync(w => w.RoomId == room.Id);
            Assert.True(prices.Any());
        }

        [Fact]
        public async Task GetAsyncWithPredicateTest()
        {
            var room = await _roomRepository.GetAsync(s => s.RoomCode == "R0010");
            Assert.NotNull(room);


            var price = await _priceRepository.GetAsync(s => s.RoomId == room.Id);
            Assert.NotNull(price);
        }

        [Fact]
        public async Task GetByIdAsyncTest()
        {
            var room = await _roomRepository.GetAsync(s => s.RoomCode == "R0010");
            var price = await _priceRepository.GetAsync(s => s.RoomId == room.Id);
            Assert.NotNull(price);

            var nPrice = await _priceRepository.GetByIdAsync(price.Id);
            Assert.NotNull(nPrice);
            Assert.Equal(price.Id, nPrice.Id);
        }


        [Fact]
        public async Task AddNewAsyncTest()
        {
            var room = await _roomRepository.GetAsync(s => s.RoomCode == "R0010");
            var price = new RoomPrice
            {
                Price = 500,
                RoomId = room.Id,
                PriceDate = DateTime.Now
            };

            price.Created(Guid.NewGuid());

            await _priceRepository.AddAsync(price);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public async Task AddNewRangeAsyncTest()
        {
            var roomA = await _roomRepository.GetAsync(s => s.RoomCode == "R0010");
            var roomB = await _roomRepository.GetAsync(s => s.RoomCode == "R0020");

            var price = new List<RoomPrice>
            {
                new RoomPrice
                {
                    Price = 500,
                    RoomId = roomA.Id,
                    PriceDate = DateTime.Now
                },
                new RoomPrice
                {
                    Price = 600,
                    RoomId = roomB.Id,
                    PriceDate = DateTime.Now
                },
            };
            price.ForEach(s => s.Created(Guid.NewGuid()));

            await _priceRepository.AddRangeAsync(price);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public async Task UpdateTest()
        {
            var room = await _roomRepository.GetAsync(s => s.RoomCode == "R0010");
            var price = await _priceRepository.GetAsync(s => s.RoomId == room.Id);

            Assert.NotNull(price);
            price.Modified(Guid.NewGuid());

            _priceRepository.Update(price);
            Assert.True(_context.SaveChangesAsync().Result);
        }


        [Fact]
        public async Task RemoveTest()
        {
            var room = await _roomRepository.GetAsync(s => s.RoomCode == "R0010");
            var price = await _priceRepository.GetAsync(s => s.RoomId == room.Id);
            Assert.NotNull(price);

            _priceRepository.Remove(price);
            Assert.True(_context.SaveChangesAsync().Result);
        }
    }
}