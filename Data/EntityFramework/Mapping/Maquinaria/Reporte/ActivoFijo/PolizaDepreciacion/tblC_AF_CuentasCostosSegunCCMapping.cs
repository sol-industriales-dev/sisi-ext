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
    public class tblC_AF_CuentasCostosSegunCCMapping : EntityTypeConfiguration<tblC_AF_CuentasCostosSegunCC>
    {
        public tblC_AF_CuentasCostosSegunCCMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.CcActivoFijo).HasColumnName("ccActivoFijo");
            Property(x => x.IdCuenta).HasColumnName("idCuenta");
            Property(x => x.Estatus).HasColumnName("estatus");
            HasRequired(x => x.Cuenta).WithMany().HasForeignKey(y => y.IdCuenta);
            ToTable("tblC_AF_CuentasCostosSegunCC");
        }
    }
}
