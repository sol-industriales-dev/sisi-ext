using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura
{
    public class MaquinariaRentadaMapping : EntityTypeConfiguration<tblM_MaquinariaRentada>
    {
        public MaquinariaRentadaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Folio).HasColumnName("Folio");
            Property(x => x.NoEconomico).HasColumnName("NoEconomico");
            Property(x => x.Equipo).HasColumnName("Equipo");
            Property(x => x.NoSerie).HasColumnName("NoSerie");
            Property(x => x.Modelo).HasColumnName("Modelo");
            Property(x => x.IdProveedor).HasColumnName("IdProveedor");
            Property(x => x.Proveedor).HasColumnName("Proveedor");
            Property(x => x.LlegadaObra).HasColumnName("LlegadaObra");
            Property(x => x.CC).HasColumnName("CC");
            Property(x => x.Obra).HasColumnName("Obra");
            Property(x => x.FechaFacturacion).HasColumnName("FechaFacturacion");
            Property(x => x.RecepcionFactura).HasColumnName("RecepcionFactura");
            Property(x => x.NoFactura).HasColumnName("NoFactura");
            Property(x => x.DepGarantia).HasColumnName("DepGarantia");
            Property(x => x.TramiteDG).HasColumnName("TramiteDG");
            Property(x => x.NotaCredito).HasColumnName("NotaCredito");
            Property(x => x.BaseHoraMensual).HasColumnName("BaseHoraMensual");
            Property(x => x.PeriodoDel).HasColumnName("PeriodoDel");
            Property(x => x.PeriodoA).HasColumnName("PeriodoA");
            Property(x => x.HorometroInicial).HasColumnName("HorometroInicial");
            Property(x => x.HorometroFinal).HasColumnName("HorometroFinal");
            Property(x => x.HorasTrabajadas).HasColumnName("HorasTrabajadas");
            Property(x => x.HorasExtras).HasColumnName("HorasExtras");
            Property(x => x.CostoHorasExtras).HasColumnName("CostoHorasExtras");
            Property(x => x.TotalHorasExtras).HasColumnName("TotalHorasExtras");
            Property(x => x.PrecioMes).HasColumnName("PrecioMes");
            Property(x => x.SeguroMes).HasColumnName("SeguroMes");
            Property(x => x.IVA).HasColumnName("IVA");
            Property(x => x.TotalRenta).HasColumnName("TotalRenta");
            Property(x => x.REQ).HasColumnName("REQ");
            Property(x => x.FechaReq).HasColumnName("FechaReq");
            Property(x => x.OrdenCompra).HasColumnName("OrdenCompra");
            Property(x => x.FechaOrdenCompra).HasColumnName("FechaOrdenCompra");
            Property(x => x.ContraRecibo).HasColumnName("ContraRecibo");
            Property(x => x.FechaContraRecibo).HasColumnName("FechaContraRecibo");
            Property(x => x.Anotaciones).HasColumnName("Anotaciones");
            Property(x => x.DifHora).HasColumnName("DifHora");
            Property(x => x.Moneda).HasColumnName("Moneda");
            Property(x => x.DifHoraHoraExtra).HasColumnName("DifHoraHoraExtra");
            Property(x => x.DifHoraContraRecibo).HasColumnName("DifHoraContraRecibo");
            Property(x => x.DifHoraFactura).HasColumnName("DifHoraFactura");
            Property(x => x.DifHoraOrdenCompra).HasColumnName("DifHoraOrdenCompra");
            Property(x => x.DifHoraFecha).HasColumnName("DifHoraFecha");
            Property(x => x.CargoDaño).HasColumnName("CargoDaño");
            Property(x => x.CargoDañoHoraExtra).HasColumnName("CargoDañoHoraExtra");
            Property(x => x.CargoDañoContraRecibo).HasColumnName("CargoDañoContraRecibo");
            Property(x => x.CargoDañoFactura).HasColumnName("CargoDañoFactura");
            Property(x => x.CargoDañoOrdenCompra).HasColumnName("CargoDañoOrdenCompra");
            Property(x => x.CargoDañoFecha).HasColumnName("CargoDañoFecha");
            Property(x => x.Fletes).HasColumnName("Fletes");
            Property(x => x.FletesHoraExtra).HasColumnName("FletesHoraExtra");
            Property(x => x.FletesNoFactura).HasColumnName("FletesNoFactura");
            Property(x => x.FletesContraRecibo).HasColumnName("FletesContraRecibo");
            Property(x => x.FletesOrdenCompra).HasColumnName("FletesOrdenCompra");
            Property(x => x.FletesFecha).HasColumnName("FletesFecha");
            ToTable("tblM_MaquinariaRentada");
        }
    }
}
