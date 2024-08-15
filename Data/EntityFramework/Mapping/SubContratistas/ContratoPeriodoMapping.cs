using Core.Entity.SubContratistas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.SubContratistas
{
    public class ContratoPeriodoMapping : EntityTypeConfiguration<tblX_ContratoPeriodo>
    {
        public ContratoPeriodoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.contratoID).HasColumnName("contratoID");
            Property(x => x.periodoID).HasColumnName("periodoID");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.actualizacion).HasColumnName("actualizacion");
            Property(x => x.validado).HasColumnName("validado");
            Property(x => x.estatus).HasColumnName("estatus");

            HasRequired(x => x.contrato).WithMany().HasForeignKey(y => y.contratoID);
            HasRequired(x => x.periodo).WithMany().HasForeignKey(y => y.periodoID);

            ToTable("tblX_ContratoPeriodo");
        }
    }
}
