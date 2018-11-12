using System;

namespace Hotels.ViewModels
{
    public class RoomTypeViewModel
    {
        public Guid RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public Guid CreateBy { get; set; }
        public Guid UpdateBy { get; set; }
    }
}