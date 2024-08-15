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
    public class tblCO_ADP_EvaluacionRelMapping : EntityTypeConfiguration<tblCO_ADP_EvaluacionRel>
    {
        public tblCO_ADP_EvaluacionRelMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.idSubContratista).HasColumnName("idSubContratista");
            Property(x => x.Preguntar).HasColumnName("Preguntar");
            Property(x => x.idReq).HasColumnName("idReq");
            Property(x => x.idAsignacion).HasColumnName("idAsignacion");

            ToTable("tblCO_ADP_EvaluacionRel");
        }
    }
}