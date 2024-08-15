using Core.Entity.Administrativo.DocumentosXPagar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.DocumentosXPagar
{
    public class tblAF_DxP_ContratoMaquinaDetalleMapping : EntityTypeConfiguration<tblAF_DxP_ContratoMaquinaDetalle>
    {
        public tblAF_DxP_ContratoMaquinaDetalleMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.ContratoMaquinaId).HasColumnName("contratoMaquinaId");
            Property(x => x.ContratoDetalleId).HasColumnName("contratoDetalleId");
            Property(x => x.Parcialidad).HasColumnName("parcialidad");
            Property(x => x.AmortizacionCapital).HasPrecision(24, 2).HasColumnName("amortizacionCapital");
            Property(x => x.IvaSCapital).HasPrecision(24, 2).HasColumnName("ivaSCapital");
            Property(x => x.Intereses).HasPrecision(24, 2).HasColumnName("intereses");
            Property(x => x.IvaIntereses).HasPrecision(24, 2).HasColumnName("ivaIntereses");
            Property(x => x.Importe).HasPrecision(24, 2).HasColumnName("importe");
            Property(x => x.Saldo).HasPrecision(24, 2).HasColumnName("saldo");
            Property(x => x.FechaVencimiento).HasColumnName("fechaVencimiento");
            Property(x => x.Pagado).HasColumnName("pagado");
            Property(x => x.FechaPago).HasColumnName("fechaPago");
            Property(x => x.GeneroInteresMoratorio).HasColumnName("generoInteresMoratorio");
            Property(x => x.Estatus).HasColumnName("estatus");
            HasRequired(x => x.ContratoMaquina).WithMany().HasForeignKey(y => y.ContratoMaquinaId);
            HasRequired(x => x.ContratoDetalle).WithMany().HasForeignKey(y => y.ContratoDetalleId);
            ToTable("tblAF_DxP_ContratoMaquinaDetalle");
        }
    }
}