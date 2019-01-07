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
    public class HotelRepositoryTest
    {
        private IApplicationDbContext _context;
        private IHotelRepository _hotelRepository;

        public HotelRepositoryTest()
        {
            _context = new ApplicationDbContextHelper().DbContext();
            _hotelRepository = new HotelRepository(_context);
        }


        [Fact]
        public async Task GetAllAsyncTest()
        {
            var hotels = await _hotelRepository.GetAllAsync();
            Assert.True(hotels.Any());
        }

        [Fact]
        public async Task GetAllAsyncWithPredicateTest()
        {
            var hotel = await _hotelRepository.GetAllAsync(w => w.HotelName == "ABC Hotel");
            Assert.True(hotel.Any());
        }

        [Fact]
        public async Task GetWithPredicateTest()
        {
            var hotel = await _hotelRepository.GetAsync(s => s.HotelName == "ABC Hotel");
            Assert.NotNull(hotel);
        }

        [Fact]
        public async Task GetAsyncWithPredicateTest()
        {
            var hotel = await _hotelRepository.GetAsync(s => s.HotelName == "ABC Hotel");
            Assert.NotNull(hotel);
        }


        [Fact]
        public async Task GetByIdAsyncTest()
        {
            var hotels = await _hotelRepository.GetAllAsync();
            Assert.NotNull(hotels);

            var hotelId = hotels.FirstOrDefault().Id;
            var nRoomType = await _hotelRepository.GetByIdAsync(hotelId);
            Assert.NotNull(nRoomType);
            Assert.Equal(hotelId, nRoomType.Id);
        }

        [Fact]
        public void AddNewTest()
        {
            var hotel = new Hotel
            {
                HotelName = "Hotel Demo",
                Address = "1234 Address",
                Email = "hotel.d@abc.om",
                Phone = "012-345-6789",
                Fax = "012-345-6789",
                PostalCode = "12345"
            };

            hotel.Created(Guid.NewGuid());

            _hotelRepository.AddAsync(hotel);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public async Task AddNewAsyncTest()
        {
            var hotel = new Hotel
            {
                HotelName = "Hotel Demo",
                Address = "1234 Address",
                Email = "hotel.d@abc.om",
                Phone = "012-345-6789",
                Fax = "012-345-6789",
                PostalCode = "12345"
            };

            hotel.Created(Guid.NewGuid());

            await _hotelRepository.AddAsync(hotel);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public void AddNewRangeTest()
        {
            var hotels = new List<Hotel>
            {
                new Hotel
                {
                    HotelName = "Hotel Demo A",
                    Address = "1234 Address",
                    Email = "hotel.d@abc.om",
                    Phone = "012-345-6789",
                    Fax = "012-345-6789",
                    PostalCode = "12345"
                },
                new Hotel
                {
                    HotelName = "Hotel Demo B",
                    Address = "1234 Address",
                    Email = "hotel.d@abc.om",
                    Phone = "012-345-6789",
                    Fax = "012-345-6789",
                    PostalCode = "12345"
                },
            };
            hotels.ForEach(s => s.Created(Guid.NewGuid()));

            _hotelRepository.AddRangeAsync(hotels);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public async Task AddNewRangeAsyncTest()
        {
            var hotels = new List<Hotel>
            {
                new Hotel
                {
                    HotelName = "Hotel Demo A",
                    Address = "1234 Address",
                    Email = "hotel.d@abc.om",
                    Phone = "012-345-6789",
                    Fax = "012-345-6789",
                    PostalCode = "12345"
                },
                new Hotel
                {
                    HotelName = "Hotel Demo B",
                    Address = "1234 Address",
                    Email = "hotel.d@abc.om",
                    Phone = "012-345-6789",
                    Fax = "012-345-6789",
                    PostalCode = "12345"
                },
            };

            hotels.ForEach(s => s.Created(Guid.NewGuid()));

            await _hotelRepository.AddRangeAsync(hotels);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public async Task UpdateTest()
        {
            var hotel = await _hotelRepository.GetAsync(s => s.HotelName == "ABC Hotel");

            Assert.NotNull(hotel);
            hotel.Modified(Guid.NewGuid());

            _hotelRepository.Update(hotel);
            Assert.True(_context.SaveChangesAsync().Result);
        }


        [Fact]
        public async Task RemoveTest()
        {
            var hotel = await _hotelRepository.GetAsync(s => s.HotelName == "ABC Hotel");
            Assert.NotNull(hotel);

            _hotelRepository.Remove(hotel);
            Assert.True(_context.SaveChangesAsync().Result);
        }
    }
}