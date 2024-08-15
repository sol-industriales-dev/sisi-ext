using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoDetalleDTO
    {
        public string Factura { get; set; }
        public string Poliza { get; set; }
        public int Cuenta { get; set; }
        public int Subcuenta { get; set; }
        public int SubSubcuenta { get; set; }
        public bool EsOverhaul { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaInicioDepreciacion { get; set; }
        public int MesesMaximoDepreciacion { get; set; }
        public string Cc { get; set; }
        public string Clave { get; set; }
        public string Descripcion { get; set; }
        public decimal MOI { get; set; }
        public decimal Altas { get; set; }
        public decimal Overhaul { get; set; }
        public DateTime? FechaCancelacion { get; set; }
        public decimal MontoCancelacion { get; set; }
        public DateTime? FechaBaja { get; set; }
        public decimal MontoBaja { get; set; }
        public decimal PorcentajeDepreciacion { get; set; }
        public decimal DepreciacionMensual { get; set; }
        public int MesesDepreciadosAñoAnterior { get; set; }
        public int MesesDepreciadosAñoActual { get; set; }
        public decimal DepreciacionAcumuladaAñoAnterior { get; set; }
        public decimal DepreciacionAñoActual { get; set; }
        public decimal BajaDepreciacion { get; set; }
        public decimal DepreciacionContableAcumulada { get; set; }
        public decimal ValorEnLibros { get; set; }
        public int? Area { get; set; }
        public int? Cuenta_OC { get; set; }
        public string AreaCuenta { get; set; }
        public string AreaCuentaDescripcion { get; set; }
        public bool faltante { get; set; }
        public DateTime? FechaBajaPol { get; set; }
        public DateTime? FechaCancelacionPol { get; set; }
        public int MesesDepreciadosAñoAnteriorParaDiferencias { get; set; }
        public bool AltaSigoplan { get; set; }
        public bool BajaSigoplan { get; set; }
        public bool CancelacionSigoplan { get; set; }
        public int AñoPol_alta { get; set; }
        public int MesPol_alta { get; set; }
        public int PolPol_alta { get; set; }
        public string TpPol_alta { get; set; }
        public int LineaPol_alta { get; set; }
        public int? AñoPol_baja { get; set; }
        public int? MesPol_baja { get; set; }
        public int? PolPol_baja { get; set; }
        public string TpPol_baja { get; set; }
        public int? LineaPol_baja { get; set; }
        public bool DepreciacionTerminadaPorMeses { get; set; }
        public int? IdDepMaquina { get; set; }
        public int? IdCatMaquina { get; set; }
        public string TipoMovimiento { get; set; }
        public int IdTipoMovimiento { get; set; }
        public string TipoActivo { get; set; }
        public bool EsExtraCatMaqDep { get; set; }

        public string Insumo { get; set; }
        public bool NuevoInsumo { get; set; }
        public bool esDepreciacionEspecialFija { get; set; }

        public int semanasDepreciacionOverhaul14_1 { get; set; }
        public decimal depreciacionOverhaul14_1 { get; set; }

        public bool esOverhaul14_1 { get; set; }

        public bool esOverhaul14_1Herradura { get; set; }
    }
}