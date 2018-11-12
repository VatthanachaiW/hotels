using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Hotels.IServices;
using Hotels.Services;
using Hotels.TestUtilities;
using Hotels.UnitOfWorks;
using Hotels.ViewModels;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Hotels.ServicesTest
{
    public class RoomTypeServiceTest
    {
        private readonly IRoomTypeService _roomTypeService;

        public RoomTypeServiceTest()
        {
            var mockLogger = new Mock<ILogger<RoomTypeService>>();
            var dbContext = new ApplicationDbContextHelper().DbContext();
            var unitOfWork = new ApplicationUnitOfWork(dbContext);
            _roomTypeService = new RoomTypeService(unitOfWork, mockLogger.Object);
        }

        [Fact]
        public void GetAllRoomTypeTest()
        {
            Assert.NotNull(_roomTypeService.GetAllRoomTypes());
        }

        [Fact]
        public async Task GetAllRoomTypeAsyncTest()
        {
            var result = await _roomTypeService.GetAllRoomTypesAsync();
            Assert.NotNull(result);
        }

        [Fact]
        public void GetRoomTypeTest()
        {
            var roomType = _roomTypeService.GetAllRoomTypes().FirstOrDefault();
            Assert.NotNull(roomType);

            Assert.NotNull(_roomTypeService.GetRoomTypeById(roomType.RoomTypeId.Value));
        }

        [Fact]
        public async Task GetRoomTypeAsyncTest()
        {
            var roomType = _roomTypeService.GetAllRoomTypes().FirstOrDefault();
            Assert.NotNull(roomType);
            var result = await _roomTypeService.GetRoomTypeByIdAsync(roomType.RoomTypeId.Value);

            Assert.NotNull(result);
        }

        [Fact]
        public void AddNewRoomTypeTest()
        {
            var roomType = new RoomTypeViewModel
            {
                RoomTypeName = "Room Type Service Test",
                CreateBy = Guid.NewGuid()
            };

            Assert.True(_roomTypeService.AddRoomType(roomType).Result);
        }

        [Fact]
        public void AddNewRoomTypeThrowExceptionTest()
        {
            RoomTypeViewModel roomType = new RoomTypeViewModel();
            Assert.Throws<AggregateException>(() => _roomTypeService.AddRoomType(roomType).Result);
        }

        [Fact]
        public void UpdateRoomTypeTest()
        {
            var roomType = _roomTypeService.GetAllRoomTypes().FirstOrDefault(s => s.RoomTypeName == "Room Type B");

            Assert.NotNull(roomType);

            roomType.UpdateBy = Guid.NewGuid();

            Assert.True(_roomTypeService.UpdateRoomType(roomType).Result);
        }

        [Fact]
        public void UpdatewRoomTypeThrowExceptionTest()
        {
            RoomTypeViewModel roomType = null;
            Assert.Throws<AggregateException>(() => _roomTypeService.UpdateRoomType(roomType).Result);
        }

        [Fact]
        public void InActiveRoomTypeTest()
        {
            var roomType = _roomTypeService.GetAllRoomTypes().FirstOrDefault(s => s.RoomTypeName == "Room Type C");
            Assert.NotNull(roomType);

            roomType.UpdateBy = Guid.NewGuid();

            Assert.True(_roomTypeService.InActiveRoomType(roomType).Result);
        }

        [Fact]
        public void InActiveRoomTypeThrowExceptionTest()
        {
            RoomTypeViewModel roomType = null;
            Assert.Throws<AggregateException>(() => _roomTypeService.InActiveRoomType(roomType).Result);
        }
    }
}