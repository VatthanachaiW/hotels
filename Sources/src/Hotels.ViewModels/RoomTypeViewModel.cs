using System;
using System.ComponentModel.DataAnnotations;

namespace Hotels.ViewModels
{
    public class RoomTypeViewModel
    {
        public Guid? RoomTypeId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string RoomTypeName { get; set; }

        public Guid CreateBy { get; set; }
        public Guid? UpdateBy { get; set; }
    }
}