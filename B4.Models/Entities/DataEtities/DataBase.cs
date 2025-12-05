using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities.DataEtities
{
    public abstract class DataBase
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Version { get; set; }
        public int? Checksum { get; set; }
        public int IsZero { get; set; }

        public DataBase() { }  

        public DataBase(
            DateTime createdAt,
            DateTime updatedAt,
            int version,
            int? checksum,
            int isZero
            ) {
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            Version = version;
            Checksum = checksum;
            IsZero = isZero;
        }
    }
}
