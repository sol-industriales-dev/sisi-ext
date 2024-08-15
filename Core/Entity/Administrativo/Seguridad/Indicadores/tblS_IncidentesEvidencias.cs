using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
    public class tblS_IncidentesEvidencias
    {
        public int id { get; set; }
        public int informe_id { get; set; }
        public virtual tblS_IncidentesInformePreliminar informe { get; set; }
        public string nombre { get; set; }
        public string ruta { get; set; }
        public int numero { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioCreadorID { get; set; }
        public virtual tblP_Usuario usuario { get; set; }
        public bool activa { get; set; }
    }
}
