using System;
using System.ComponentModel.DataAnnotations;

namespace Hotels.Entities.Masters
{
    public class Receipt : BaseEntity
    {
        public string ReceiptNo { get; set; }
        public DateTime ReceiptDate { get; set; }

        public Guid RoomId { get; set; }
        public Room Room { get; set; }

        public Guid PriceId { get; set; }
        public RoomPrice Price { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }

        public string ProvinceCode { get; set; }
        [DataType(DataType.PhoneNumber)] public string Mobile { get; set; }
        [DataType(DataType.EmailAddress)] public string Email { get; set; }

        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }

        public decimal PaidPrice { get; set; }
    }
}