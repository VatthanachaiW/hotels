using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hotels.ViewModels;

namespace Hotels.IServices
{
    public interface IRoomTypeService
    {
        List<RoomTypeViewModel> GetAllRoomTypes();
        Task<List<RoomTypeViewModel>> GetAllRoomTypesAsync();

        RoomTypeViewModel GetRoomTypeById(Guid roomTypeId);
        Task<RoomTypeViewModel> GetRoomTypeByIdAsync(Guid roomTypeId);

        Task<bool> AddRoomType(RoomTypeViewModel roomType);
        Task<bool> UpdateRoomType(RoomTypeViewModel roomType);
        Task<bool> InActiveRoomType(RoomTypeViewModel roomType);
    }
}