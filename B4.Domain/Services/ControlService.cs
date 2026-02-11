using B4.Models.Entities;
using B4.Models.RepositoryInterfaces;
using B4.Models.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace B4.Domain.Services
{
    public class ControlService : IControlService
    {
        private readonly IControlRepository _controlRepository;

        public ControlService(IControlRepository controlRepository)
        {
            _controlRepository = controlRepository;
        }

        // -------------------------------------------------
        // Obtener por ID
        // -------------------------------------------------
        public async Task<Control?> GetByIdAsync(int id)
        {
            return await _controlRepository.GetByIdAsync(id);
        }

        // -------------------------------------------------
        // Obtener todos
        // -------------------------------------------------
        public async Task<IEnumerable<Control>> GetAllAsync()
        {
            return await _controlRepository.GetAllAsync();
        }

        // -------------------------------------------------
        // Agregar nuevo control
        // -------------------------------------------------
        public async Task AddAsync(Control control)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            // Si quieres inicializar campos por defecto, puedes hacerlo aquí
            if (control.Activo == null)
                control.Activo = true; // ejemplo

            await _controlRepository.AddAsync(control);
        }

        // -------------------------------------------------
        // Actualizar control existente
        // -------------------------------------------------
        public async Task UpdateAsync(Control control)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            var existing = await _controlRepository.GetByIdAsync(control.IdControl);

            if (existing == null)
                throw new Exception($"No existe control con id={control.IdControl}");

            await _controlRepository.UpdateAsync(control);
        }

        // -------------------------------------------------
        // Eliminar control
        // -------------------------------------------------
        public async Task DeleteAsync(int id)
        {
            var existing = await _controlRepository.GetByIdAsync(id);

            if (existing == null)
                throw new Exception($"No existe control con id={id}");

            await _controlRepository.DeleteAsync(id);
        }
    }
}
