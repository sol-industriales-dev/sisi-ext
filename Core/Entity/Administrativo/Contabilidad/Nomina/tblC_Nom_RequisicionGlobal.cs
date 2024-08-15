using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.Nomina;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_RequisicionGlobal
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int numero { get; set; }
        public TipoRequisicionGlobalEnum tipoRequisicion { get; set; }
        public bool registroActivo { get; set; }
    }
}
