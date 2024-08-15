using Core.Entity.Maquinaria.Reporte.ActivoFijo.Colombia;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Reporte.ActivoFijo.Colombia
{
    public class tblC_AF_CuentaColombiaMapping : EntityTypeConfiguration<tblC_AF_CuentaColombia>
    {
        public tblC_AF_CuentaColombiaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cuenta).HasColumnName("cuenta");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.tipoCuentaId).HasColumnName("tipoCuentaId");
            Property(x => x.colombiaMexico).HasColumnName("colombiaMexico");
            Property(x => x.cuentaIdMexico).HasColumnName("cuentaIdMexico");
            Property(x => x.estatus);
            HasRequired(x => x.tipoCuenta).WithMany().HasForeignKey(y => y.tipoCuentaId);
            HasOptional(x => x.cuentaMexico).WithMany().HasForeignKey(y => y.cuentaIdMexico);
            ToTable("tblC_AF_CuentaColombia");
        }
    }
}
