using Core.Entity.SAAP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.SAAP
{
    class ActividadMapping : EntityTypeConfiguration<tblSAAP_Actividad>
    {
        public ActividadMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.areaEvaluadora).HasColumnName("areaEvaluadora");
            Property(x => x.clasificacion).HasColumnName("clasificacion");
            Property(x => x.porcentaje).HasColumnName("porcentaje");
            Property(x => x.diasCompromiso).HasColumnName("diasCompromiso");
            Property(x => x.usuarioCreacion_id).HasColumnName("usuarioCreacion_id");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.usuarioModificacion_id).HasColumnName("usuarioModificacion_id");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.registroActivo).HasColumnName("registroActivo");

            ToTable("tblSAAP_Actividad");
        }
    }
}
