using B4.Models.Entities.DataEntities;
using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces;
using B4.Models.RepositoryInterfaces.LkInterfaces;
using B4.Models.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Domain.Services
{
    public class UsuariosService : IUsuariosService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuariosService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _usuarioRepository.GetByIdAsync(id);
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _usuarioRepository.GetByEmailAsync(email);
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _usuarioRepository.GetAllAsync();
        }

        public async Task AddAsync(Usuario usuario)
        {
            await _usuarioRepository.AddAsync(usuario);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _usuarioRepository.GetByIdAsync(id);

            if (existing == null)
                throw new Exception($"No existe ciclo con id={id}");

            await _usuarioRepository.DeleteAsync(id);
        }
    }
}
