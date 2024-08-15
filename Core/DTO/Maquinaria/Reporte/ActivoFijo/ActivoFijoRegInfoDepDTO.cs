using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoRegInfoDepDTO
    {
        public int IdCatMaquinaDepreciacion { get; set; }
        public int Año { get; set; }
        public int Mes { get; set; }
        public int Poliza { get; set; }
        public string TP { get; set; }
        public int Linea { get; set; }
        public int TM { get; set; }
        public int Cuenta { get; set; }
        public int Subcuenta { get; set; }
        public int SubSubcuenta { get; set; }
        public string Factura { get; set; }
        public decimal Monto { get; set; }
        public DateTime? FechaFactura { get; set; }
        public DateTime FechaPol { get; set; }
        public string Concepto { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public int TipoActivo { get; set; }
        public int IdCatMaquina { get; set; }
        public DateTime? FechaInicioDepreciacion { get; set; }
        public decimal? PorcentajeDepreciacion { get; set; }
        public int? MesesTotalesDepreciacion { get; set; }
        public int TipoDelMovimiento { get; set; }
        public string PolizaRefAlta { get; set; }
        public string AreaCuenta { get; set; }
        public bool CapturaPorSistema { get; set; }

        public string insumo { get; set; }
    }
}