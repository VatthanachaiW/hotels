using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotels.Entities.Masters
{
    public class BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public bool IsActive { get; set; }

        public Guid CreateBy { get; private set; }
        [DataType(DataType.DateTime)] public DateTime CreateOn { get; private set; }

        public Guid? ModifiedBy { get; private set; }
        [DataType(DataType.DateTime)] public DateTime? ModifiedOn { get; private set; }

        public void Created(Guid createBy)
        {
            CreateBy = createBy;
            CreateOn = DateTime.UtcNow;
        }

        public void Modified(Guid modifiedBy)
        {
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTime.UtcNow;
        }
    }
}