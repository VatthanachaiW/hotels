using System.Collections.ObjectModel;

namespace Hotels.Entities.Masters
{
    public class RoomType : BaseEntity
    {
        public string RoomTypeName { get; set; }

        public Collection<Room> Rooms { get; set; }
    }
}