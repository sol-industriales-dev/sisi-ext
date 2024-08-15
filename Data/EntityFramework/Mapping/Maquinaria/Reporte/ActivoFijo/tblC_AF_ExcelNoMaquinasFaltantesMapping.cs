using Core.Entity.Maquinaria.Reporte.ActivoFijo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Reporte.ActivoFijo
{
    public class tblC_AF_ExcelNoMaquinasFaltantesMapping : EntityTypeConfiguration<tblC_AF_ExcelNoMaquinasFaltantes>
    {
        public tblC_AF_ExcelNoMaquinasFaltantesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.numero).HasColumnName("numero");
            Property(x => x.cuenta).HasColumnName("cuenta");
            Property(x => x.poliza).HasColumnName("poliza");
            Property(x => x.tp).HasColumnName("tp");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.monto).HasColumnName("monto");
            Property(x => x.motivo).HasColumnName("motivo");
            ToTable("tblC_AF_ExcelNoMaquinasFaltantes");
        }
    }
}
