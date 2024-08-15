using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.BackLogs
{
    public class tblBL_MotivoCancelacionReq
    {
        public int id { get; set; }
        public int idBL { get; set; }
        public int idUsuario { get; set; }
        public virtual tblP_Usuario lstUsuarios { get; set; }
        public string motivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
