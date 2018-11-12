using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hotels.Entities.Masters;
using Hotels.IServices;
using Hotels.IUnitOfWorks;
using Hotels.Utilities;
using Hotels.ViewModels;
using Microsoft.Extensions.Logging;

namespace Hotels.Services
{
    public class RoomTypeService : BaseService, IRoomTypeService
    {
        public RoomTypeService(IApplicationUnitOfWork unitOfWork, ILogger<BaseService> logger) : base(unitOfWork,
            logger)
        {
            MapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RoomType, RoomTypeViewModel>().ForMember(destination =>
                    destination.RoomTypeId, ops => ops.MapFrom(source => source.Id));

                cfg.CreateMap<RoomTypeViewModel, RoomType>().ForMember(destination =>
                    destination.Id, ops => ops.MapFrom(source => source.RoomTypeId));
            });
        }

        public List<RoomTypeViewModel> GetAllRoomTypes()
        {
            return UnitOfWork.RoomTypeRepository.GetAll().Select(s => new RoomTypeViewModel
            {
                RoomTypeId = s.Id,
                RoomTypeName = s.RoomTypeName,
                CreateBy = s.CreateBy
            }).ToList();
        }

        public async Task<List<RoomTypeViewModel>> GetAllRoomTypesAsync()
        {
            var roomTypes = await UnitOfWork.RoomTypeRepository.GetAllAsync();

            return roomTypes.Select(s => new RoomTypeViewModel
            {
                RoomTypeId = s.Id,
                RoomTypeName = s.RoomTypeName,
                CreateBy = s.CreateBy
            }).ToList();
        }

        public RoomTypeViewModel GetRoomTypeById(Guid roomTypeId)
        {
            IMapper iMapper = MapperConfiguration.CreateMapper();
            return iMapper.Map<RoomType, RoomTypeViewModel>(UnitOfWork.RoomTypeRepository.GetById(roomTypeId));
        }

        public async Task<RoomTypeViewModel> GetRoomTypeByIdAsync(Guid roomTypeId)
        {
            IMapper iMapper = MapperConfiguration.CreateMapper();

            var roomType = await UnitOfWork.RoomTypeRepository.GetByIdAsync(roomTypeId);
            return iMapper.Map<RoomType, RoomTypeViewModel>(roomType);
        }

        public async Task<bool> AddRoomType(RoomTypeViewModel roomType)
        {
            try
            {
                if (ObjectValidate.IsAnyNullOrEmpty(roomType)) throw new Exception("Invalid data exception");

                IMapper iMapper = MapperConfiguration.CreateMapper();
                var tmp = iMapper.Map<RoomTypeViewModel, RoomType>(roomType);
                await UnitOfWork.RoomTypeRepository.AddAsync(tmp);
                return await UnitOfWork.CommitAsync();
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, $"Error exception on insert new item {roomType}");
                UnitOfWork.Rollback();
                UnitOfWork.Dispose();

                throw;
            }
        }

        public async Task<bool> UpdateRoomType(RoomTypeViewModel roomType)
        {
            try
            {
                if (roomType == null) throw new DataException("Invalid data exception");

                var exist = await UnitOfWork.RoomTypeRepository.GetByIdAsync(roomType.RoomTypeId.Value);
                if (exist != null)
                {
                    exist.RoomTypeName = roomType.RoomTypeName;
                    exist.Modified(roomType.UpdateBy.Value);

                    UnitOfWork.RoomTypeRepository.Update(exist);
                    return await UnitOfWork.CommitAsync();
                }

                throw new DataException("Data not found");
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, $"Error exception on insert new item {roomType}");
                UnitOfWork.Rollback();
                UnitOfWork.Dispose();

                throw;
            }
        }

        public async Task<bool> InActiveRoomType(RoomTypeViewModel roomType)
        {
            try
            {
                if (roomType == null) throw new DataException("Invalid data exception");

                var exist = await UnitOfWork.RoomTypeRepository.GetByIdAsync(roomType.RoomTypeId.Value);
                if (exist != null)
                {
                    exist.IsActive = false;
                    exist.Modified(roomType.UpdateBy.Value);

                    UnitOfWork.RoomTypeRepository.Update(exist);
                    return await UnitOfWork.CommitAsync();
                }

                throw new DataException("Data not found");
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, $"Error exception on insert new item {roomType}");
                UnitOfWork.Rollback();
                UnitOfWork.Dispose();

                throw;
            }
        }
    }
}