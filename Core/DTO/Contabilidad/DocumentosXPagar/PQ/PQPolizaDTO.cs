using Core.Enum.Administracion.DocumentosXPagar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.DocumentosXPagar.PQ
{
    public class PQPolizaDTO
    {
        public string cuenta { get; set; }
        public string cuentaDescripcion { get; set; }
        public int tm { get; set; }
        public int referencia { get; set; }
        public string cc { get; set; }
        public string concepto { get; set; }
        public decimal monto { get; set; }
        public int itm { get; set; }
        public decimal interesDiario { get; set; }
        public int? tipoLinea { get; set; }
        public DateTime? fechaFirma { get; set; }
    }
}
