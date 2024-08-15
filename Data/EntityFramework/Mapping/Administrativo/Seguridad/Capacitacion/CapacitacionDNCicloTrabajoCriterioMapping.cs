using Core.Entity.Administrativo.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Capacitacion
{
    public class CapacitacionDNCicloTrabajoCriterioMapping : EntityTypeConfiguration<tblS_CapacitacionDNCicloTrabajoCriterio>
    {
        public CapacitacionDNCicloTrabajoCriterioMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.ponderacion).HasColumnName("ponderacion");
            Property(x => x.cicloTrabajoID).HasColumnName("cicloTrabajoID");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblS_CapacitacionDNCicloTrabajoCriterio");
        }
    }
}
