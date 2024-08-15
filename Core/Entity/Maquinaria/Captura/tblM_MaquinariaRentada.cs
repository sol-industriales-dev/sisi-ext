using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Multiempresa;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Usuarios;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_MaquinariaRentada
    {
        public int id { get; set; }
        public string Folio { get; set; }
        public string NoEconomico { get; set; }
        public string Equipo { get; set; }
        public string NoSerie { get; set; }
        public string Modelo { get; set; }
        public int IdProveedor { get; set; }
        public string Proveedor { get; set; }
        public DateTime LlegadaObra { get; set; }
        public string CC { get; set; }
        public string Obra { get; set; }
        public DateTime FechaFacturacion { get; set; }
        public DateTime RecepcionFactura { get; set; }
        public string NoFactura { get; set; }
        public string DepGarantia { get; set; }
        public bool TramiteDG { get; set; }
        public string NotaCredito { get; set; }
        public bool AplicaNC { get; set; }
        public int BaseHoraMensual { get; set; }
        public DateTime PeriodoDel { get; set; }
        public DateTime PeriodoA { get; set; }
        public decimal HorometroInicial { get; set; }
        public decimal HorometroFinal { get; set; }
        public decimal HorasTrabajadas { get; set; }
        public decimal HorasExtras { get; set; }
        public decimal CostoHorasExtras { get; set; }
        public decimal TotalHorasExtras { get; set; }
        public decimal PrecioMes { get; set; }
        public decimal SeguroMes { get; set; }
        public decimal IVA { get; set; }
        public decimal TotalRenta  { get; set; }
        public string REQ { get; set; }
        public DateTime FechaReq { get; set; }
        public string OrdenCompra { get; set; }
        public DateTime FechaOrdenCompra { get; set; }
        public string ContraRecibo { get; set; }
        public DateTime FechaContraRecibo { get; set; }
        public string Anotaciones { get; set; }
        public bool DifHora { get; set; }
        public bool Moneda { get; set; }
        public decimal DifHoraHoraExtra { get; set; }
        public string DifHoraContraRecibo { get; set; }
        public string DifHoraFactura { get; set; }
        public string DifHoraOrdenCompra { get; set; }
        public DateTime DifHoraFecha { get; set; }
        public bool CargoDaño { get; set; }
        public decimal CargoDañoHoraExtra { get; set; }
        public string CargoDañoContraRecibo { get; set; }
        public string CargoDañoFactura { get; set; }
        public string CargoDañoOrdenCompra { get; set; }
        public DateTime CargoDañoFecha { get; set; }
        public bool Fletes { get; set; }
        public decimal FletesHoraExtra { get; set; }
        public string FletesContraRecibo { get; set; }
        public string FletesNoFactura { get; set; }
        public string FletesOrdenCompra { get; set; }
        public DateTime FletesFecha { get; set; }
    }
}
