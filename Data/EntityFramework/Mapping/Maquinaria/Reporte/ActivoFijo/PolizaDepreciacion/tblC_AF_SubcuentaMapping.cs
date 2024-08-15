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
    public class tblC_AF_SubcuentaMapping : EntityTypeConfiguration<tblC_AF_Subcuenta>
    {
        public tblC_AF_SubcuentaMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.CuentaId).HasColumnName("cuentaId");
            Property(x => x.Subcuenta).HasColumnName("subcuenta");
            Property(x => x.SubSubcuenta).HasColumnName("subSubcuenta");
            Property(x => x.Descripcion).HasColumnName("descripcion");
            Property(x => x.EsMaquinaria).HasColumnName("esMaquinaria");
            Property(x => x.EsOverhaul).HasColumnName("esOverhaul");
            Property(x => x.meses).HasColumnName("meses");
            Property(x => x.porcentaje).HasColumnName("porcentaje");
            Property(x => x.ConceptoDepreciacion).HasColumnName("conceptoDepreciacion");
            Property(x => x.Estatus).HasColumnName("estatus");
            HasRequired(x => x.Cuenta).WithMany().HasForeignKey(y => y.CuentaId);
            ToTable("tblC_AF_Subcuenta");
        }
    }
}