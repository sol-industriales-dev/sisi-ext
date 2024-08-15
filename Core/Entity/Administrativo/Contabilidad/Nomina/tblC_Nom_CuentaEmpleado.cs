using Core.DTO;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_CuentaEmpleado : InfoRegistroTablaDTO
    {
        public int id { get; set; }
        public int numero { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public int cuentaId { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public int digito { get; set; }
        public string cuentaDescripcion { get; set; }
        public string cc { get; set; }
        public string ccDescripcion { get; set; }
        public bool validada { get; set; }
        public int? usuarioValidoId { get; set; }
        public DateTime? fechaValidacion { get; set; }

        [ForeignKey("cuentaId")]
        public virtual tblC_Nom_Cuenta cuenta { get; set; }

        [ForeignKey("usuarioValidoId")]
        public virtual tblP_Usuario usuarioValido { get; set; }
    }
}
