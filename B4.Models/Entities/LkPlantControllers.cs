using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities
{
    public class LkPlantControllers
    {
        public int IdCompanyController { get; set; }
        public int IdCompany { get; set; }
        public string Controller { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Constructor vacío
        public LkPlantControllers() { }

        // Constructor con parámetros
        public LkPlantControllers(int idCompanyController, int idCompany, string controller, string email)
        {
            IdCompanyController = idCompanyController;
            IdCompany = idCompany;
            Controller = controller;
            Email = email;
        }
    }
}