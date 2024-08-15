using Core.Enum.RecursosHumanos.Vacaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Vacaciones
{
    public class VacacionesGestionDTO
    {
        public int id { get; set; }
        public int idVacaciones { get; set; }
        public int idUsuario { get; set; }
        public OrdenGestionEnum orden { get; set; }
        public GestionEstatusEnum estatus { get; set; }
        public string nombreCompleto { get; set; }
        public string firmaElect { get; set; }

    }
}
