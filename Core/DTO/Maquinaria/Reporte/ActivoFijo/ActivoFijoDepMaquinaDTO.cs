using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoDepMaquinaDTO
    {
        public int IdDepMaquina { get; set; }
        public int IdCatMaquina { get; set; }
        public string CC { get; set; }
        public string NoEconomico { get; set; }
        public string AreaCuenta { get; set; }
        public string Descripcion { get; set; }
        public string TipoMovimiento { get; set; }
        public string TipoActivo { get; set; }
        public decimal Monto { get; set; }
        public string Factura { get; set; }
        public string Poliza { get; set; }
        public decimal DepreciacionSemanal { get; set; }
        public decimal DepreciacionMensual { get; set; }
        public decimal DepreciacionAcumulada { get; set; }
        public int MesesTotalesDepreciacion { get; set; }
        public int MesesFaltantes { get; set; }
        public decimal ValorLibro { get; set; }
        public decimal PorcentajeDepreciacion { get; set; }
        public DateTime FechaInicioDepreciacion { get; set; }
        public DateTime? FechaBaja { get; set; }
        public bool EsExtraCatMaqDep { get; set; }
        public int semanasDepreciacionOH_14_1 { get; set; }
        public decimal depreciacionOH_14_1 { get; set; }
    }
}