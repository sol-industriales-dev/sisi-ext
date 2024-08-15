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
    public class tblAF_DxP_PagoMaquinaMapping : EntityTypeConfiguration<tblAF_DxP_PagoMaquina>
    {
        public tblAF_DxP_PagoMaquinaMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.PagoId).HasColumnName("pagoId");
            Property(x => x.ContratoMaquinaId).HasColumnName("contratoMaquinaId");
            Property(x => x.Estatus).HasColumnName("estatus");
            Property(x => x.montoPagado).HasColumnName("montoPagado");
            Property(x => x.fechaPago).HasColumnName("fechaPago");
           // HasRequired(x => x.Pago).WithMany().HasForeignKey(y => y.PagoId);
          ///  HasRequired(x => x.ContratoMaquina).WithMany().HasForeignKey(y => y.ContratoMaquinaId);
            ToTable("tblAF_DxP_PagoMaquina");
        }
    }
}