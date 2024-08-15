using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion
{
    public class PolizaDetalleDTO
    {
        public DateTime FechaPoliza { get; set; }
        public int Año { get; set; }
        public int Mes { get; set; }
        public string TipoPoliza { get; set; }
        public int Poliza { get; set; }
        public string DescripcionCuenta { get; set; }
        public int Linea { get; set; }
        public int Cuenta { get; set; }
        public int Subcuenta { get; set; }
        public int SubSubcuenta { get; set; }
        public int TipoMovimiento { get; set; }
        public int iTipoMovimiento { get; set; }
        public string Referencia { get; set; }
        public string CC { get; set; }
        public string DescripcionCC { get; set; }
        public string Concepto { get; set; }
        public decimal Monto { get; set; }
        public int? Area { get; set; }
        public int? Cuenta_OC { get; set; }
        public string AreaCuenta { get; set; }
    }
}