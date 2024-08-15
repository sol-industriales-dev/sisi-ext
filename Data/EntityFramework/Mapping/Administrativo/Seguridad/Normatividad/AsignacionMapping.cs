using Core.Entity.Administrativo.Seguridad.Requerimientos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Requerimientos
{
    public class AsignacionMapping : EntityTypeConfiguration<tblNOM_Asignacion>
    {
        public AsignacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.normaID).HasColumnName("normaID");
            Property(x => x.fechaAsignacion).HasColumnName("fechaAsignacion");
            Property(x => x.fechaInicioEvaluacion).HasColumnName("fechaInicioEvaluacion");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblNOM_Asignacion");
        }
    }
}
