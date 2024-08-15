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
    public class tblC_AF_ExcelNoMaquinasMapping : EntityTypeConfiguration<tblC_AF_ExcelNoMaquinas>
    {
        public tblC_AF_ExcelNoMaquinasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.numero).HasColumnName("numero");
            Property(x => x.cuenta).HasColumnName("cuenta");
            Property(x => x.subcuenta).HasColumnName("subcuenta");
            Property(x => x.subsubcuenta).HasColumnName("subsubcuenta");
            Property(x => x.fechaAlta).HasColumnName("fechaAlta");
            Property(x => x.fechaInicioDep).HasColumnName("fechaInicioDep");
            Property(x => x.poliza).HasColumnName("poliza");
            Property(x => x.tp).HasColumnName("tp");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.moi).HasColumnName("moi");
            Property(x => x.altas).HasColumnName("altas");
            Property(x => x.componentes).HasColumnName("componentes");
            Property(x => x.fechaBaja).HasColumnName("fechaBaja");
            Property(x => x.polizaBaja).HasColumnName("polizaBaja");
            Property(x => x.tpBaja).HasColumnName("tpBaja");
            Property(x => x.porcentaje).HasColumnName("porcentaje");
            Property(x => x.mesesDep).HasColumnName("mesesDep");
            ToTable("tblC_AF_ExcelNoMaquinas");
        }
    }
}
