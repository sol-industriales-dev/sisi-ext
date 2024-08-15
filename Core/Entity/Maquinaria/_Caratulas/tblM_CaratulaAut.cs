using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria._Caratulas
{
    public class tblM_CaratulaAut
    {
        public int id { get; set; }
        public int idCaratula { get; set; }
        [ForeignKey("idCaratula")]
        public virtual tblM_CaratulaDet lstCaratula { get; set; }
        public bool esAutorizado { get; set; }
        public string firma { get; set; }
        public DateTime? fechaAutorizacion { get; set; }
        public string comentario { get; set; }
        public int claveAutorizante { get; set; }
        public int idUsuarioTecnico { get; set; }
        [ForeignKey("idUsuarioTecnico")]
        public virtual tblP_Usuario lstUsuariosTecnico { get; set; }
        public int idUsuarioServicio { get; set; }
        [ForeignKey("idUsuarioServicio")]
        public virtual tblP_Usuario lstUsuariosServicio { get; set; }
        public int idUsuarioConstruccion { get; set; }
        [ForeignKey("idUsuarioConstruccion")]
        public virtual tblP_Usuario lstUsuariosConstruccion { get; set; }       
        public int estatus { get; set; }
        public string nombreAutorizante { get; set; }
        public int orden { get; set; }
        public int idAlerta { get; set; }
    }
}
