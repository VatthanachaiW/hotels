using System;
using System.Collections.ObjectModel;

namespace Hotels.Entities.Masters
{
    public class RoomPrice : BaseEntity
    {
        public Guid RoomId { get; set; }
        public Room Room { get; set; }

        public DateTime PriceDate { get; set; }
        public decimal Price { get; set; }

        public Collection<Receipt> Receipts { get; set; }
    }
}