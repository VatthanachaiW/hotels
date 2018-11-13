using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Hotels.Entities.Masters
{
    public class Hotel : BaseEntity
    {
        public string HotelName { get; set; }
        public string Address { get; set; }
        [DataType(DataType.PostalCode)] public string PostalCode { get; set; }
        [DataType(DataType.PhoneNumber)] public string Phone { get; set; }
        [DataType(DataType.PhoneNumber)] public string Fax { get; set; }
        public string Email { get; set; }
    }
}