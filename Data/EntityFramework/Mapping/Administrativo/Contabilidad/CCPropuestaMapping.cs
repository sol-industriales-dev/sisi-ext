using Core.Entity.Administrativo.Contabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad
{
    class CCPropuestaMapping : EntityTypeConfiguration<tblC_RelCCPropuesta>
    {
        public CCPropuestaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.ccPrincipal).HasColumnName("ccPrincipal");
            Property(x => x.ccSecundario).HasColumnName("ccSecundario");
            ToTable("tblC_RelCCPropuesta");
        }
    }
}
