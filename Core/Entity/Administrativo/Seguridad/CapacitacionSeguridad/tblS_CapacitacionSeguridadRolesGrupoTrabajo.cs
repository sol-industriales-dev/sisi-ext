using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridadRolesGrupoTrabajo
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public int cantDiasLaborales { get; set; }
        public int CantDiasDescando { get; set; }
        public int cantDiasLaborales2 { get; set; }
        public int CantDiasDescando2 { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool mixto { get; set; }
        public bool esActivo { get; set; }

    }
}
