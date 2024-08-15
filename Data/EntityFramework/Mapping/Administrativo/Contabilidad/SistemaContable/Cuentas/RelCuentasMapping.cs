using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.Cuentas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.SistemaContable.Cuentas
{
    public class RelCuentasMapping : EntityTypeConfiguration<tblC_Cta_RelCuentas>
    {
        public RelCuentasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.palEmpresa).HasColumnName("palEmpresa");
            Property(x => x.palCta).HasColumnName("palCta");
            Property(x => x.palScta).HasColumnName("palScta");
            Property(x => x.palSscta).HasColumnName("palSscta");
            Property(x => x.secEmpresa).HasColumnName("secEmpresa");
            Property(x => x.secCta).HasColumnName("secCta");
            Property(x => x.secScta).HasColumnName("secScta");
            Property(x => x.secSscta).HasColumnName("secSscta");
            Property(x => x.idUsuarioRegistro).HasColumnName("idUsuarioRegistro");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            ToTable("tblC_Cta_RelCuentas");
        }
    }
}
