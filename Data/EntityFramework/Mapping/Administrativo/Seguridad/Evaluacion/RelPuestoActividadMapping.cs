using Core.Entity.Administrativo.Seguridad.Evaluacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Evaluacion
{
    public class RelPuestoActividadMapping : EntityTypeConfiguration<tblSED_RelPuestoActividad>
    {
        public RelPuestoActividadMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.puestoID).HasColumnName("puestoID");
            Property(x => x.actividadID).HasColumnName("actividadID");
            Property(x => x.periodicidad).HasColumnName("periodicidad");
            Property(x => x.division).HasColumnName("division");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblSED_RelPuestoActividad");
        }
    }
}
