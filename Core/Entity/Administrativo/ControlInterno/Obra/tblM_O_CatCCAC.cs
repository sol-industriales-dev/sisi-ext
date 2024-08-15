using Core.Entity.Principal.Usuarios;
using Core.Enum.Administracion.ControlInterno.Obra;
using Core.Enum.Principal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.ControlInterno.Obra
{
    public class tblM_O_CatCCAC
    {
        public int id { get; set; }
        public int idUsuarioRegistro { get; set; }
        public tipoCatalogoEnum tipo { get; set; }
        public string clave { get; set; }
        public string descripcion { get; set; }
        public DateTime fechaArranque { get; set; }
        public authEstadoEnum authEstado { get; set; }
        public bool exiteEnkontrol { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
        [ForeignKey("idUsuarioRegistro")]
        public virtual tblP_Usuario usuario { get; set; }
        [ForeignKey("idCatalogo")]
        public virtual ICollection<tblM_O_CatCCAC_Auth> lstAuth { get; set; }
    }
}
