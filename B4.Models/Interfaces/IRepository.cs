using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Interfaces
{
    // T debe ser una clase
    public interface IRepository<T> where T : class
    {
        // C - Create
        Task AddAsync(T entity);

        // R - Read
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();

        // U - Update
        Task UpdateAsync(T entity);

        // D - Delete
        Task DeleteAsync(int id);
    }

    // Clase de Dominio de ejemplo (POCO)
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
