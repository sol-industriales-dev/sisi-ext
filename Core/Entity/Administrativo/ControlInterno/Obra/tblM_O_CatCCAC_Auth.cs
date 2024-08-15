using Core.Entity.Principal.Usuarios;
using Core.Enum.Principal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.ControlInterno.Obra
{
    public class tblM_O_CatCCAC_Auth
    {
        public int id { get; set; }
        public int idCatalogo { get; set; }
        public int idUsuario { get; set; }
        public int orden { get; set; }
        public DateTime fechaFirma { get; set; }
        public string firma { get; set; }
        public authEstadoEnum authEstado { get; set; }
        public string motivoRechazo { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
        [ForeignKey("idUsuario")]
        public virtual tblP_Usuario usuario { get; set; }
    }
}
