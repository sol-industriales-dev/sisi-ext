using Core.Entity.ControlObra.Evaluacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.ControlObra.Evaluador
{
    public class tblCO_ADP_EvaluadorXccMapping : EntityTypeConfiguration<tblCO_ADP_EvaluadorXcc>
    {
        public tblCO_ADP_EvaluadorXccMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.evaluador).HasColumnName("evaluador");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.elementos).HasColumnName("elementos");
            Property(x => x.esActivo).HasColumnName("esActivo");

            ToTable("tblCO_ADP_EvaluadorXcc");
        }
    }
}
