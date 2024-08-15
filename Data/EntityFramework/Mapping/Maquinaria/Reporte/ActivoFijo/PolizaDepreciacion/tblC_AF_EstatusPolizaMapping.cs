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
    public class tblC_AF_EstatusPolizaMapping : EntityTypeConfiguration<tblC_AF_EstatusPoliza>
    {
        public tblC_AF_EstatusPolizaMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Estatus).HasColumnName("estatus");
            Property(x => x.Descripcion).HasColumnName("descripcion");
            ToTable("tblC_AF_EstatusPoliza");
        }
    }
}