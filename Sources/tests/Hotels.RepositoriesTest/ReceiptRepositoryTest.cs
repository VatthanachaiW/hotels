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
    public class ReceiptRepositoryTest
    {
        private IApplicationDbContext _context;
        private IRoomRepository _roomRepository;
        private IRoomPriceRepository _roomPriceRepository;
        private IReceiptRepository _receiptRepository;

        public ReceiptRepositoryTest()
        {
            _context = new ApplicationDbContextHelper().DbContext();
            _roomRepository = new RoomRepository(_context);
            _roomPriceRepository = new RoomPriceRepository(_context);
            _receiptRepository = new ReceiptRepository(_context);
        }


        [Fact]
        public async Task GetAllAsyncTest()
        {
            var receipt = await _receiptRepository.GetAllAsync();
            Assert.True(receipt.Any());
        }

        [Fact]
        public async Task GetAllAsyncWithPredicateTest()
        {
            var room = await _roomRepository.GetAsync(f => f.RoomCode == "R0010");
            var receipt = await _receiptRepository.GetAllAsync(w => w.RoomId == room.Id);
            Assert.True(receipt.Any());
        }

        [Fact]
        public async Task GetAsyncWithPredicateTest()
        {
            var room = await _roomRepository.GetAsync(f => f.RoomCode == "R0010");
            var receipt = await _receiptRepository.GetAsync(s => s.RoomId == room.Id);
            Assert.NotNull(receipt);
        }

        [Fact]
        public async Task GetByIdTest()
        {
            var receipt = await _receiptRepository.GetAllAsync();
            Assert.NotNull(receipt);
            var receiptId = receipt.FirstOrDefault().Id;
            var nHotel = await _receiptRepository.GetByIdAsync(receiptId);
            Assert.NotNull(nHotel);
            Assert.Equal(receiptId, nHotel.Id);
        }

        [Fact]
        public async Task GetByIdAsyncTest()
        {
            var receipt = await _receiptRepository.GetAllAsync();
            Assert.NotNull(receipt);

            var receiptId = receipt.FirstOrDefault().Id;
            var nHotel = await _receiptRepository.GetByIdAsync(receiptId);

            Assert.NotNull(nHotel);
            Assert.Equal(receiptId, nHotel.Id);
        }

        [Fact]
        public async Task AddNewAsyncTest()
        {
            var room = await _roomRepository.GetAsync(f => f.RoomCode == "R0010");
            Assert.NotNull(room);

            var price = await _roomPriceRepository.GetAsync(s =>
                s.RoomId == room.Id && s.PriceDate.Date == DateTime.Now.Date);
            Assert.NotNull(price);

            var receipt = new Receipt
            {
                CheckIn = DateTime.Now,
                CheckOut = DateTime.Now.AddDays(1),
                RoomId = room.Id,
                PriceId = price.Id,

                Firstname = "Customer",
                Lastname = "A",
                Address = "1234 ABC",
                Email = "cus.a@abc.com",
                PostalCode = "12345",
                Mobile = "012-345-6789",
                ReceiptDate = DateTime.Now.AddDays(1),
                ReceiptNo = $"R-{DateTime.Now:ddmmyyyy}-000001",
                PaidPrice = 700
            };

            receipt.Created(Guid.NewGuid());

            await _receiptRepository.AddAsync(receipt);
            Assert.True(_context.SaveChangesAsync().Result);
        }


        [Fact]
        public async Task AddNewRangeAsyncTest()
        {
            var room = await _roomRepository.GetAsync(f => f.RoomCode == "R0010");
            Assert.NotNull(room);

            var price = await _roomPriceRepository.GetAsync(s =>
                s.RoomId == room.Id && s.PriceDate.Date == DateTime.Now.Date);
            Assert.NotNull(price);

            var receipts = new List<Receipt>
            {
                new Receipt
                {
                    CheckIn = DateTime.Now,
                    CheckOut = DateTime.Now.AddDays(1),
                    RoomId = room.Id,
                    PriceId = price.Id,

                    Firstname = "Customer",
                    Lastname = "A",
                    Address = "1234 ABC",
                    Email = "cus.a@abc.com",
                    PostalCode = "12345",
                    Mobile = "012-345-6789",
                    ReceiptDate = DateTime.Now.AddDays(1),
                    ReceiptNo = $"R-{DateTime.Now:ddmmyyyy}-000001",
                    PaidPrice = 700
                },
                new Receipt
                {
                    CheckIn = DateTime.Now,
                    CheckOut = DateTime.Now.AddDays(1),
                    RoomId = room.Id,
                    PriceId = price.Id,

                    Firstname = "Customer",
                    Lastname = "A",
                    Address = "1234 ABC",
                    Email = "cus.a@abc.com",
                    PostalCode = "12345",
                    Mobile = "012-345-6789",
                    ReceiptDate = DateTime.Now.AddDays(1),
                    ReceiptNo = $"R-{DateTime.Now:ddmmyyyy}-000002",
                    PaidPrice = 750
                },
            };
            receipts.ForEach(s => s.Created(Guid.NewGuid()));

            await _receiptRepository.AddRangeAsync(receipts);
            Assert.True(_context.SaveChangesAsync().Result);
        }


        [Fact]
        public async Task UpdateTest()
        {
            var receipt = await _receiptRepository.GetAsync(s => s.ReceiptNo == $"R-{DateTime.Now:ddmmyyyy}-000001");

            Assert.NotNull(receipt);
            receipt.Modified(Guid.NewGuid());

            _receiptRepository.Update(receipt);
            Assert.True(_context.SaveChangesAsync().Result);
        }


        [Fact]
        public async Task RemoveTest()
        {
            var receipt = await _receiptRepository.GetAsync(s => s.ReceiptNo == $"R-{DateTime.Now:ddmmyyyy}-000001");
            Assert.NotNull(receipt);

            _receiptRepository.Remove(receipt);
            Assert.True(_context.SaveChangesAsync().Result);
        }
    }
}