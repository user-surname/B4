namespace B4.Models.Entities.LkEntities
{
    public abstract class LkBase
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long IsActive { get; set; } // 1 = activo, 0 = inactivo

        public LkBase() { }

        public LkBase(DateTime createdAt, DateTime updatedAt, long isActive)
        {
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            IsActive = isActive;
        }

        public void Activate() => IsActive = 1;
        public void Deactivate() => IsActive = 0;
    }
}
