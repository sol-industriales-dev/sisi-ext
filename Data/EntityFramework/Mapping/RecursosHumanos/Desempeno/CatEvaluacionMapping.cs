using Core.Entity.RecursosHumanos.Desempeno;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Desempeno
{
    public class CatEvaluacionMapping : EntityTypeConfiguration<tblRH_ED_CatEvaluacion>
    {
        public CatEvaluacionMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idProceso).HasColumnName("idProceso");
            Property(x => x.calificacion).HasColumnName("calificacion");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.fechaFin).HasColumnName("fechaFin");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            HasRequired(x => x.proceso).WithMany().HasForeignKey(y => y.idProceso);
            ToTable("tblRH_ED_CatEvaluacion");
        }
    }
}
