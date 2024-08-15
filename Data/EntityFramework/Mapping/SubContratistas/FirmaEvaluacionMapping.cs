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
    public class FirmaEvaluacionMapping : EntityTypeConfiguration<tblX_FirmaEvaluacion>
    {
        public FirmaEvaluacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasOptional(x => x.firmantePendiente).WithMany().HasForeignKey(x => x.firmantePendienteId);
            HasRequired(x => x.usuarioCreacion).WithMany().HasForeignKey(x => x.usuarioCreacionId);
            HasRequired(x => x.usuarioModificacion).WithMany().HasForeignKey(x => x.usuarioModificacionId);
            HasRequired(x => x.evaluacion).WithMany().HasForeignKey(x => x.evaluacionId);
            ToTable("tblX_FirmaEvaluacion");
        }
    }
}
