using Core.Entity.RecursosHumanos.Evaluacion360;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.RecursosHumanos.Evaluacion360
{
    public class tblRH_Eval360_EvaluacionesEvaluadorDetMapping : EntityTypeConfiguration<tblRH_Eval360_EvaluacionesEvaluadorDet>
    {
        public tblRH_Eval360_EvaluacionesEvaluadorDetMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblRH_Eval360_EvaluacionesEvaluadorDet");
        }
    }
}