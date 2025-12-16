namespace B4.Models.Entities.DataEntities

{
    public abstract class DataBase
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Version { get; set; }
        public int Checksum { get; set; }
        public int IsZero { get; set; }

        protected DataBase() { }

        protected DataBase(DateTime createdAt, DateTime updatedAt, int version, int checksum, int isZero)
        {
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            Version = version;
            Checksum = checksum;
            IsZero = isZero;
        }
    }
}
