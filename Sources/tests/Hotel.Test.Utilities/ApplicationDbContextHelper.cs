using System;
using System.Collections.Generic;
using System.Linq;
using Hotels.DbConnections;
using Hotels.Entities.Masters;
using Hotels.Entities.Profiles;
using Hotels.IDbConnections;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hotels.TestUtilities
{
    public class ApplicationDbContextHelper
    {
        public IApplicationDbContext DbContext()
        {
            // กำหนดให้ใช้ Database แบบ In-Memory
            var option = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid()
                    .ToString())
                .EnableSensitiveDataLogging()
                .Options;

            var context = new ApplicationDbContext(option);

            //User Initial
            UserInit(context);

            //RoomType Initial
            RoomTypeInit(context);

            //Room Initial
            RoomInit(context);

            //RoomPrice Initial
            RoomPriceInit(context);

            //Receipt Initial
            ReceiptInit(context);
            return context;
        }

        private void UserInit(ApplicationDbContext context)
        {
            string useradmin = "admin@hoteldemo.com";

            var user = new ApplicationUser
            {
                UserName = useradmin,
                Email = useradmin,
                Firstname = "Admin",
                Lastname = "System",
                EmailConfirmed = true,
                LockoutEnabled = false
            };

            context.Set<ApplicationUser>().Add(user);
        }
        //RoomType Initial
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

        //Room Initial
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
                    Description = "Sample Room Demo",
                    RoomTypeId = roomtypeA.Id,
                },
                new Room
                {
                    RoomCode = "R0020",
                    Description = "Sample Room Demo",
                    RoomTypeId = roomtypeB.Id,
                },
                new Room
                {
                    RoomCode = "R0030",
                    Description = "Sample Room Demo",
                    RoomTypeId = roomtypeC.Id,
                }
            };

            rooms.ForEach(s => s.Created(Guid.NewGuid()));
            context.Set<Room>().AddRange(rooms);
            context.SaveChanges();
        }

        //RoomPrice Initial
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

        //Receipt Initial
        private void ReceiptInit(ApplicationDbContext context)
        {
            var roomA = context.Set<Room>().FirstOrDefault(s => s.RoomCode == "R0010");
            var priceA = context.Set<RoomPrice>().FirstOrDefault(s => s.RoomId == roomA.Id);

            var receipts = new List<Receipt>
            {
                new Receipt
                {
                    RoomId = roomA.Id,
                    ReceiptNo = $"{DateTime.Now:ddmmyyyy}-00001",
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