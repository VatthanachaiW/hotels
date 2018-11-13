using System;
using System.Collections.ObjectModel;

namespace Hotels.Entities.Masters
{
    public class Room : BaseEntity
    {
        public string RoomCode { get; set; }
        public string Description { get; set; }

        public Guid RoomTypeId { get; set; }
        public RoomType RoomType { get; set; }

        public Collection<RoomPrice> RoomPrices { get; set; }
        public Collection<Receipt> Receipts { get; set; }
    }
}