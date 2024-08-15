using Core.Entity.Administrativo.Contratistas;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
    public class tblS_IncidentesRelEmpresaContratistas
    {
        public int id { get; set; }
        public int idEmpresa { get; set; }
        public int idContratista { get; set; }
        public virtual tblP_Usuario empleadosContratistas { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
    }
}
