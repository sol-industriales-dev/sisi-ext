using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.CtrlPptalOficinasCentrales
{
    public class IngresosGastosDTO
    {
        public int pPoliza { get; set; }
        public string poliza { get; set; }
        public int linea { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string cc { get; set; }
        public string descripcion { get; set; }
        public int mes { get; set; }
        public string mesTexto { get; set; }
        public decimal gastoMensual { get; set; }
        public decimal ingresoMensual { get; set; }
        public string concepto { get; set; }
        public int area { get; set; }
        public int cuenta_oc { get; set; }
        public string areaCuenta { get; set; }
        public decimal gastoAnioPasado { get; set; }
    }
}
