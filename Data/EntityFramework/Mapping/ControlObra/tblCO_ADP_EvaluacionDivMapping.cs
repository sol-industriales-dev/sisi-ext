using Core.Entity.ControlObra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.ControlObra
{
    public class tblCO_ADP_EvaluacionDivMapping : EntityTypeConfiguration<tblCO_ADP_EvaluacionDiv>
    {
        public tblCO_ADP_EvaluacionDivMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.idbutton).HasColumnName("idbutton");
            Property(x => x.idsection).HasColumnName("idsection");
            Property(x => x.orden).HasColumnName("orden");
            Property(x => x.SubContratista).HasColumnName("SubContratista");
            Property(x => x.toltips).HasColumnName("toltips");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.idEvaluador).HasColumnName("idEvaluador");
            Property(x => x.idPlantilla).HasColumnName("idPlantilla");

            ToTable("tblCO_ADP_EvaluacionDiv");
        }
    }
}
