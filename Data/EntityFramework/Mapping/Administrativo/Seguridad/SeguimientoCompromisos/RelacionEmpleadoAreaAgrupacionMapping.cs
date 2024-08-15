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
    class RelacionEmpleadoAreaAgrupacionMapping : EntityTypeConfiguration<tblSC_RelacionEmpleadoAreaAgrupacion>
    {
        public RelacionEmpleadoAreaAgrupacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.usuario_id).HasColumnName("usuario_id");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.agrupacion_id).HasColumnName("agrupacion_id");
            Property(x => x.tipoUsuario).HasColumnName("tipoUsuario");
            Property(x => x.usuarioCreacion_id).HasColumnName("usuarioCreacion_id");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.usuarioModificacion_id).HasColumnName("usuarioModificacion_id");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.registroActivo).HasColumnName("registroActivo");

            ToTable("tblSC_RelacionEmpleadoAreaAgrupacion");
        }
    }
}
