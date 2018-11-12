using System;
using Hotels.DbConnections;
using Hotels.Entities.Masters;
using Hotels.IDbConnections;
using Hotels.IRepositories;
using Hotels.IUnitOfWorks;
using Hotels.Repositories;
using Hotels.TestUtilities;
using Hotels.UnitOfWorks;
using Xunit;

namespace Hotels.UnitOfWorkTest
{
    public class ApplicationUnitOfWorkTest
    {
        private IApplicationDbContext _context;

        private IApplicationUnitOfWork _unitOfWork;

        public ApplicationUnitOfWorkTest()
        {
            _context = new ApplicationDbContextHelper().DbContext();

            _unitOfWork = new ApplicationUnitOfWork(_context);
        }

        [Fact]
        public void UnitOfWorkGetItemTest()
        {
            var roomType = _unitOfWork.RoomTypeRepository.Get(p => p.RoomTypeName == "Room Type A");
            Assert.NotNull(roomType);
        }

        [Fact]
        public void UnitOfWorkGetAllItemTest()
        {
            var roomTypes = _unitOfWork.RoomTypeRepository.GetAll();
            Assert.NotNull(roomTypes);
        }

        [Fact]
        public void UnitOfWorkUpdateItemTest()
        {
            var roomType = _unitOfWork.RoomTypeRepository.Get(p => p.RoomTypeName == "Room Type A");
            Assert.NotNull(roomType);

            roomType.Modified(Guid.NewGuid());
            _unitOfWork.RoomTypeRepository.Update(roomType);

            Assert.True(_unitOfWork.CommitAsync().Result);
        }

        [Fact]
        public void UnitOfWorkRollBackTest()
        {
            var roomType = new RoomType
            {
                RoomTypeName = "Room"
            };


            roomType.Created(Guid.NewGuid());
            _unitOfWork.RoomTypeRepository.Add(roomType);
            _unitOfWork.Rollback();

            Assert.Null(_unitOfWork.RoomTypeRepository.Get(s => s.RoomTypeName == "Room"));
        }
    }
}