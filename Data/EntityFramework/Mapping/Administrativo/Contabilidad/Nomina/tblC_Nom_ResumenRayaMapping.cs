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
    public class tblC_Nom_ResumenRayaMapping : EntityTypeConfiguration<tblC_Nom_ResumenRaya>
    {
        public tblC_Nom_ResumenRayaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nominaId).HasColumnName("nominaId");
            Property(x => x.cuentaId).HasColumnName("cuentaId");
            Property(x => x.cta).HasColumnName("cta");
            Property(x => x.concepto).HasColumnName("concepto");
            Property(x => x.monto).HasColumnName("monto");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.usuarioCreacionId).HasColumnName("usuarioCreacionId");
            Property(x => x.usuarioModificacionId).HasColumnName("usuarioModificacionId");
            HasRequired(x => x.nomina).WithMany().HasForeignKey(y => y.nominaId);
            HasRequired(x => x.cuenta).WithMany().HasForeignKey(y => y.cuentaId);
            HasRequired(x => x.usuarioCreacion).WithMany().HasForeignKey(y => y.usuarioCreacionId);
            HasRequired(x => x.usuarioModificacion).WithMany().HasForeignKey(y => y.usuarioModificacionId);
            ToTable("tblC_Nom_ResumenRaya");
        }
    }
}
