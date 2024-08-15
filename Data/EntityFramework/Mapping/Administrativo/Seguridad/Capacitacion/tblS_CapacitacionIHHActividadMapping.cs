using Core.Entity.Administrativo.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionIHHActividadMapping : EntityTypeConfiguration<tblS_CapacitacionIHHActividad>
    {
        public tblS_CapacitacionIHHActividadMapping()
        {
            HasKey(x => x.id);
            HasRequired(x => x.usuarioCreacion).WithMany().HasForeignKey(x => x.usuarioCreacionId);
            HasRequired(x => x.usuarioModificacion).WithMany().HasForeignKey(x => x.usuarioModificacionId);
            ToTable("tblS_CapacitacionIHHActividad");
        }
    }
}
