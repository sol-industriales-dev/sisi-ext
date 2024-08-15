using Core.Entity.SeguimientoCompromisos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.SeguimientoCompromisos
{
    class EvidenciaMapping : EntityTypeConfiguration<tblSC_Evidencia>
    {
        public EvidenciaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.agrupacion_id).HasColumnName("agrupacion_id");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.actividad_id).HasColumnName("actividad_id");
            Property(x => x.consecutivo).HasColumnName("consecutivo");
            Property(x => x.rutaEvidencia).HasColumnName("rutaEvidencia");
            Property(x => x.progresoEstimado).HasColumnName("progresoEstimado");
            Property(x => x.comentariosCaptura).HasColumnName("comentariosCaptura");
            Property(x => x.usuarioEvaluador_id).HasColumnName("usuarioEvaluador_id");
            Property(x => x.comentariosEvaluador).HasColumnName("comentariosEvaluador");
            Property(x => x.progreso).HasColumnName("progreso");
            Property(x => x.terminacion).HasColumnName("terminacion");
            Property(x => x.fechaEvaluacion).HasColumnName("fechaEvaluacion");
            Property(x => x.usuarioCreacion_id).HasColumnName("usuarioCreacion_id");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.usuarioModificacion_id).HasColumnName("usuarioModificacion_id");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.registroActivo).HasColumnName("registroActivo");

            ToTable("tblSC_Evidencia");
        }
    }
}
