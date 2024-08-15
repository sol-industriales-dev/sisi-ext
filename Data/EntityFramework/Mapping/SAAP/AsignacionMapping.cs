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
    class AsignacionMapping : EntityTypeConfiguration<tblSAAP_Asignacion>
    {
        public AsignacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.agrupacion_id).HasColumnName("agrupacion_id");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.actividad_id).HasColumnName("actividad_id");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.usuarioCreacion_id).HasColumnName("usuarioCreacion_id");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.usuarioModificacion_id).HasColumnName("usuarioModificacion_id");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.registroActivo).HasColumnName("registroActivo");

            ToTable("tblSAAP_Asignacion");
        }
    }
}
