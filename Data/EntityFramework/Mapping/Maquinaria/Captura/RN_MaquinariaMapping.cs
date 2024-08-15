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
    public class RN_MaquinariaMapping : EntityTypeConfiguration<tblM_RN_Maquinaria>
    {
        public RN_MaquinariaMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.Folio).HasColumnName("Folio");
            Property(x => x.PeriodoInicial).HasColumnName("PeriodoInicial");
            Property(x => x.IdCentroCosto).HasColumnName("IdCentroCosto");
            Property(x => x.IdProveedor).HasColumnName("IdProveedor");
            Property(x => x.IdAreaCuenta).HasColumnName("IdAreaCuenta");
            Property(x => x.NumFactura).HasColumnName("NumFactura");
            Property(x => x.DepGarantia).HasColumnName("DepGarantia");
            Property(x => x.TramiteDG).HasColumnName("TramiteDG");
            Property(x => x.NotaCredito).HasColumnName("NotaCredito");
            Property(x => x.AplicaNC).HasColumnName("AplicaNC");
            Property(x => x.BaseHoraMensual).HasColumnName("BaseHoraMensual");
            Property(x => x.PeriodoDel).HasColumnName("PeriodoDel");
            Property(x => x.PeriodoA).HasColumnName("PeriodoA");
            Property(x => x.HorometroInicial).HasColumnName("HorometroInicial");
            Property(x => x.HorometroFinal).HasColumnName("HorometroFinal");
            Property(x => x.HorasTrabajadas).HasColumnName("HorasTrabajadas");
            Property(x => x.HorasExtras).HasColumnName("HorasExtras");
            Property(x => x.TotalHorasExtras).HasColumnName("TotalHorasExtras");
            Property(x => x.PrecioPorMes).HasColumnName("PrecioPorMes");
            Property(x => x.SeguroPorMes).HasColumnName("SeguroPorMes");
            Property(x => x.IVA).HasColumnName("IVA");
            Property(x => x.TotalRenta).HasColumnName("TotalRenta");
            Property(x => x.OrdenCompra).HasColumnName("OrdenCompra");
            Property(x => x.ContraRecibo).HasColumnName("ContraRecibo");
            Property(x => x.Anotaciones).HasColumnName("Anotaciones");
            Property(x => x.IdTipoMoneda).HasColumnName("IdTipoMoneda");
            Property(x => x.DifHora).HasColumnName("DifHora");
            Property(x => x.DifHoraExtra).HasColumnName("DifHoraExtra");
            Property(x => x.DifContraRecibo).HasColumnName("DifContraRecibo");
            Property(x => x.DifFactura).HasColumnName("DifFactura");
            Property(x => x.DifOrdenCompra).HasColumnName("DifOrdenCompra");
            Property(x => x.DifFechaContraRecibo).HasColumnName("DifFechaContraRecibo");
            Property(x => x.CargoDaño).HasColumnName("CargoDaño");
            Property(x => x.CargoDañoFactura).HasColumnName("CargoDañoFactura");
            Property(x => x.CargoDañoOrdenCompra).HasColumnName("CargoDañoOrdenCompra");
            Property(x => x.Fletes).HasColumnName("Fletes");
            Property(x => x.FletesFactura).HasColumnName("FletesFactura");
            Property(x => x.FletesOrdenCompra).HasColumnName("FletesOrdenCompra");
            Property(x => x.Activo).HasColumnName("Activo");
            Property(x => x.FechaCreacion).HasColumnName("FechaCreacion");
            Property(x => x.FechaModificacion).HasColumnName("FechaModificacion");
            Property(x => x.IdUsuario).HasColumnName("IdUsuario");
            HasRequired(x => x.AreaCuenta).WithMany().HasForeignKey(d => d.IdAreaCuenta);
            HasRequired(x => x.Usuario).WithMany().HasForeignKey(d => d.IdUsuario);
            HasRequired(x => x.CC).WithMany().HasForeignKey(d => d.IdCentroCosto);
            ToTable("tblM_RN_Maquinaria");
        }
    }
}