using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.MaquinariaRentada
{
    public class MaquinaRentadaDTO
    {
        public int Id { get; set; }
        public string Folio { get; set; }
        public int PeriodoInicial { get; set; }
        public int IdCentroCosto { get; set; }
        public string CentroCosto { get; set; }
        public long IdProveedor { get; set; }
        public string Proveedor { get; set; }
        public string Equipo { get; set; }
        public string NumeroSerie { get; set; }
        public string Modelo { get; set; }
        public int IdAreaCuenta { get; set; }
        public string AreaCuenta { get; set; }
        public string NumeroFactura { get; set; }
        public decimal? DepGarantia { get; set; }
        public bool TramiteDG { get; set; }
        public string NotaCredito { get; set; }
        public bool AplicaNC { get; set; }
        public int? BaseHoraMensual { get; set; }
        public DateTime PeriodoDel { get; set; }
        public DateTime PeriodoA { get; set; }
        public decimal HorometroInicial { get; set; }
        public decimal? HorometroFinal { get; set; }
        public decimal? HorasTrabajadas { get; set; }
        public decimal? HorasExtras { get; set; }
        public decimal? TotalHorasExtras { get; set; }
        public decimal? PrecioPorMes { get; set; }
        public decimal? SeguroPorMes { get; set; }
        public decimal IVA { get; set; }
        public decimal? TotalRenta { get; set; }
        public string OrdenCompra { get; set; }
        public string ContraRecibo { get; set; }
        public string Anotaciones { get; set; }
        public int IdTipoMoneda { get; set; }
        public bool DifHora { get; set; }
        public decimal? DifHoraExtra { get; set; }
        public string DifContraRecibo { get; set; }
        public string DifFactura { get; set; }
        public string DifOrdenCompra { get; set; }
        public DateTime? DifFechaContraRecibo { get; set; }
        public bool CargoDaño { get; set; }
        public string CargoDañoFactura { get; set; }
        public string CargoDañoOrdenCompra { get; set; }
        public bool Fletes { get; set; }
        public string FletesFactura { get; set; }
        public string FletesOrdenCompra { get; set; }
    }
}