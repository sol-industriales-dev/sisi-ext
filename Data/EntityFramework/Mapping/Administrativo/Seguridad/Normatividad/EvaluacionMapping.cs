using Core.Entity.Administrativo.Seguridad.Requerimientos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Requerimientos
{
    public class EvaluacionMapping : EntityTypeConfiguration<tblNOM_Evaluacion>
    {
        public EvaluacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.evidenciaID).HasColumnName("evidenciaID");
            Property(x => x.indicadorID).HasColumnName("indicadorID");
            Property(x => x.aprobado).HasColumnName("aprobado");
            Property(x => x.calificacion).HasColumnName("calificacion");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblNOM_Evaluacion");
        }
    }
}
