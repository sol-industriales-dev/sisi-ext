using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
   public class EmpleadoOTDTO
    {
        public int id { get; set; }
        public string  NombreE { get; set; }
        public string PuestoE { get; set; }
        public decimal HorasTrabajo { get; set; }
        public string Accion { get; set; }
        public int PersonalID { get; set; }
        public int OrdenTrabajoID { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFin { get; set; }
        public int Tipo { get; set; }

    }
}
