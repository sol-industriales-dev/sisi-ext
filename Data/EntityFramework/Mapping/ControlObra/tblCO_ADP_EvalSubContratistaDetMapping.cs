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
    public class tblCO_ADP_EvalSubContratistaDetMapping : EntityTypeConfiguration<tblCO_ADP_EvalSubContratistaDet>
    {
        public tblCO_ADP_EvalSubContratistaDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            
            Property(x => x.idEvaluacion).HasColumnName("idEvaluacion");
            Property(x => x.tipoEvaluacion).HasColumnName("tipoEvaluacion");
            Property(x => x.idRow).HasColumnName("idRow");
            Property(x => x.rutaArchivo).HasColumnName("rutaArchivo");
            Property(x => x.fechaDocumento).HasColumnName("fechaDocumento");

            Property(x => x.comentario).HasColumnName("comentario");
            Property(x => x.planesDeAccion).HasColumnName("planesDeAccion");
            Property(x => x.responsable).HasColumnName("responsable");
            Property(x => x.fechaCompromiso).HasColumnName("fechaCompromiso");
            Property(x => x.calificacion).HasColumnName("calificacion");
            Property(x => x.evaluacionPendiente).HasColumnName("evaluacionPendiente");
            Property(x => x.opcional).HasColumnName("opcional");

            HasRequired(x => x.evaluacionSubcontratista).WithMany().HasForeignKey(x => x.idEvaluacion);

            ToTable("tblCO_ADP_EvalSubContratistaDet");
        }
    }
}
