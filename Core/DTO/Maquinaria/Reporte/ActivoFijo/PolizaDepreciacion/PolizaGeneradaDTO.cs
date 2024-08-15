using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion
{
    public class PolizaGeneradaDTO
    {
        public int Año { get; set; }
        public int Mes { get; set; }
        public int Semana { get; set; }
        public int Dia { get; set; }
        public int Poliza { get; set; }
        public string TipoPoliza { get; set; }
        public int Linea { get; set; }
        public int Cuenta { get; set; }
        public int Subcuenta { get; set; }
        public int SubSubcuenta { get; set; }
        public int RelacionCuentaAñoId { get; set; }
        public int Digito { get; set; }
        public int TipoMovimiento { get; set; }
        public string Referencia { get; set; }
        public string CC { get; set; }
        public int? CatMaquinaId { get; set; }
        public string NumeroEconomico { get; set; }
        public string Concepto { get; set; }
        public decimal Monto { get; set; }
        public int IClave { get; set; }
        public int ITM { get; set; }
        public int? CcId { get; set; }
        public int? Area { get; set; }
        public int? Cuenta_OC { get; set; }
        public string AreaCuenta { get; set; }
        public string AreaCuentaDescripcion { get; set; }

        public bool esOH141 { get; set; }
    }
}