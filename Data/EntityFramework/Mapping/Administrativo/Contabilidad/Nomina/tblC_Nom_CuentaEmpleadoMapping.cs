using Core.Entity.Administrativo.Contabilidad.Nomina;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_CuentaEmpleadoMapping : EntityTypeConfiguration<tblC_Nom_CuentaEmpleado>
    {
        public tblC_Nom_CuentaEmpleadoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasColumnName("id");
            Property(x => x.numero).HasColumnName("numero");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.apellidoPaterno).HasColumnName("apellidoPaterno");
            Property(x => x.apellidoMaterno).HasColumnName("apellidoMaterno");
            Property(x => x.cuentaId).HasColumnName("cuentaId");
            Property(x => x.cta).HasColumnName("cta");
            Property(x => x.scta).HasColumnName("scta");
            Property(x => x.sscta).HasColumnName("sscta");
            Property(x => x.digito).HasColumnName("digito");
            Property(x => x.cuentaDescripcion).HasColumnName("cuentaDescripcion");
            Property(x => x.validada).HasColumnName("validada");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.ccDescripcion).HasColumnName("ccDescripcion");
            Property(x => x.usuarioValidoId).HasColumnName("usuarioValidoId");
            Property(x => x.fechaValidacion).HasColumnName("fechaValidacion");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.usuarioCreacionId).HasColumnName("usuarioCreacionId");
            Property(x => x.usuarioModificacionId).HasColumnName("usuarioModificacionId");
            HasRequired(x => x.cuenta).WithMany().HasForeignKey(y => y.cuentaId);
            HasOptional(x => x.usuarioValido).WithMany().HasForeignKey(y => y.usuarioValidoId);
            HasRequired(x => x.usuarioCreacion).WithMany().HasForeignKey(y => y.usuarioCreacionId);
            HasRequired(x => x.usuarioModificacion).WithMany().HasForeignKey(y => y.usuarioModificacionId);
            ToTable("tblC_Nom_CuentaEmpleado");
        }
    }
}
