using System;
using System.Collections.Generic;
using System.Linq;
using Hotels.DbConnections;
using Hotels.Entities.Masters;
using Hotels.IDbConnections;
using Microsoft.EntityFrameworkCore;

namespace Hotels.TestUtilities
{
    public class ApplicationDbContextHelper
    {
        public IApplicationDbContext DbContext()
        {
            var option = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid()
                    .ToString())
                .EnableSensitiveDataLogging()
                .Options;

            var context = new ApplicationDbContext(option);

            RoomTypeInit(context);
            RoomInit(context);
            RoomPriceInit(context);
            ReceiptInit(context);
            HotelInit(context);
            return context;
        }

        private void HotelInit(ApplicationDbContext context)
        {
            var hotel = new Hotel
            {
                HotelName = "Wonder Swarm",
                Address = "1234 Address",
                Email = "w.swarm@abc.om",
                Phone = "012-345-6789",
                Fax = "012-345-6789",
                PostalCode = "12345"
            };

            context.Set<Hotel>().Add(hotel);
            context.SaveChanges();
        }

        private void RoomTypeInit(ApplicationDbContext context)
        {
            var roomTypes = new List<RoomType>
            {
                new RoomType
                {
                    RoomTypeName = "Room Type A"
                },
                new RoomType
                {
                    RoomTypeName = "Room Type B"
                },
                new RoomType
                {
                    RoomTypeName = "Room Type C"
                },
                new RoomType
                {
                    RoomTypeName = "Room Type D"
                },
                new RoomType
                {
                    RoomTypeName = "Room Type E"
                },
            };

            roomTypes.ForEach(s => s.Created(Guid.NewGuid()));

            context.Set<RoomType>().AddRange(roomTypes);
            context.SaveChanges();
        }

        private void RoomInit(ApplicationDbContext context)
        {
            var roomtypeA = context.Set<RoomType>().FirstOrDefault(s => s.RoomTypeName == "Room Type A");
            var roomtypeB = context.Set<RoomType>().FirstOrDefault(s => s.RoomTypeName == "Room Type B");
            var roomtypeC = context.Set<RoomType>().FirstOrDefault(s => s.RoomTypeName == "Room Type C");

            var rooms = new List<Room>
            {
                new Room
                {
                    RoomCode = "R0010",
                    Description = "Sample Room Demo AA",
                    RoomTypeId = roomtypeA.Id,
                },
                new Room
                {
                    RoomCode = "R0011",
                    Description = "Sample Room Demo AB",
                    RoomTypeId = roomtypeA.Id,
                },
                new Room
                {
                    RoomCode = "R0012",
                    Description = "Sample Room Demo AC",
                    RoomTypeId = roomtypeA.Id,
                },
                new Room
                {
                    RoomCode = "R0013",
                    Description = "Sample Room Demo AD",
                    RoomTypeId = roomtypeA.Id,
                },
                new Room
                {
                    RoomCode = "R0020",
                    Description = "Sample Room Demo B",
                    RoomTypeId = roomtypeB.Id,
                },
                new Room
                {
                    RoomCode = "R0030",
                    Description = "Sample Room Demo C",
                    RoomTypeId = roomtypeC.Id,
                }
            };

            rooms.ForEach(s => s.Created(Guid.NewGuid()));
            context.Set<Room>().AddRange(rooms);
            context.SaveChanges();
        }

        private void RoomPriceInit(ApplicationDbContext context)
        {
            var roomA = context.Set<Room>().FirstOrDefault(s => s.RoomCode == "R0010");
            var roomB = context.Set<Room>().FirstOrDefault(s => s.RoomCode == "R0020");

            var roomPrices = new List<RoomPrice>
            {
                new RoomPrice
                {
                    RoomId = roomA.Id,
                    Price = 500.25m,
                    PriceDate = DateTime.Now
                },
                new RoomPrice
                {
                    RoomId = roomB.Id,
                    Price = 800m,
                    PriceDate = DateTime.Now
                },
            };

            roomPrices.ForEach(s => s.Created(Guid.NewGuid()));
            context.Set<RoomPrice>().AddRange(roomPrices);
            context.SaveChanges();
        }

        private void ReceiptInit(ApplicationDbContext context)
        {
            var roomA = context.Set<Room>().FirstOrDefault(s => s.RoomCode == "R0010");
            var priceA = context.Set<RoomPrice>().FirstOrDefault(s => s.RoomId == roomA.Id);
            var receipts = new List<Receipt>
            {
                new Receipt
                {
                    RoomId = roomA.Id,
                    ReceiptNo = $"R-{DateTime.Now:yyyyMMdd}-000001",
                    ReceiptDate = DateTime.Now,
                    CheckIn = DateTime.Now,
                    CheckOut = DateTime.Now.AddDays(1),
                    PriceId = priceA.Id,

                    Firstname = "Robby",
                    Lastname = "Will",
                    Address = "123 Panama",
                    PostalCode = "12345",
                    Email = "robby.w@abc.com",
                    Mobile = "012-345-6789",
                    PaidPrice = 450m
                },
            };

            receipts.ForEach(s => s.Created(Guid.NewGuid()));
            context.Set<Receipt>().AddRange(receipts);
            context.SaveChanges();
        }
    }
}