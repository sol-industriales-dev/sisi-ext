using Core.Entity.Maquinaria.Reporte.ActivoFijo;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Reporte.ActivoFijo
{
    public class tblC_AF_ComportamientoDePolizaMapping : EntityTypeConfiguration<tblC_AF_ComportamientoDePoliza>
    {
        public tblC_AF_ComportamientoDePolizaMapping()
        {
            HasKey(x => x.id);
            ToTable("tblC_AF_ComportamientoDePoliza");
        }
    }
}
