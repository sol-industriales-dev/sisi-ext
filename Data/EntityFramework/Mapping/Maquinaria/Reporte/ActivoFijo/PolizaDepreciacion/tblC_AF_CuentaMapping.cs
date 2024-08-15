using Core.Entity.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion
{
    public class tblC_AF_CuentaMapping : EntityTypeConfiguration<tblC_AF_Cuenta>
    {
        public tblC_AF_CuentaMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Cuenta).HasColumnName("cuenta");
            Property(x => x.Descripcion).HasColumnName("descripcion");
            Property(x => x.TipoCuentaId).HasColumnName("tipoCuentaId");
            Property(x => x.Estatus).HasColumnName("Estatus");
            HasRequired(x => x.TipoCuenta).WithMany().HasForeignKey(y => y.TipoCuentaId);
            ToTable("tblC_AF_Cuenta");
        }
    }
}