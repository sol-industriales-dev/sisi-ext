using Core.Enum.Contabilidad.EstadoFinanciero;
using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.EstadoFinanciero
{
    public class tblEF_ClienteProveedorRelacion
    {
        public int id { get; set; }
        public EmpresaEnum empresaId { get; set; }
        public EmpresaEnum empresaRelacionadaId { get; set; }
        public int numeroRelacionado { get; set; }
        public TipoRelacionEnum tipoRelacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
