using Core.Entity.Maquinaria.Reporte.ActivoFijo.Peru;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Reporte.ActivoFijo.Peru
{
    public class tblC_AF_CuentaPeruMapping : EntityTypeConfiguration<tblC_AF_CuentaPeru>
    {
        public tblC_AF_CuentaPeruMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cuenta).HasColumnName("cuenta");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.tipoCuentaId).HasColumnName("tipoCuentaId");
            Property(x => x.peruMexico).HasColumnName("peruMexico");
            Property(x => x.cuentaIdMexico).HasColumnName("cuentaIdMexico");
            Property(x => x.estatus);
            HasRequired(x => x.tipoCuenta).WithMany().HasForeignKey(y => y.tipoCuentaId);
            HasOptional(x => x.cuentaMexico).WithMany().HasForeignKey(y => y.cuentaIdMexico);
            ToTable("tblC_AF_CuentaPeru");
        }
    }
}
