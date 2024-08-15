using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.Seguridad.Indicadores;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
    public class tblS_IncidentesEvidenciasRIA
    {
        public int id { get; set; }
        public int incidente_id { get; set; }
        public virtual tblS_Incidentes incidente { get; set; }
        public string nombre { get; set; }
        public string ruta { get; set; }
        public int numero { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public int usuarioCreadorID { get; set; }
        public virtual tblP_Usuario usuario { get; set; }
        public TipoEvidenciaRIAEnum tipoEvidenciaRIA { get; set; }
        public bool estatus { get; set; }
    }
}
