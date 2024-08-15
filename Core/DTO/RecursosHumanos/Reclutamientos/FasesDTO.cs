using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class FasesDTO
    {
        public int id { get; set; }
        public string nombreFase { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
        public int cantActividades { get; set; }
        public string Item1 { get; set; }
        public string Item2 { get; set; }
    }
}
