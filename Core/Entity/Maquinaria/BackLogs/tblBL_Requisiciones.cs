using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.BackLogs
{
    public class tblBL_Requisiciones
    {
        public int id { get; set; }
        public int idBackLog { get; set; }
        public string numRequisicion { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacionRequisicion { get; set; }
        public DateTime? fechaModificacionRequisicion { get; set; }
        public bool esActivo { get; set; }
    }
}
