using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.Requisicion
{
    public class tblCom_ReqDet
    {
        public int id { get; set; }
        public int idReq { get; set; }
        public int partida { get; set; }
        public int insumo { get; set; }
        public DateTime requerido { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio { get; set; }
        public decimal cantOrdenada { get; set; }
        public DateTime ordenada { get; set; }
        public string estatus { get; set; }
        public decimal cantCancelada { get; set; }
        public string referencia { get; set; }
        public decimal cantExcedida { get; set; }
        public int area { get; set; }
        public int cuenta { get; set; }
        public string descripcion { get; set; }
        public string observaciones { get; set; }
        public string comentarioSurtidoQuitar { get; set; }
        public bool estatusRegistro { get; set; }
        public decimal PERU_saldo { get; set; }
        public string PERU_ordenFabricacion { get; set; }
        public string PERU_tipoRequisicion { get; set; }
        public string noEconomico { get; set; }
        public virtual List<tblCom_ReqDet_Comentarios> comentarios { get; set; }
    }
}
