using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotels.Entities.Masters;
using Hotels.IDbConnections;
using Hotels.IRepositories;
using Hotels.Repositories;
using Hotels.TestUtilities;
using Microsoft.Extensions.Localization.Internal;
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
        public void GetAllTest()
        {
            Assert.True(_receiptRepository.GetAll().Any());
        }

        [Fact]
        public void GetAllWithPredicateTest()
        {
            var room = _roomRepository.Get(f => f.RoomCode == "R0010");
            Assert.True(_receiptRepository.GetAll(w => w.RoomId == room.Id).Any());
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
            var room = _roomRepository.Get(f => f.RoomCode == "R0010");
            var receipt = await _receiptRepository.GetAllAsync(w => w.RoomId == room.Id);
            Assert.True(receipt.Any());
        }

        [Fact]
        public void GetWithPredicateTest()
        {
            var room = _roomRepository.Get(f => f.RoomCode == "R0010");
            var receipt = _receiptRepository.Get(s => s.RoomId == room.Id);
            Assert.NotNull(receipt);
        }

        [Fact]
        public async Task GetAsyncWithPredicateTest()
        {
            var room = _roomRepository.Get(f => f.RoomCode == "R0010");
            var receipt = await _receiptRepository.GetAsync(s => s.RoomId == room.Id);
            Assert.NotNull(receipt);
        }

        [Fact]
        public void GetByIdTest()
        {
            var receipt = _receiptRepository.GetAll().FirstOrDefault();
            Assert.NotNull(receipt);

            var nHotel = _receiptRepository.GetById(receipt.Id);
            Assert.NotNull(nHotel);
            Assert.Equal(receipt.Id, nHotel.Id);
        }

        [Fact]
        public async Task GetByIdAsyncTest()
        {
            var receipt = _receiptRepository.GetAll().FirstOrDefault();
            Assert.NotNull(receipt);

            var nReceipt = await _receiptRepository.GetByIdAsync(receipt.Id);
            Assert.NotNull(nReceipt);
            Assert.Equal(receipt.Id, nReceipt.Id);
        }

        [Fact]
        public void AddNewTest()
        {
            var room = _roomRepository.Get(f => f.RoomCode == "R0010");
            var price = _roomPriceRepository.Get(s => s.RoomId == room.Id && s.PriceDate.Date == DateTime.Now.Date);

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
                ReceiptNo = $"R-{DateTime.Now:yyyyMMdd}-000001",
                PaidPrice = 700
            };

            receipt.Created(Guid.NewGuid());

            _receiptRepository.Add(receipt);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public async Task AddNewAsyncTest()
        {
            var room = _roomRepository.Get(f => f.RoomCode == "R0010");
            var price = _roomPriceRepository.Get(s => s.RoomId == room.Id && s.PriceDate.Date == DateTime.Now.Date);

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
                ReceiptNo = $"R-{DateTime.Now:yyyyMMdd}-000001",
                PaidPrice = 700
            };

            receipt.Created(Guid.NewGuid());

            await _receiptRepository.AddAsync(receipt);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public void AddNewRangeTest()
        {
            var room = _roomRepository.Get(f => f.RoomCode == "R0010");
            var price = _roomPriceRepository.Get(s => s.RoomId == room.Id && s.PriceDate.Date == DateTime.Now.Date);

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
                    ReceiptNo = $"R-{DateTime.Now:yyyyMMdd}-000001",
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
                    ReceiptNo = $"R-{DateTime.Now:yyyyMMdd}-000002",
                    PaidPrice = 750
                },
            };
            receipts.ForEach(s => s.Created(Guid.NewGuid()));

            _receiptRepository.AddRange(receipts);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public async Task AddNewRangeAsyncTest()
        {
            var room = _roomRepository.Get(f => f.RoomCode == "R0010");
            var price = _roomPriceRepository.Get(s => s.RoomId == room.Id && s.PriceDate.Date == DateTime.Now.Date);

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
                    ReceiptNo = $"R-{DateTime.Now:yyyyMMdd}-000001",
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
                    ReceiptNo = $"R-{DateTime.Now:yyyyMMdd}-000002",
                    PaidPrice = 750
                },
            };
            receipts.ForEach(s => s.Created(Guid.NewGuid()));


            await _receiptRepository.AddRangeAsync(receipts);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public void UpdateTest()
        {
            var receipt = _receiptRepository.Get(s => s.ReceiptNo == $"R-{DateTime.Now:yyyyMMdd}-000001");

            Assert.NotNull(receipt);
            receipt.Modified(Guid.NewGuid());

            _receiptRepository.Update(receipt);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public void UpdateRangeTest()
        {
            var receipt = _receiptRepository.GetAll(s => s.ReceiptNo.Contains($"R-{DateTime.Now:yyyyMMdd}"));
            Assert.NotNull(receipt);

            receipt.ForEach(s => s.Modified(Guid.NewGuid()));
            _receiptRepository.UpdateRange(receipt);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public void RemoveTest()
        {
            var receipt = _receiptRepository.Get(s => s.ReceiptNo == $"R-{DateTime.Now:yyyyMMdd}-000001");
            Assert.NotNull(receipt);

            _receiptRepository.Remove(receipt);
            Assert.True(_context.SaveChangesAsync().Result);
        }

        [Fact]
        public void RemoveRangeTest()
        {
            var receipt = _receiptRepository.GetAll(s => s.ReceiptNo.Contains($"R-{DateTime.Now:yyyyMMdd}"));
            Assert.NotNull(receipt);

            _receiptRepository.RemoveRange(receipt);
            Assert.True(_context.SaveChangesAsync().Result);
        }
    }
}