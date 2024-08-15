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
    public class FirmaEvaluacionDetalleMapping : EntityTypeConfiguration<tblX_FirmaEvaluacionDetalle>
    {
        public FirmaEvaluacionDetalleMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.firmaEvaluacion).WithMany().HasForeignKey(x => x.firmaEvaluacionId);
            HasRequired(x => x.firmante).WithMany().HasForeignKey(x => x.firmanteId);
            HasRequired(x => x.usuarioCreacion).WithMany().HasForeignKey(x => x.usuarioCreacionId);
            ToTable("tblX_FirmaEvaluacionDetalle");
        }
    }
}
