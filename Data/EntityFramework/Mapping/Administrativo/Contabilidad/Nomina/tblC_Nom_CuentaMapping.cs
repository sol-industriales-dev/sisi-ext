using Core.Entity.Administrativo.Contabilidad.Nomina;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_CuentaMapping : EntityTypeConfiguration<tblC_Nom_Cuenta>
    {
        public tblC_Nom_CuentaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cta).HasColumnName("cta");
            Property(x => x.scta).HasColumnName("scta");
            Property(x => x.sscta).HasColumnName("sscta");
            Property(x => x.digito).HasColumnName("digito");
            Property(x => x.tipoCuentaId).HasColumnName("tipoCuentaId");
            Property(x => x.tipoNominaId).HasColumnName("tipoNominaId");
            Property(x => x.clasificacionCcId).HasColumnName("clasificacionCcId");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.usuarioCreacionId).HasColumnName("usuarioCreacionId");
            Property(x => x.usuarioModificacionId).HasColumnName("usuarioModificacionId");
            HasRequired(x => x.tipoCuenta).WithMany().HasForeignKey(y => y.tipoCuentaId);
            HasRequired(x => x.tipoNomina).WithMany().HasForeignKey(y => y.tipoNominaId);
            HasRequired(x => x.clasificacionCc).WithMany().HasForeignKey(y => y.clasificacionCcId);
            HasRequired(x => x.usuarioCreacion).WithMany().HasForeignKey(y => y.usuarioCreacionId);
            HasRequired(x => x.usuarioModificacion).WithMany().HasForeignKey(y => y.usuarioModificacionId);
            ToTable("tblC_Nom_Cuenta");
        }
    }
}
