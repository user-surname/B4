using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities.LkEntities
{
    public abstract class LkBase
    {

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long IsActive { get; set; }

        public LkBase() {}

        public LkBase(DateTime createdAt, DateTime updatedAt, long isActive)
        {
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            IsActive = isActive;
        }

    }
}
