using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Overhaul;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class DGDTO
    {
        public int id { get; set; }
        public int idActividad { get; set; }
        public string descripcion { get; set; }
        public string comentario { get; set; }
        public decimal duracion { get; set; }
        public string fechaInicio { get; set; }
        public string fechaInicioP { get; set; }
        public string fechaEjecucion { get; set; }
        public string comentarioRE { get; set; }
        public DateTime fechaInicioRaw { get; set; }
        public List<tblM_ComentarioActividadOverhaul> archivos { get; set; }
        public string fechaFin { get; set; }
    }
}
