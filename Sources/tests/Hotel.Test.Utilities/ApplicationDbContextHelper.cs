using System;
using System.Collections.Generic;
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

            var roomtypes = new List<RoomType>
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

            context.Set<RoomType>().AddRange(roomtypes);
            context.SaveChanges();

            return context;
        }
    }
}