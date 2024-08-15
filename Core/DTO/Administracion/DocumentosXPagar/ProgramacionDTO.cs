using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.DocumentosXPagar
{
    public class ProgramacionDTO
    {
        public string cc { get; set; }
        public decimal importe { get; set; }
        public decimal importeDLLS { get; set; }
        public decimal capital { get; set; }
        public decimal intereses { get; set; }
        public decimal iva { get; set; }
        public decimal ivaInteres { get; set; }
        public decimal porcentaje { get; set; }
        public decimal total { get; set; }
        public string noEconomico { get; set; }
        public int? idCatMaquina { get; set; }
        public int parcialidad { get; set; }
        public decimal tipoCambio { get; set; }
        public string areaCuenta { get; set; }
        public bool liquidar { get; set; }
        public decimal? penaConvencional { get; set; }
        public bool opcionCompra { get; set; }
        public decimal? montoOpcionCompra { get; set; }
    }
}
