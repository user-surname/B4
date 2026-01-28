namespace B4.Models.Entities.DataEntities

{
    public class Usuario
    {

        public int Id { get; set; }
        public String Email { get; set; }

        public String HashedPassword { get; set; }

        public String Role { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        protected Usuario() { }

        protected Usuario(int id, String email, String password, String role, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            Email = email;
            HashedPassword = password;
            Role = role;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
