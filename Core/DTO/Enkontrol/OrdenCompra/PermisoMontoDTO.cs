using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class PermisoMontoDTO
    {
        public string cc { get; set; }
        public int area { get; set; }
        public int cuenta { get; set; }
        public int? tblPrincipalNumAutorizaciones { get; set; }
        public decimal monto_minimo_autoriza { get; set; }
        public decimal monto_maximo_autoriza { get; set; }
        public int consecutivo { get; set; }
        public int? empleadoVobo { get; set; }
        public string empleadoVoboDesc { get; set; }
        public int? tblVoboNumAutorizaciones { get; set; }
        public int? tipoGrupo { get; set; }
        public int? tblGruposNumAutorizaciones { get; set; }
        public int? empleadoAutoriza { get; set; }
        public string empleadoAutorizaDesc { get; set; }
        public int? tblAutNumAutorizaciones { get; set; }
        public int? ordenAutorizacion { get; set; }
    }
}
