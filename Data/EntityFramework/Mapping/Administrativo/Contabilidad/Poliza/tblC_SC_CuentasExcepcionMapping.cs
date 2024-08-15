using Core.Entity.Administrativo.Contabilidad.Poliza;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Poliza
{
    public class tblC_SC_CuentasExcepcionMapping : EntityTypeConfiguration<tblC_SC_CuentasExcepcion>
    {
        public tblC_SC_CuentasExcepcionMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cuenta).HasColumnName("cuenta");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.usuarioCaptura).HasColumnName("usuarioCaptura");
            Property(x => x.fechaModifica).HasColumnName("fechaModifica");
            Property(x => x.usuarioModifica).HasColumnName("usuarioModifica");
            Property(x => x.esActivo).HasColumnName("esActivo");
            ToTable("tblC_SC_CuentasExcepcion");
        }
    }
}