using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Usuarios
{
    public class tblP_RolesGrupoTrabajo
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public int cantDiasLaborales { get; set; }
        public int cantDiasDescanso { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
