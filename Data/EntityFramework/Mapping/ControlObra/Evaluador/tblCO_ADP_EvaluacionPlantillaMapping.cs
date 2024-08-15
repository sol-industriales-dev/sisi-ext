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
    public class tblCO_ADP_EvaluacionPlantillaMapping : EntityTypeConfiguration<tblCO_ADP_EvaluacionPlantilla>
    {
        public tblCO_ADP_EvaluacionPlantillaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.nombrePlantilla).HasColumnName("nombrePlantilla");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.contratos).HasColumnName("contratos");

            ToTable("tblCO_ADP_EvaluacionPlantilla");
        }
    }
}
