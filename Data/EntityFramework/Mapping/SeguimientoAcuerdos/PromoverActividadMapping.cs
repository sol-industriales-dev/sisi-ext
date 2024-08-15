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
    public class PromoverActividadMapping : EntityTypeConfiguration<tblSA_PromoverActividad>
    {
        public PromoverActividadMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.actividadID).HasColumnName("actividadID");
            Property(x => x.observacion).HasColumnName("observacion");
            Property(x => x.columna).HasColumnName("columna");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.jefeID).HasColumnName("jefeID");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            Property(x => x.fechaAccion).HasColumnName("fechaAccion");
            Property(x => x.accion).HasColumnName("accion");
            Property(x => x.responsableID).HasColumnName("responsableID");

            HasRequired(x => x.actividad).WithMany().HasForeignKey(y => y.actividadID);
            ToTable("tblSA_PromoverActividad");
        }
    }
}
