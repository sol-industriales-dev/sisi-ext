using Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class CapacitacionSeguridadEmpleadoPrivilegioMapping : EntityTypeConfiguration<tblS_CapacitacionSeguridadEmpleadoPrivilegio>
    {
        public CapacitacionSeguridadEmpleadoPrivilegioMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idUsuario).HasColumnName("idUsuario");
            Property(x => x.puesto_id).HasColumnName("puesto_id");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.idPrivilegio).HasColumnName("idPrivilegio");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            Property(x => x.division).HasColumnName("division");
            ToTable("tblS_CapacitacionSeguridadEmpleadoPrivilegio");
        }
    }
}
