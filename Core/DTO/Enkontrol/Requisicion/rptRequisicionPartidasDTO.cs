using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Requisicion
{
    public class rptRequisicionPartidasDTO
    {
        public string partida { get; set; }
        public string insumo { get; set; }
        public string areaCuenta { get; set; }
        public string fechaRequerido { get; set; }
        public string cantidadRequerida { get; set; }
        public string estatus { get; set; }
        public string ordenCompra { get; set; }
        public string fechaOrdenada { get; set; }
        public string cantidadOrdenada { get; set; }
        public string proveedor { get; set; }
    }
}
