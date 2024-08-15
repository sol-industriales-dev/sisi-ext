using Core.Entity.SeguimientoAcuerdos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.SeguimientoAcuerdos
{
    public class ActividadMapping : EntityTypeConfiguration<tblSA_Actividades>
    {
        public ActividadMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.minutaID).HasColumnName("minutaID");
            Property(x => x.columna).HasColumnName("columna");
            Property(x => x.orden).HasColumnName("orden");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.actividad).HasColumnName("actividad");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.responsableID).HasColumnName("responsableID");
            Property(x => x.responsable).HasColumnName("responsable");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.fechaCompromiso).HasColumnName("fechaCompromiso");
            Property(x => x.prioridad).HasColumnName("prioridad");
            Property(x => x.comentariosCount).HasColumnName("comentariosCount");
            Property(x => x.enVersion).HasColumnName("enVersion");
            Property(x => x.revisaID).HasColumnName("revisaID");
            Property(x => x.revisa).HasColumnName("revisa");
            HasRequired(x => x.minuta).WithMany(x => x.actividades).HasForeignKey(y => y.minutaID);
            ToTable("tblSA_Actividades");
        }
    }
}
