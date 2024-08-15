using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.SeguimientoAcuerdos
{
    public class PromoverDTO
    {
        public int id { get; set; }
        public string minuta { get; set; }
        public int actividadID { get; set; }
        public string actividad { get; set; }
        public string descripcion { get; set; }
        public string observacion { get; set; }
        public string fechaRegistro { get; set; }
        public int columna { get; set; }
        public string responsable { get; set; }
    }
}
